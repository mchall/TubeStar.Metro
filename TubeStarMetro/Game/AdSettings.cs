using System;
using System.Collections.Generic;

namespace TubeStarMetro
{
    public static class AdSettings
    {
        public static string PubCenterApplicationId
        {
            get
            {
#if DEBUG
                return "d25517cb-12d4-4699-8bdc-52040c712cab";
#endif
                return "d8876e83-6eb2-419a-a2f6-5731fd24e4fe";
            }
        }

        public static string Ad_728x90
        {
            get
            {

#if DEBUG
                return "10042998";
#endif

                return "166359";
            }
        }
    }
}