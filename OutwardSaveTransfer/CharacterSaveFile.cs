using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OutwardSaveTransfer
{
    class CharacterSaveFile
    {
        public List<BasicSaveData> itemLists;
        public List<BasicSaveData> stashedItemLists;
        private PSaveData saveData;

        public CharacterSaveFile()
        {
            itemLists = new List<BasicSaveData>();
            stashedItemLists = new List<BasicSaveData>();
        }

        public CharacterSaveFile(string serializeString)
        {
            itemLists = new List<BasicSaveData>();
            stashedItemLists = new List<BasicSaveData>();
        }

        public XmlDocument Desirelize(string serializeString)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CharacterSaveFile));
            XmlDocument xml =  new XmlDocument();
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

            xml.LoadXml(serializeString);

            XmlNodeList psaveNodes = xml.SelectNodes("//Character/PSave");

            foreach(XmlNode psave in psaveNodes)
            {
                XmlNode visualData = psave.SelectSingleNode("VisualData", namespaces);


                VisualData.genderNames genderIndex;
                if(!Enum.TryParse<VisualData.genderNames>(visualData.SelectSingleNode("Gender", namespaces).InnerText, out genderIndex))
                {
                    genderIndex = VisualData.genderNames.Male;
                }

                saveData = new PSaveData(new VisualData(((int)genderIndex), Int16.Parse(visualData.SelectSingleNode("HairStyleIndex", namespaces).InnerText), Int16.Parse(visualData.SelectSingleNode("HairColorIndex", namespaces).InnerText),
                    Int16.Parse(visualData.SelectSingleNode("SkinIndex", namespaces).InnerText), Int16.Parse(visualData.SelectSingleNode("HeadVariationIndex", namespaces).InnerText)));

                saveData.SetNewSave(bool.Parse(psave.SelectSingleNode("NewSave", namespaces).InnerText));
                saveData.SetUID(psave.SelectSingleNode("UID", namespaces).InnerText);
                saveData.SetName(psave.SelectSingleNode("Name", namespaces).InnerText);

                if (psaveNodes.Count > 1)
                {
                    //corrupted file 2 or more psavedatas
                    break;
                }
            }

            XmlNodeList itemNodes = xml.SelectNodes("//Character/ItemList/BasicSaveData");

            foreach(XmlNode item in itemNodes)
            {
                itemLists.Add(new BasicSaveData(item.SelectSingleNode("Identifier", namespaces).InnerText, item.SelectSingleNode("SyncData", namespaces).InnerText));
            }

            return xml;
            //using (TextReader reader = new StringReader(serializeString))
            //{
            //    serializer.Deserialize(reader);
            //}
        }

        public XmlDocument ChangeVisualData(string serializeString)
        {
            XmlDocument xml = new XmlDocument();
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

            xml.LoadXml(serializeString);

            XmlNodeList pSaveNodes = xml.SelectNodes("//Character/PSave");

            if (pSaveNodes.Count == 1)
            {
                foreach (XmlNode psaveData in pSaveNodes)
                {
                    psaveData.SelectSingleNode("Name", namespaces).InnerText = saveData.GetName();
                }
            }

            XmlNodeList visualDataNodes = xml.SelectNodes("//Character/PSave/VisualData");

            if (visualDataNodes.Count == 1)
            {
                foreach (XmlNode visualData in visualDataNodes)
                {
                    visualData.SelectSingleNode("SkinIndex", namespaces).InnerText = saveData.visualInfo.GetSkinIndex().ToString();
                    visualData.SelectSingleNode("Gender", namespaces).InnerText = ((VisualData.genderNames)saveData.visualInfo.GetGender()).ToString();
                    visualData.SelectSingleNode("HeadVariationIndex", namespaces).InnerText = saveData.visualInfo.GetHeadVariationIndex().ToString();
                    visualData.SelectSingleNode("HairStyleIndex", namespaces).InnerText = saveData.visualInfo.GetHairStyleIndex().ToString();
                    visualData.SelectSingleNode("HairColorIndex", namespaces).InnerText = saveData.visualInfo.GetHairColorIndex().ToString();
                }
            }

            return xml;
        }

        public XmlDocument FillStashesXmlDocument(XmlDocument xml)
        {
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");
            //XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

            XmlNodeList psaveNodes = xml.SelectNodes("//Character/PSave");

            if (psaveNodes.Count > 1 || psaveNodes.Count == 0)
            {
                //corrupted file 2 or more psavedatas
                return xml;
            }

            foreach (XmlNode psave in psaveNodes)
            {
                XmlElement stashedMoney = xml.CreateElement("StashedMoney");
                stashedMoney.InnerText = this.saveData.GetStashedMoney().ToString();
                psave.AppendChild(stashedMoney);
            }

            XmlNodeList stashedItemNodes = xml.SelectNodes("//Character/StashedItemList");
            XmlNode rootChar = xml.SelectSingleNode("Character", namespaces);

            if (stashedItemNodes.Count == 0)
            {
                XmlElement stashedItem = xml.CreateElement("StashedItemList");
                rootChar.AppendChild(stashedItem);

                stashedItemNodes = xml.SelectNodes("//Character/StashedItemList");
            }

            foreach (BasicSaveData item in stashedItemLists)
            {
                XmlElement basicSaveData = xml.CreateElement("BasicSaveData");

                XmlElement identifier = xml.CreateElement("Identifier");
                identifier.InnerText = item.GetIdentifier();
                identifier.SetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", "xsd:string");

                XmlElement syncData = xml.CreateElement("SyncData");
                syncData.InnerText = item.GetSyncData();

                basicSaveData.AppendChild(identifier);
                basicSaveData.AppendChild(syncData);

                stashedItemNodes[0].AppendChild(basicSaveData);
            }

            return xml;
        }

        public void DesirelizeWithStashFix(string serializeString)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(CharacterSaveFile));
            XmlDocument xml = new XmlDocument();
            XmlNamespaceManager namespaces = new XmlNamespaceManager(xml.NameTable);
            namespaces.AddNamespace("msbld", "http://schemas.microsoft.com/developer/msbuild/2003");

            xml.LoadXml(serializeString);

            XmlNodeList psaveNodes = xml.SelectNodes("//Character/PSave");

            foreach (XmlNode psave in psaveNodes)
            {
                XmlNode visualData = psave.SelectSingleNode("VisualData", namespaces);


                VisualData.genderNames genderIndex;
                if (!Enum.TryParse<VisualData.genderNames>(visualData.SelectSingleNode("Gender", namespaces).InnerText, out genderIndex))
                {
                    genderIndex = VisualData.genderNames.Male;
                }


                saveData = new PSaveData(new VisualData(((int)genderIndex), Int16.Parse(visualData.SelectSingleNode("HairStyleIndex", namespaces).InnerText), Int16.Parse(visualData.SelectSingleNode("HairColorIndex", namespaces).InnerText),
                    Int16.Parse(visualData.SelectSingleNode("SkinIndex", namespaces).InnerText), Int16.Parse(visualData.SelectSingleNode("HeadVariationIndex", namespaces).InnerText)));

                saveData.SetNewSave(bool.Parse(psave.SelectSingleNode("NewSave", namespaces).InnerText));
                saveData.SetUID(psave.SelectSingleNode("UID", namespaces).InnerText);
                saveData.SetName(psave.SelectSingleNode("Name", namespaces).InnerText);

                if (psaveNodes.Count > 1)
                {
                    //corrupted file 2 or more psavedatas
                    break;
                }
            }

            XmlNodeList itemNodes = xml.SelectNodes("//Character/ItemList/BasicSaveData");
            //string newHierarchy = "1Stash_" + saveData.getUID() + ";40";

            foreach (XmlNode item in itemNodes)
            {
                //if (item.SelectSingleNode("SyncData", namespaces)["Hierarchy"].InnerText == "")
                itemLists.Add(new BasicSaveData(item.SelectSingleNode("Identifier", namespaces).InnerText, item.SelectSingleNode("SyncData", namespaces).InnerText));
            }

            XmlNodeList stashedItemNodes = xml.SelectNodes("//Character/StashedItemList/BasicSaveData");
            //string newHierarchy = "1Stash_" + saveData.getUID() + ";40";

            foreach (XmlNode item in stashedItemNodes)
            {
                //if (item.SelectSingleNode("SyncData", namespaces)["Hierarchy"].InnerText == "")
                stashedItemLists.Add(new BasicSaveData(item.SelectSingleNode("Identifier", namespaces).InnerText, item.SelectSingleNode("SyncData", namespaces).InnerText));
            }

            //using (TextReader reader = new StringReader(serializeString))
            //{
            //    serializer.Deserialize(reader);
            //}
        }

        public PSaveData GetPSaveData()
        {
            return saveData;
        }

        public ref PSaveData GetPSaveDataByRef()
        {
            return ref saveData;
        }

        public void FixStashedItems()
        {
            foreach(BasicSaveData item in itemLists)
            {
                if(item.GetSyncData().Contains(""))
                {

                }
            }
        }

    }
}
