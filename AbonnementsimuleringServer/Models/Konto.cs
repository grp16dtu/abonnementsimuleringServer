using System;
using System.Collections.Generic;
using System.Data;
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

        public Konto(DataSet dataSet, Bruger bruger)
        {
            AbosimBruger = bruger;

            EconomicAftalenummer = (int)dataSet.Tables["MySqlData"].Rows[0]["economicAftalenummer"];
            EconomicBrugernavn = dataSet.Tables["MySqlData"].Rows[0]["economicBrugernavn"].ToString();
            EconomicKodeord = dataSet.Tables["MySqlData"].Rows[0]["economicKodeord"].ToString();
        }
    }  
}