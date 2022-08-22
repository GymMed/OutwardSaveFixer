using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardSaveTransfer
{
    class PSaveData
    {
        private bool newSave;
        private string UID;
        private string name;

        public VisualData visualInfo;

        private Int64 stashedMoney;

        public PSaveData(VisualData vd)
        {
            visualInfo = vd;
        }

        public void SetNewSave(bool setSave)
        {
            newSave = setSave;
        }

        public bool GetNewSave()
        {
            return newSave;
        }


        public void SetUID(string newUID)
        {
            UID = newUID;
        }

        public string GetUID()
        {
            return UID;
        }

        public void SetStashedMoney(Int64 newMoney)
        {
            stashedMoney = newMoney;
        }

        public Int64 GetStashedMoney()
        {
            return stashedMoney;
        }

        public void SetName(string newName)
        {
            name = newName;
        }

        public string GetName()
        {
            return name;
        }
    }
}
