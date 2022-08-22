using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using OutwardSaveTransfer;

namespace OutwardSaveFixer
{
    public partial class EntryForm : Form
    {
        List<OSFPanel> listPanel = new List<OSFPanel>();
        int panelIndex;
        EditCharGridView charGrid;
        List<characterInfo> charsInfo;
        private static ConsoleWindowClass editConsole;

        public enum races
        {
            Auraian,
            Tramon,
            Kazite
        }

        enum PVisualIndependentData
        {
            Face,
            HairStyle,
            HairColor
        }

        struct SaveLocator
        {
            public Boolean isLocationGood;
            public int totalSaves;

            public SaveLocator(Boolean isLocationGood, int totalSave)
            {
                this.isLocationGood = isLocationGood;
                this.totalSaves = totalSave;
            }
        }

        string[] oldConversionsENVC = new string[]
        {
            "Hallowed",
            "Abrassar",
            "AntiqueField",
            "Berg",
            "Caldera",
            "Chersonese",
            "Cierzo",
            "Emercar",
            "Harmattan",
            "Levant",
            "Monsoon",
            "NewSirocco"
        };

        string[] oldConversionsLEGACYC = new string[]
        {
            "LegacyChests"
        };

        string[] oldConversionsMAPC = new string[]
        {
            "Map"
        };

        string[] oldConversionsWORLDC = new string[]
        {
            "World"
        };

        public EntryForm()
        {
            InitializeComponent();
        }

        private void button_leave(object sender, EventArgs e)
        {
            on_button_leave((sender as Button));
        }

        private void button_hover(object sender, EventArgs e)
        {
            on_button_hover((sender as Button));
        }

        private void on_button_hover(Button btn)
        {
            btn.BackColor = Color.Green;
        }

        private void on_button_leave(Button btn)
        {
            btn.BackColor = Color.Transparent;
        }

        private void EntryForm_Load(object sender, EventArgs e)
        {
            //root
            listPanel.Add(new OSFPanel(EntryFormPanel, EntryFormPanel));

            //children 1
            listPanel.Add(new OSFPanel(TransferAllSavesDE, EntryFormPanel));
            listPanel.Add(new OSFPanel(TransferAllSavesOV, EntryFormPanel));
            listPanel.Add(new OSFPanel(EditCharPanel, EntryFormPanel));

            //children 2
            listPanel.Add(new OSFPanel(EditCharMainPanel, EditCharPanel));

            //children 3
            listPanel.Add(new OSFPanel(EditSpecificCharPanel, EditCharMainPanel));

            //children 4
            listPanel.Add(new OSFPanel(EditSaveLocationPanel, EditSpecificCharPanel));

            //children 5
            listPanel.Add(new OSFPanel(EditConsolePanel, EditSaveLocationPanel));

            Redraw_Menu();
        }

        private void Redraw_Menu()
        {
            listPanel[panelIndex].Get_Panel().BringToFront();

            foreach (OSFPanel panele in listPanel)
            {
                panele.Get_Panel().Hide();
            }

            listPanel[panelIndex].Get_Panel().Show();
        }

        private int Get_Panel_Index(string name)
        {
            int totalPanels = listPanel.Count;

            for (int currentPanel = 0; currentPanel < totalPanels; currentPanel++)
            {
                if (listPanel[currentPanel].Get_Panel().Name == name)
                {
                    return currentPanel;
                }
            }

            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelIndex = Get_Panel_Index("TransferAllSavesDE");
            Redraw_Menu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelIndex = Get_Panel_Index("TransferAllSavesOV");
            Redraw_Menu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelIndex = Get_Panel_Index("EditCharPanel");
            Redraw_Menu();
        }

        private void back_button_Click(object sender, EventArgs e)
        {
            panelIndex = Get_Panel_Index(listPanel[panelIndex].Get_Parent_Panel().Name);
            Redraw_Menu();
        }

        private void file_browser_edit_char_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    EditCharTextBox.Text = fbd.SelectedPath;
            }
        }

        private void file_browser_older_version_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    SOVTextBox.Text = fbd.SelectedPath;
            }
        }

        private void file_browser_DE_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    DETextBox.Text = fbd.SelectedPath;
            }
        }

        private void file_browser_EditOutPut_button_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    EditOutputTextBox.Text = fbd.SelectedPath;
            }
        }

        private void EditCharSelect_Click(object sender, EventArgs e)
        {
            SaveLocator saveInfo = Check_Location(EditCharTextBox.Text);

            if (saveInfo.isLocationGood)
            {
                panelIndex = Get_Panel_Index("EditCharMainPanel");
                Redraw_Menu();

                if (charGrid == null)
                {
                    charGrid = new EditCharGridView(editCharSelectGridView, EditCharTextBox.Text);
                }
                else
                {
                    charGrid.Reset_Grid(EditCharTextBox.Text);
                }
            }
        }

        private void SOVSelect_Click(object sender, EventArgs e)
        {
            SaveLocator saveInfo = Check_Location(SOVTextBox.Text);

            if (saveInfo.isLocationGood)
            {

            }
        }

        private void DESelect_Click(object sender, EventArgs e)
        {
            SaveLocator saveInfo = Check_Location(DETextBox.Text);

            if (saveInfo.isLocationGood)
            {
                openTransfer(DETextBox.Text, saveInfo.totalSaves);
            }
        }

        private SaveLocator Check_Location(string mainLocation)
        {
            if (mainLocation != "")
            {
                if (Directory.Exists(mainLocation))//SaveGames
                {
                    string saveGamesDirectory = mainLocation + "\\SaveGames".Replace(@"\\", @"\");

                    if (Directory.Exists(saveGamesDirectory))
                    {
                        var directories = Directory.GetDirectories(saveGamesDirectory);

                        if (directories.Length > 0)
                        {
                            int totalSaves = getTotalSaves(saveGamesDirectory, directories);
                            
                            MessageBox.Show("We managed to locate your save files! Total saved characters found " + totalSaves + "!", "Success");
                            return new SaveLocator(true, totalSaves);
                        }
                        else
                        {
                            MessageBox.Show("Missing saves in '" + saveGamesDirectory + "'", "Failed!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Missing 'SaveGames' directory in " + mainLocation, "Failed!");
                    }
                }
                else
                {
                    MessageBox.Show("Typed in directory doesn't exist!", "Failed!");
                }
            }
            else
            {
                MessageBox.Show("You forgot to type in location!", "Failed!");
            }

            return new SaveLocator(false, 0);
        }

        private int getTotalSaves(string saveGameDirectory, string[] steamIds)
        {
            int totalSteamIds = steamIds.Length;
            int totalSaves = 0;
            int directoriesLength;

            for (int currentSteamId = 0; currentSteamId < totalSteamIds; currentSteamId++)
            {
                var directories = Directory.GetDirectories(steamIds[currentSteamId]);
                directoriesLength = directories.Length;

                for (int currentSaveDirectory = 0; currentSaveDirectory < directoriesLength; currentSaveDirectory++)
                {
                    if (directories[currentSaveDirectory].Contains("Save_"))
                    {
                        totalSaves++;
                    }
                }
            }

            return totalSaves;
        }

        private void openTransfer(string location, int totalSaves)
        {
            this.Hide();
            Form2 options = new Form2(location, totalSaves);
            options.Show();
        }

        private void EditCharStart_Click(object sender, EventArgs e)
        {
            charsInfo = charGrid.Get_CheckBoxes();
            Base_Edit_Data();
        }

        private void Base_Edit_Data()
        {
            int activeChar = Get_Edit_Char_Index(charsInfo);

            if (activeChar > -1)
            {
                panelIndex = Get_Panel_Index("EditSpecificCharPanel");
                Redraw_Menu();

                List<SelectionClass> raceSelections = new List<SelectionClass>();
                raceSelections.Add(new SelectionClass("Auraian", ((int)races.Auraian).ToString()));
                raceSelections.Add(new SelectionClass("Tramon", ((int)races.Tramon).ToString()));
                raceSelections.Add(new SelectionClass("Kazite", ((int)races.Kazite).ToString()));

                this.RaceComboBox.DataSource = raceSelections;
                this.RaceComboBox.DisplayMember = "Name";
                this.RaceComboBox.ValueMember = "Value";

                this.RaceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                VisualData charVisualData = charsInfo[activeChar].Get_Character_Save().GetPSaveDataByRef().visualInfo;
                this.RaceComboBox.SelectedIndex = charVisualData.GetSkinIndex();

                List<SelectionClass> genderSelections = new List<SelectionClass>();
                genderSelections.Add(new SelectionClass("Male", ((int)OutwardSaveTransfer.VisualData.genderNames.Male).ToString()));
                genderSelections.Add(new SelectionClass("Female", ((int)OutwardSaveTransfer.VisualData.genderNames.Female).ToString()));

                this.GenderComboBox.DataSource = genderSelections;
                this.GenderComboBox.DisplayMember = "Name";
                this.GenderComboBox.ValueMember = "Value";
                this.GenderComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.GenderComboBox.SelectedIndex = charVisualData.GetGender();

                Set_Drop_Boxes((races)charVisualData.GetSkinIndex(), charVisualData);

                this.CharNameBox.Text = charsInfo[activeChar].Get_Character_Save().GetPSaveDataByRef().GetName();
                this.EditCharName.Text = this.CharNameBox.Text;
            }
            else
            {
                panelIndex = Get_Panel_Index("EditSaveLocationPanel");
                Redraw_Menu();
            }
        }

        private void Set_Drop_Boxes(races race, VisualData charVisualData)
        {
            List<SelectionClass> faceSelections = new List<SelectionClass>();
            List<SelectionClass> hairStyleSelection = new List<SelectionClass>();
            List<SelectionClass> hairColorSelection = new List<SelectionClass>();

            VisualData.genderNames gender = (VisualData.genderNames)charVisualData.GetGender();

            switch (race)
            {
                case races.Auraian:
                    {
                        switch (gender)
                        {
                            case VisualData.genderNames.Female:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Auraian]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Auraian]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Auraian]);
                                    break;
                                }
                            case VisualData.genderNames.Male:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Auraian]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Auraian]);
                                    break;
                                }
                            default:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                        }
                        break;
                    }
                case races.Kazite:
                    {
                        switch (gender)
                        {
                            case VisualData.genderNames.Female:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Kazite]);
                                    break;
                                }
                            case VisualData.genderNames.Male:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Kazite]);
                                    break;
                                }
                            default:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                        }
                        break;
                    }
                case races.Tramon:
                    {
                        switch (gender)
                        {
                            case VisualData.genderNames.Female:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Kazite]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Kazite]);
                                    break;
                                }
                            case VisualData.genderNames.Male:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Tramon]);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Tramon]);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Tramon]);
                                    break;
                                }
                            default:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        switch (gender)
                        {
                            case VisualData.genderNames.Female:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                            case VisualData.genderNames.Male:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                            default:
                                {
                                    Fill_List_With_Selections(ref faceSelections, 1, 6);
                                    Fill_List_With_Selections(ref hairStyleSelection, 1, 15);
                                    Fill_List_With_Selections(ref hairColorSelection, 1, 11);
                                    break;
                                }
                        }
                        break;
                    }
            }

            Set_Selection_ComboBox_Defaults(ref charVisualData, ref faceSelections, ref hairStyleSelection, ref hairColorSelection);
        }

        private void Set_Selection_ComboBox_Defaults(ref VisualData charVisualData, ref List<SelectionClass> faceSelections, ref List<SelectionClass>  hairStyleSelection, ref List<SelectionClass>  hairColorSelection)
        {
            int headIndex = charVisualData.GetHeadVariationIndex();
            int hairStyleIndex = charVisualData.GetHairStyleIndex();
            int hairColorIndex = charVisualData.GetHairColorIndex();

            this.FaceComboBox.DataSource = faceSelections;
            this.FaceComboBox.DisplayMember = "Name";
            this.FaceComboBox.ValueMember = "Value";
            this.FaceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.FaceComboBox.SelectedIndex = headIndex < (faceSelections != null ? faceSelections.Count : 0) ? headIndex : 0;

            this.HairStyleComboBox.DataSource = hairStyleSelection;
            this.HairStyleComboBox.DisplayMember = "Name";
            this.HairStyleComboBox.ValueMember = "Value";
            this.HairStyleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.HairStyleComboBox.SelectedIndex = hairStyleIndex < (hairStyleSelection != null ? hairStyleSelection.Count : 0) ? hairStyleIndex : 0;

            this.HairColorComboBox.DataSource = hairColorSelection;
            this.HairColorComboBox.DisplayMember = "Name";
            this.HairColorComboBox.ValueMember = "Value";
            this.HairColorComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.HairColorComboBox.SelectedIndex = hairColorIndex < (hairColorSelection != null ? hairColorSelection.Count : 0) ? hairColorIndex : 0;
        }

        private void Fill_List_With_Selections(ref List<SelectionClass> selection, int nameIndex, int endValueIndex, int startValueIndex = 0, int stepSize = 1)
        {
            for(int currentSelection = startValueIndex; currentSelection < endValueIndex; currentSelection += stepSize)
            {
                selection.Add(new SelectionClass(nameIndex.ToString(), currentSelection.ToString()));
                nameIndex += stepSize;
            }
        }

        private int Get_Edit_Char_Index(List<characterInfo> chars)
        {
            int charsLength = chars.Count;

            for(int currentChar = 0; currentChar < charsLength; currentChar++)
            {
                if(chars[currentChar].Get_Mark())
                {
                    return currentChar;
                }
            }

            return -1;
        }

        private void EditCharNext_Click(object sender, EventArgs e)
        {
            int activeChar = Get_Edit_Char_Index(charsInfo);

            if (activeChar > -1)
            {
                PSaveData saveData = charsInfo[activeChar].Get_Character_Save().GetPSaveDataByRef();

                saveData.SetName(this.CharNameBox.Text);
                saveData.visualInfo.SetGender(this.GenderComboBox.SelectedIndex);
                saveData.visualInfo.SetSkinIndex(this.RaceComboBox.SelectedIndex);
                saveData.visualInfo.SetHeadVariationIndex(this.FaceComboBox.SelectedIndex);
                saveData.visualInfo.SetHairStyleIndex(this.HairStyleComboBox.SelectedIndex);
                saveData.visualInfo.SetHairColorIndex(this.HairColorComboBox.SelectedIndex);

                charsInfo[activeChar].Set_Mark(false);

                Base_Edit_Data();
            }
            else
            {
                panelIndex = Get_Panel_Index("EditSaveLocationPanel");
                Redraw_Menu();
            }
        }

        private void StartEditingCharsButton_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(this.EditOutputTextBox.Text))
            {
                panelIndex = Get_Panel_Index("EditConsolePanel");
                Redraw_Menu();

                if (editConsole == null)
                {
                    editConsole = new ConsoleWindowClass(editConsoleWindow);
                }
            }
            else
            {
                MessageBox.Show("Failed to locate location!", "Failiure!");
            }
        }

        private void StartEditing_Click(object sender, EventArgs e)
        {
            string saveLocation = this.EditOutputTextBox.Text;
            string saveGameLocation = Path.Combine(saveLocation, "SaveGames");

            try
            {
                // Create the base SaveGames folder in case it doesnt exist
                Directory.CreateDirectory(saveGameLocation);

                if (!Directory.Exists(this.EditCharTextBox.Text))
                {
                    editConsole.Print_error_text("\nCould not locate games 'SaveGames' folder. Cannot recreate save files.");
                    return;
                }

                string saveSteamIdLocation, charSteamId, charFolder, copyFolder, newestSaveFolderName, fullNewestFolder;
                List<string>steamIdsSaved = new List<string>();
                bool foundSteamId = false;
                int steamIdsSavedTotal;

                foreach (characterInfo character in charsInfo)
                {
                    if (character.Needed_Changes())
                    {
                        //steam id folder creation
                        steamIdsSavedTotal = steamIdsSaved.Count;
                        charSteamId = character.Get_Steam_ID().ToString();

                        for (int currentSteamId = 0; currentSteamId < steamIdsSavedTotal; currentSteamId++)
                        {
                            if(steamIdsSaved[currentSteamId] == charSteamId)
                            {
                                foundSteamId = true;
                                break;
                            }
                        }

                        saveSteamIdLocation = Path.Combine(saveGameLocation, charSteamId);

                        if (!foundSteamId)
                        {

                            Directory.CreateDirectory(saveSteamIdLocation);
                            steamIdsSaved.Add(charSteamId);
                        }
                        else
                        {
                            foundSteamId = false;
                        }

                        //character folder creation
                        charFolder = Path.Combine(saveSteamIdLocation, character.Get_Encoded_Folder());

                        //character newest save folder creation
                        copyFolder = charGrid.Get_Path_With_Newest_Data(character.Get_Location());
                        newestSaveFolderName = Get_Last_Folder_From_Path(copyFolder);

                        fullNewestFolder = Path.Combine(charFolder, newestSaveFolderName);

                        if (fullNewestFolder != copyFolder)
                        {
                            Directory.CreateDirectory(charFolder);
                            Directory.CreateDirectory(fullNewestFolder);

                            Form2.CopyAll(new DirectoryInfo(copyFolder), new DirectoryInfo(fullNewestFolder));
                            File.SetAttributes(fullNewestFolder, FileAttributes.Normal);
                        }

                        Change_Char_Info(fullNewestFolder, character);
                    }
                }
            }
            catch (Exception ex)
            {
                editConsole.Print_text($"\n{ex}");
            }
        }

        private static void Change_Char_Info(string saveInstanceFolder, characterInfo character)
        {
            string fileWithExtension;

            if(character.Is_Defintive_Edition())
            {
                fileWithExtension = "Char0.defedc";
            }
            else
            {
                fileWithExtension = "Char0.charc";
            }

            string savePath = Path.Combine(saveInstanceFolder, fileWithExtension);
            if (!File.Exists(savePath))
            {
                editConsole.Print_error_text($"\nMissing {fileWithExtension} file in folder \"{saveInstanceFolder}\"!");
                return;
            }

            editConsole.Print_text($"\nChanging visual data of \"{character.Get_Character_Save().GetPSaveDataByRef().GetName()}\" also known as \"{character.Get_Old_Name()}\"!");
            XmlDocument xml;
            GZipStream gzipStream = new GZipStream(File.OpenRead(savePath), CompressionMode.Decompress);
            StreamReader streamReader = new StreamReader(gzipStream);

            xml = character.Get_Character_Save().ChangeVisualData(streamReader.ReadToEnd());

            streamReader.Close();
            gzipStream.Close();

            Form2.SaveXmlOutwardFile(savePath, xml, ref editConsole);
        }

        private string Get_Last_Folder_From_Path(string path)
        {
            int lastFolderStartIndex = path.LastIndexOf("\\") + 1;

            return path.Substring(lastFolderStartIndex, path.Length - lastFolderStartIndex);
        }

        private void Fix_Max_Selections()
        {
            var faceList = this.FaceComboBox.DataSource as List<SelectionClass>;
            var hairStyleList = this.HairStyleComboBox.DataSource as List<SelectionClass>;
            var hairColorList = this.HairColorComboBox.DataSource as List<SelectionClass>;

            int activeChar = Get_Edit_Char_Index(charsInfo);

            if (faceList != null)
            {
                int totalFaces = faceList.Count;
                int limitToFaces = Max_Of_PVisualData_List(PVisualIndependentData.Face);

                if (totalFaces < limitToFaces)
                {
                    Fill_List_With_Selections(ref faceList, totalFaces + 1, limitToFaces, totalFaces);
                }
                else if (totalFaces > limitToFaces)
                {
                    faceList.RemoveRange(limitToFaces, totalFaces - limitToFaces);
                }
            }
            else
            {
                return;
            }

            if (hairStyleList != null)
            {
                int totalHairStyles = hairStyleList.Count;
                int limitToHairStyles = Max_Of_PVisualData_List(PVisualIndependentData.HairStyle);

                if (totalHairStyles < limitToHairStyles)
                {
                    Fill_List_With_Selections(ref hairStyleList, totalHairStyles + 1, limitToHairStyles, totalHairStyles);
                }
                else if (totalHairStyles > limitToHairStyles)
                {
                    hairStyleList.RemoveRange(limitToHairStyles, totalHairStyles - limitToHairStyles);
                }
            }
            else
            {
                return;
            }

            if (hairColorList != null)
            {
                int totalHairColors = hairColorList.Count;
                int limitToHairColors = Max_Of_PVisualData_List(PVisualIndependentData.HairColor);

                if (totalHairColors < limitToHairColors)
                {
                    Fill_List_With_Selections(ref hairColorList, totalHairColors + 1, limitToHairColors, totalHairColors);
                }
                else if (totalHairColors > limitToHairColors)
                {
                    hairColorList.RemoveRange(limitToHairColors, totalHairColors - limitToHairColors);
                }
            }
            else
            {
                return;
            }

            this.FaceComboBox.DataSource = null;
            this.HairStyleComboBox.DataSource = null;
            this.HairColorComboBox.DataSource = null;

            Set_Selection_ComboBox_Defaults(ref charsInfo[activeChar].Get_Character_Save().GetPSaveDataByRef().visualInfo, ref faceList, ref hairStyleList, ref hairColorList);
        }

        private void GenderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fix_Max_Selections();
        }

        private void RaceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fix_Max_Selections();
        }

        private int Max_Of_PVisualData_List(PVisualIndependentData data)
        {
            switch ((races)this.RaceComboBox.SelectedIndex)
            {
                case races.Auraian:
                    {
                        switch ((VisualData.genderNames)this.GenderComboBox.SelectedIndex)
                        {
                            case VisualData.genderNames.Male:
                                {
                                    switch(data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                            case VisualData.genderNames.Female:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                            default:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                        }
                    }
                case races.Kazite:
                    {
                        switch ((VisualData.genderNames)this.GenderComboBox.SelectedIndex)
                        {
                            case VisualData.genderNames.Male:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Kazite];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Kazite];
                                            }
                                    }
                                }
                            case VisualData.genderNames.Female:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Kazite];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Kazite];
                                            }
                                    }
                                }
                            default:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Kazite];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Kazite];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                        }
                    }
                case races.Tramon:
                    {
                        switch ((VisualData.genderNames)this.GenderComboBox.SelectedIndex)
                        {
                            case VisualData.genderNames.Male:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Tramon];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                    }
                                }
                            case VisualData.genderNames.Female:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Tramon];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                    }
                                }
                            default:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Tramon];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Tramon];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Tramon];
                                            }
                                    }
                                }
                        }
                    }
                default:
                    {
                        switch ((VisualData.genderNames)this.GenderComboBox.SelectedIndex)
                        {
                            case VisualData.genderNames.Male:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                            case VisualData.genderNames.Female:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.femaleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                            default:
                                {
                                    switch (data)
                                    {
                                        case PVisualIndependentData.Face:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairStyle:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairStyleSelectionsLimits[(int)races.Auraian];
                                            }
                                        case PVisualIndependentData.HairColor:
                                            {
                                                return RacePSaveDataChoices.Instance.maleHairColorSelectionsLimits[(int)races.Auraian];
                                            }
                                        default:
                                            {
                                                return RacePSaveDataChoices.Instance.maleFaceSelectionsLimits[(int)races.Auraian];
                                            }
                                    }
                                }
                        }
                    }
            }
        }


    }
}

