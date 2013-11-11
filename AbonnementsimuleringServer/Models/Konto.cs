using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class Konto
    {
        public int EconomicAftalenummer { get; set; }
        public string EconomicBrugernavn { get; set; }
        public string EconomicKodeord { get; set; }
        public Bruger AbosimBruger { get; set; }
        
        
        public Konto()
        {
        }
    }  
}