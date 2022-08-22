using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardSaveTransfer
{
    class BasicSaveData
    {
        private string Identifier;
        private string SyncData;

        public BasicSaveData()
        {

        }

        public BasicSaveData(string id, string syncInfo)
        {
            this.Identifier = id;
            this.SyncData = syncInfo;
        }

        public string GetIdentifier()
        {
            return Identifier;
        }

        public void SetIdentifier(string newId)
        {
            Identifier = newId;
        }

        public string GetSyncData()
        {
            return SyncData;
        }

        public void SetSyncData(string newSyncData)
        {
            SyncData = newSyncData;
        }
    }
}
