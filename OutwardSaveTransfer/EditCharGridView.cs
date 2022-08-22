using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OutwardSaveTransfer;
using System.Xml;

namespace OutwardSaveFixer
{
    class EditCharGridView
    {
        DataGridView charGrid;
        string charsLocation;

        List<characterInfo> charsList = new List<characterInfo>();

        struct indexNumber
        {
            public int index;
            public Int64 number;

            public indexNumber(int index, Int64 number)
            {
                this.index = index;
                this.number = number;
            }
        }
;
        public EditCharGridView(DataGridView gridView, string charsLocation)
        {
            this.charGrid = gridView;
            this.charsLocation = charsLocation;

            Add_Columns();
            Get_Characters_Info();
        }

        public void Reset_Grid(string location)
        {
            charsLocation = location;
            charGrid.Rows.Clear();

            Get_Characters_Info();
            charGrid.Refresh();
        }

        private void Add_Columns()
        {
            DataGridViewCheckBoxColumn CheckBoxCol = new DataGridViewCheckBoxColumn();
            CheckBoxCol.HeaderText = "Select";

            charGrid.Columns.Add(CheckBoxCol);
            charGrid.Columns.Add("Name", "Name");
            charGrid.Columns.Add("SteamFolder", "Steam ID");
            charGrid.Columns.Add("EncodedFolder", "Encoded Folder");

            DataGridViewCheckBoxColumn CheckBoxDefCol = new DataGridViewCheckBoxColumn();
            CheckBoxDefCol.HeaderText = "Definitive Edition";
            charGrid.Columns.Add(CheckBoxDefCol);

            foreach (DataGridViewColumn column in charGrid.Columns)
            {
                if (column.Index > 0)
                {
                    column.ReadOnly = true;
                }
            }
        }

        private void Get_Characters_Info()
        {
            string saveGamesDirectory = this.charsLocation + "\\SaveGames".Replace(@"\\", @"\");
            var directories = Directory.GetDirectories(saveGamesDirectory);

            Get_All_Saves(saveGamesDirectory, directories);
        }

        private void Get_All_Saves(string saveGameDirectory, string[] steamIds)
        {
            int totalSteamIds = steamIds.Length;
            int directoriesLength;
            string saveWithDate;

            for (int currentSteamId = 0; currentSteamId < totalSteamIds; currentSteamId++)
            {
                var directories = Directory.GetDirectories(steamIds[currentSteamId]);
                directoriesLength = directories.Length;

                for (int currentSaveDirectory = 0; currentSaveDirectory < directoriesLength; currentSaveDirectory++)
                {
                    if (directories[currentSaveDirectory].Contains("Save_"))
                    {
                        saveWithDate = Get_Path_With_Newest_Data(directories[currentSaveDirectory]);

                        if (saveWithDate != "")
                        {
                            CharacterSaveFile csf;
                            bool isDefinitiveEdition;

                            if (Get_Character_Info(saveWithDate, out csf, out isDefinitiveEdition))
                            {
                                charsList.Add(new characterInfo(directories[currentSaveDirectory], csf, isDefinitiveEdition));
                                charGrid.Rows.Add(false, csf.GetPSaveData().GetName(), charsList[charsList.Count - 1].Get_Steam_ID(), charsList[charsList.Count - 1].Get_Encoded_Folder(), isDefinitiveEdition);
                            }
                        }
                    }
                }
            }
        }

        public string Get_Path_With_Newest_Data(string path)
        {
            if(Directory.Exists(path))
            {
                indexNumber highiestData = new indexNumber(0, 0);

                var directories = Directory.GetDirectories(path);
                int directoriesLength = directories.Length;
                int lastFolderStartIndex;

                Int64 saveDate;

                for (int currentDateSave = 0; currentDateSave < directoriesLength; currentDateSave++)
                {
                    lastFolderStartIndex = directories[currentDateSave].LastIndexOf("\\") + 1;
                    Int64.TryParse(directories[currentDateSave].Substring(lastFolderStartIndex, directories[currentDateSave].Length - lastFolderStartIndex), out saveDate);

                    if (saveDate > 0 && saveDate > highiestData.number)
                    {
                        highiestData = new indexNumber(currentDateSave, saveDate);
                    }
                }

                return directories[highiestData.index];
            }

            return "";
        }

        private Boolean Get_Character_Info(string saveInstanceFolder, out CharacterSaveFile charSaveFile, out bool isDefinitiveEdition)
        {
            charSaveFile = new CharacterSaveFile();
            string savePath = Path.Combine(saveInstanceFolder, "char0.defedc");

            if (!File.Exists(savePath))
            {
                savePath = Path.Combine(saveInstanceFolder, "char0.charc");
                isDefinitiveEdition = false;

                if (!File.Exists(savePath))
                {
                    return false;
                }
            }
            else
            {
                isDefinitiveEdition = true;
            }

            GZipStream gzipStream = new GZipStream(File.OpenRead(savePath), CompressionMode.Decompress);
            StreamReader streamReader = new StreamReader(gzipStream);

            XmlDocument charXml = charSaveFile.Desirelize(streamReader.ReadToEnd());

            streamReader.Close();
            gzipStream.Close();

            return true;
        }

        public List<characterInfo> Get_CheckBoxes()
        {
            int totalRows = charGrid.Rows.Count;

            for (int currentRow = 0; currentRow < totalRows; currentRow++)
            {
                object value = charGrid.Rows[currentRow].Cells[0].Value;
                if (value != null && (Boolean)value)
                {
                    charsList[currentRow].Set_Mark((Boolean)value);
                }
            }

            return charsList;
        }
    }
}
