using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardSaveTransfer
{
    class VisualData
    {
        private int gender;
        private int hairStyleIndex;
        private int hairColorIndex;
        private int skinIndex;
        private int headVariationIndex;

        //private static string[] genderNames =
        //{
        //    "Male",
        //    "Female"
        //};

        public enum genderNames
        {
            Male,
            Female
        }

        public VisualData(int setGender, int setHairStyleIndex, int setHairColorIndex, int setSkinIndex, int setHeadVariationIndex)
        {
            this.gender = setGender;
            this.hairStyleIndex = setHairStyleIndex;
            this.hairColorIndex = setHairColorIndex;
            this.skinIndex = setSkinIndex;
            this.headVariationIndex = setHeadVariationIndex;
        }

        public void SetGender(int newGender)
        {
            gender = newGender;
        }

        public int GetGender()
        {
            return gender;
        }

        public void SetHairStyleIndex(int newIndex)
        {
            hairStyleIndex = newIndex;
        }

        public int GetHairStyleIndex()
        {
            return hairStyleIndex;
        }

        public void SetHairColorIndex(int newIndex)
        {
            hairColorIndex = newIndex;
        }

        public int GetHairColorIndex()
        {
            return hairColorIndex;
        }

        public int GetSkinIndex()
        {
            return skinIndex;
        }

        public void SetSkinIndex(int newIndex)
        {
            skinIndex = newIndex;
        }

        public int GetHeadVariationIndex()
        {
            return headVariationIndex;
        }

        public void SetHeadVariationIndex(int newIndex)
        {
            headVariationIndex = newIndex;
        }
    }
}
