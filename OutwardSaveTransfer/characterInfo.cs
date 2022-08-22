using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutwardSaveTransfer;

namespace OutwardSaveFixer
{
    class characterInfo
    {
        string location, oldName;
        CharacterSaveFile charSaveFile;
        bool isChecked, neededEdit, isDefinitiveEdition;

        public characterInfo(string location, CharacterSaveFile charSaveFile, bool isDefinitiveEdition = false, bool isChecked = false)
        {
            this.location = location;
            this.charSaveFile = charSaveFile;
            this.isChecked = isChecked;
            this.isDefinitiveEdition = isDefinitiveEdition;
            this.neededEdit = false;
            this.oldName = charSaveFile.GetPSaveDataByRef().GetName();
        }

        public Int64 Get_Steam_ID()
        {
            string[] folders = location.Split('\\');
            Int64 steamID;
            Int64.TryParse(folders[folders.Length - 2], out steamID);

            return steamID;
        }

        public string Get_Encoded_Folder()
        {
            //remove "\\"(counts as 1 char) from string
            int lastFolderStartIndex = location.LastIndexOf("\\") + 1;

            return location.Substring(lastFolderStartIndex, location.Length - lastFolderStartIndex);
        }

        public string Get_Location()
        {
            return this.location;
        }

        public bool Get_Mark()
        {
            return isChecked;
        }

        public void Set_Mark(bool mark)
        {
            this.isChecked = mark;

            if (!mark)
            {
                neededEdit = true;
            }
        }

        public bool Needed_Changes()
        {
            return neededEdit;
        }

        public string Get_Old_Name()
        {
            return oldName;
        }

        public bool Is_Defintive_Edition()
        {
            return isDefinitiveEdition;
        }

        public CharacterSaveFile Get_Character_Save()
        {
            return this.charSaveFile;
        }
    }
}
