using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace OutwardSaveTransfer
{
    public partial class Form2 : Form
    {
        private static string loadDirectory;
        private static ConsoleWindowClass console;
        private static bool transfering = false;
        //static readonly Regex hierarchyRegex = new Regex(@"(<Hierarchy>)(.*)(<\/Hierarchy>)");

        static readonly Dictionary<string, string> StashAreaToStashUID = new Dictionary<string, string>()
        {
            {
                "Berg",
                "ImqRiGAT80aE2WtUHfdcMw"
            },
            {
                "CierzoNewTerrain",
                "ImqRiGAT80aE2WtUHfdcMw"
            },
            {
                "Levant",
                "ZbPXNsPvlUeQVJRks3zBzg"
            },
            {
                "Monsoon",
                "ImqRiGAT80aE2WtUHfdcMw"
            },
            {
                "Harmattan",
                "ImqRiGAT80aE2WtUHfdcMw"
            },
            {
                "NewSirocco",
                "IqUugGqBBkaOcQdRmhnMng"
            }
        };

        public Form2(string saveDirectory, int savesChars)
        {
            InitializeComponent();
            label2.Text = "Total saved characters found: " + savesChars;
            loadDirectory = saveDirectory;

            console = new ConsoleWindowClass(consoleWindow);
        }

        private void file_browser_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    textBox1.Text = fbd.SelectedPath;
            }
        }

        private static void MigrateAll(string saveDirectory)
        {
            try
            {
                // Create the base SaveGames folder in case it doesnt exist
                Directory.CreateDirectory(Path.Combine(saveDirectory, "SaveGames"));

                // Create a temp folder for us to work with
                string tempSavesFolder = Path.Combine(saveDirectory, "SaveGames_TEMP");

                // Check vanilla saves folder
                string vanillaSavesFolder = Path.Combine(loadDirectory, "SaveGames");

                if (!Directory.Exists(vanillaSavesFolder))
                {
                    console.Print_error_text("\nThere is no 'SaveGames' folder in your selected folder.");
                    return;
                }

                // Copy vanilla saves into temp folder
                console.Print_text("\nCopying save files to destinations temporary location!");
                CopyAll(new DirectoryInfo(vanillaSavesFolder), new DirectoryInfo(tempSavesFolder));

                // Process saves in temp folder
                foreach (string baseSaveFolder in Directory.GetDirectories(tempSavesFolder))
                {
                    console.Print_text("\nProcessing save folder: " + Path.GetFileName(baseSaveFolder) + "...");

                    foreach (string characterFolder in Directory.GetDirectories(baseSaveFolder))
                    {
                        if (characterFolder.Contains("Save_"))
                        {
                            console.Print_text("\n\nProcessing character folder: " + Path.GetFileName(characterFolder) + "...");

                            foreach (string saveInstanceFolder in Directory.GetDirectories(characterFolder))
                            {
                                console.Print_text("\n\nProcessing save instance: " + Path.GetFileName(saveInstanceFolder) + "...");

                                ProcessSaveStashes(saveInstanceFolder);

                                FixFileNames(saveInstanceFolder);
                            }
                        }
                        else
                        {
                            console.Print_text("\nFound not a save instance: " + Path.GetFileName(characterFolder) + "...");
                        }

                    }
                }

                console.Print_text("\nRemoving temporary location and moving files to SaveGames...");
                // Move temp folders into real save dir
                foreach (string steamIdDir in Directory.GetDirectories(tempSavesFolder))
                {
                    try
                    {
                        // Try move the entire steam ID folder
                        Directory.Move(steamIdDir, steamIdDir.Replace("SaveGames_TEMP", "SaveGames"));
                    }
                    catch
                    {
                        // If it failed, move each subdir
                        foreach (string charDir in Directory.GetDirectories(steamIdDir))
                        {
                            try
                            {
                                // Try move the character UID folder
                                Directory.Move(charDir, charDir.Replace("SaveGames_TEMP", "SaveGames"));
                            }
                            catch
                            {
                                // Character already exists, ignore
                            }
                        }
                    }
                }

                Directory.Delete(tempSavesFolder, true);
            }
            catch (Exception ex)
            {
                console.Print_error_text("\nCaught exception. Error: " + ex);
            }

            console.Print_success_text("\nFinished!");
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);
            string fileName;

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fileName = Path.Combine(target.ToString(), fi.Name);

                if(File.Exists(fileName))
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }

                fi.CopyTo(fileName);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        static void ProcessSaveStashes(string saveInstanceFolder)
        {
            string savePath = Path.Combine(saveInstanceFolder, "char0.charc");
            if (!File.Exists(savePath))
            {
                console.Print_error_text($"\nMissing char0.charc file in folder \"{saveInstanceFolder}\"!");
                return;
            }

            //StreamReader streamReader = new StreamReader(savePath);
            //GZipStream gzipStream = new GZipStream(streamReader.BaseStream, CompressionMode.Decompress);
            //BinaryReader binaryReader = new BinaryReader(gzipStream, Encoding.UTF8);

            //console.Print_text("\n" + binaryReader.ReadString());
            GZipStream gzipStream = new GZipStream(File.OpenRead(savePath), CompressionMode.Decompress);
            StreamReader streamReader = new StreamReader(gzipStream);

            CharacterSaveFile fnf = new CharacterSaveFile();
            console.Print_text($"\nReading file: {Path.GetFileName(savePath)}");
            XmlDocument charXml = fnf.Desirelize(streamReader.ReadToEnd());
            console.Print_text("\n" + streamReader.ReadToEnd());

            streamReader.Close();
            gzipStream.Close();

            string area, stashUID, filePath;
            bool noPlaceError = false;

            console.Print_text("\nFixing stashed items...");

            foreach (KeyValuePair<string, string> entry in StashAreaToStashUID)
            {
                area = entry.Key;
                stashUID = entry.Value;

                try
                {
                    EnvironmentSaveFile save = new EnvironmentSaveFile();
                    save.SetAreaName(area);

                    filePath = Path.Combine(saveInstanceFolder, $"{area}.envc");

                    if (File.Exists(filePath))
                    {
                        XmlDocument xml;

                        GZipStream gzipStreamEnv = new GZipStream(File.OpenRead(filePath), CompressionMode.Decompress);
                        StreamReader streamReaderEnv = new StreamReader(gzipStreamEnv);

                        xml = save.FixStashDesirelize(streamReaderEnv.ReadToEnd(), ref fnf, stashUID);

                        streamReaderEnv.Close();
                        gzipStreamEnv.Close();

                        SaveXmlOutwardFile(filePath, xml, ref console);
                    }
                    else
                    {
                        console.Print_error_text($"\nMissing \"{area}.envc\" file! Character name \"{fnf.GetPSaveData().GetName()}\".");
                        noPlaceError = true;
                    }
                }
                catch(Exception ex)
                {
                    console.Print_text("\nException detected. Error...\n" + ex);
                }
            }

            if(noPlaceError)
            {
                console.Print_success_text("\nPossible that character was played in split screen mode or never travelled to locations in that case it's FINE!");
            }

            console.Print_text($"\nLoaded {fnf.stashedItemLists.Count} stashed items and {fnf.GetPSaveData().GetStashedMoney()} silver.");
            console.Print_text($"\nSaving stashed items: {fnf.stashedItemLists.Count}...");
            charXml = fnf.FillStashesXmlDocument(charXml);
            SaveXmlOutwardFile(savePath, charXml, ref console);
        }

        public static void SaveXmlOutwardFile(string savePath, XmlDocument xml, ref ConsoleWindowClass console)
        {
            try
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                using (var fs = File.Create(savePath))
                {
                    using (var gz = new GZipStream(fs, CompressionMode.Compress))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.Encoding = new UTF8Encoding(false);
                        using (var writer = XmlWriter.Create(gz, settings))
                        {
                            xml.Save(gz);
                        }
                    }
                }

                console.Print_success_text($"\nSaved file: \"{savePath}\"!");
            }
            catch(Exception ex)
            {
                console.Print_error_text("\nCaught exception. Error: " + ex);
            }
        }

        static void FixFileNames(string saveInstanceFolder)
        {
            console.Print_text("\nFixing file extensions...");

            // Update file extensions
            foreach (string file in Directory.GetFiles(saveInstanceFolder))
            {
                string directory = Path.GetDirectoryName(file);
                string name = Path.GetFileNameWithoutExtension(file);

                string ext = Path.GetExtension(file);

                switch (ext)
                {
                    case ".envc":
                        {
                            ext = ".deenvc";
                            break;
                        }
                    case ".charc":
                        {
                            ext = ".defedc";
                            break;
                        }
                    case ".legacyc":
                        {
                            ext = ".defedc";
                            break;
                        }
                    case ".mapc":
                        {
                            ext = ".defedc";
                            break;
                        }
                    case ".worldc":
                        {
                            ext = ".defedc";
                            break;
                        }
                    default:
                        {
                            ext = string.Empty;
                            break;
                        }
                };

                if (ext == string.Empty)
                    continue;

                File.Move(file, Path.Combine(directory, $"{name}{ext}"));

                console.Print_success_text($"\nFixed extension for file: \"{file}\"!");
            }
        }

        private async void check_save_location_button_Click(object sender, EventArgs e)
        {
            //string n = @"F:\Games\outTest\SaveGames_TEMP\76561197960267366\Save_4gpGPEyWsUygMX0QlWiYZA\20220305030732";
            //ProcessSaveStashes(n);
            //return;
            if(Directory.Exists(textBox1.Text))
            {
                if (!transfering)
                {
                    console.Print_success_text("Directory exist!");
                    transfering = true;

                    await Task.Run(() =>
                    {
                        MigrateAll(textBox1.Text);
                        transfering = false;
                    });
                }
                else
                {
                    console.Print_error_text("\nProgram already started transfering files!");
                }
            }
            else
            {
                console.Print_error_text("\nYour typed in directory doesn't exist!");
            }
        }

        private void Form2_Closing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void consoleWindow_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
