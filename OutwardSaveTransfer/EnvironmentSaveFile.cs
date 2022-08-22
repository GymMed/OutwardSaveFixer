using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Net;

namespace OutwardSaveTransfer
{
    class EnvironmentSaveFile
    {
        public List<BasicSaveData> itemLists;
        private string areaName;
        private int saveCreationGameTime;
        private double gameTime;

        public EnvironmentSaveFile()
        {
            itemLists = new List<BasicSaveData>();
        }

        public void Desirelize(string serializeString)
        {
            XmlDocument xml = new XmlDocument();
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

            xml.LoadXml(serializeString);

            XmlNodeList enviromentNodes = xml.SelectNodes("//Environment");


            if (enviromentNodes.Count > 1 || enviromentNodes.Count == 0)
            {
                //error bad envc
                return;
            }

            gameTime = double.Parse(enviromentNodes[0].SelectSingleNode("GameTime", namespaces).InnerText);
            saveCreationGameTime = Int16.Parse(enviromentNodes[0].SelectSingleNode("SaveCreationGameTime", namespaces).InnerText);

            XmlNodeList itemNodes = xml.SelectNodes("//Environment/ItemList");

            foreach(XmlNode item in itemNodes)
            {
                itemLists.Add(new BasicSaveData(item.SelectSingleNode("Identifier", namespaces).InnerText, item.SelectSingleNode("SyncData", namespaces).InnerText));
            }
        }

        public XmlDocument FixStashDesirelize(string serializeString, ref CharacterSaveFile charSave, string chestUID)
        {
            //gets data by reference be careful cuz changes impact main CharacterSaveFile PSaveData class
            PSaveData saveData = charSave.GetPSaveDataByRef();
            XmlDocument xml = new XmlDocument();
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

            xml.LoadXml(serializeString);

            XmlNodeList enviromentNodes = xml.SelectNodes("//Environment");


            if (enviromentNodes.Count > 1 || enviromentNodes.Count == 0)
            {
                //error bad envc
                return xml;
            }

            gameTime = double.Parse(enviromentNodes[0].SelectSingleNode("GameTime", namespaces).InnerText);
            saveCreationGameTime = Int16.Parse(enviromentNodes[0].SelectSingleNode("SaveCreationGameTime", namespaces).InnerText);
            string chestCoinsStr;//change to int later
            string fullIdentifierData, fullSyncData;

            XmlNodeList itemNodes = xml.SelectNodes("//Environment/ItemList/BasicSaveData");

            foreach (XmlNode item in itemNodes)
            {
                fullIdentifierData = item.SelectSingleNode("Identifier", namespaces).InnerText;
                fullSyncData = item.SelectSingleNode("SyncData", namespaces).InnerText;

                if (fullIdentifierData == chestUID)
                {
                    int startIndex = fullSyncData.IndexOf("<SubClassesData>");
                    string startSCD = fullSyncData.Substring(startIndex + 16);
                    chestCoinsStr = startSCD.Substring(0, startSCD.IndexOf("</SubClassesData>"));
                    saveData.SetStashedMoney(saveData.GetStashedMoney() + GetChestMoney(ref chestCoinsStr));
                }
                else
                {
                    if (fullSyncData.Contains("Hierarchy") && checkSyncDataHierarchy(fullSyncData, chestUID))
                    {
                        FixHierarchy(ref fullSyncData, ref saveData);
                        charSave.stashedItemLists.Add(new BasicSaveData(fullIdentifierData, fullSyncData));
                        item.ParentNode.RemoveChild(item);
                    }
                    else
                    {
                        itemLists.Add(new BasicSaveData(fullIdentifierData, fullSyncData));
                    }
                }
            }

            return xml;
        }

        private Int64 GetChestMoney(ref string chestCoinsStr)
        {
            //29 cuz "TreasureChestContainedSilver/" string length when follow ups coins and ends with ";"
            int startIndex = chestCoinsStr.IndexOf("TreasureChestContainedSilver") + 29;
            int endIndex = chestCoinsStr.IndexOf(";");

            return Int64.Parse(chestCoinsStr.Substring(startIndex, endIndex - startIndex));
        }

        private void FixHierarchy(ref string fullString, ref PSaveData saveData)
        {
            int startIndex = fullString.IndexOf("<Hierarchy>") + 11;
            string startHierarchy = fullString.Substring(startIndex);

            int endIndex = startHierarchy.IndexOf("</Hierarchy>");
            //string hierarchy = startHierarchy.Substring(0, endIndex);

            fullString = fullString.Remove(startIndex, endIndex).Insert(startIndex, "1Stash_" + saveData.GetUID() + ";40");
        }

        private bool checkSyncDataHierarchy(string fullString, string chestUID)
        {
            int startIndex = fullString.IndexOf("<Hierarchy>");
            string startHierarchy = fullString.Substring(startIndex + 11);
            string hierarchy = startHierarchy.Substring(0, startHierarchy.IndexOf("</Hierarchy>"));

            if (hierarchy.Contains(chestUID))
            {
                return true;
            }
            return false;
        }

        public string GetAreaName()
        {
            return areaName;
        }

        public void SetAreaName(string newAreaName)
        {
            areaName = newAreaName;
        }

        public int GetSaveCreationGameTime()
        {
            return saveCreationGameTime;
        }

        public void SetSaveCreationGameTime(int creationTime)
        {
            saveCreationGameTime = creationTime;
        }

        public double GetGameTime()
        {
            return gameTime;
        }

        public void SetGameTime(double newGameTime)
        {
            gameTime = newGameTime;
        }
    }
}
