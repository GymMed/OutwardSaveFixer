using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutwardSaveTransfer;

namespace OutwardSaveFixer
{
    //Singelton class without thread safety
    public sealed class RacePSaveDataChoices
    {
        private RacePSaveDataChoices()
        {

        }

        private static RacePSaveDataChoices instance = null;

        public static RacePSaveDataChoices Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new RacePSaveDataChoices();
                }
                return instance;
            }
        }

        //Auraian, Tramon, Kazite
        public readonly int[] maleFaceSelectionsLimits = new int[] { 8, 6, 7 };
        public readonly int[] femaleFaceSelectionsLimits = new int[] { 6, 6, 6 };

        public readonly int[] maleHairStyleSelectionsLimits = new int[] { 15, 15, 15 };
        public readonly int[] femaleHairStyleSelectionsLimits = new int[] { 15, 15, 15 };

        public readonly int[] maleHairColorSelectionsLimits = new int[] { 11, 11, 11 };
        public readonly int[] femaleHairColorSelectionsLimits = new int[] { 11, 11, 11 };
    }
}
