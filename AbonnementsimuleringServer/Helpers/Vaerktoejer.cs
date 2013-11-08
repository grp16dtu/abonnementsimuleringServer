using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Helpers
{
    public class Vaerktoejer
    {
        public static int FindEconomicAftalenummer(string aftaleOgBrugernavn)
        {
            string[] split = aftaleOgBrugernavn.Split(':');
            return Convert.ToInt32(split[0]);
        }
    }
}