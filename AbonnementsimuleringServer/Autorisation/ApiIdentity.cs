using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Providers.Entities;

namespace AbonnementsimuleringServer.Autorisation
{
    public class ApiIdentitet : IIdentity
    {
        public Bruger Bruger { get; private set; }
        public int EconomicAftalenummer { get; set; }
        public string EconomicBrugernavn { get; set; }
        public string EconomicKodeord { get; set; }

        public ApiIdentitet(Konto konto)
        {
            if (konto == null)
                throw new ArgumentNullException("user");

            Bruger = konto.AbosimBruger;
            EconomicAftalenummer = konto.EconomicAftalenummer;
            EconomicBrugernavn = konto.EconomicBrugernavn;
            EconomicKodeord = konto.EconomicKodeord;
        }

        public string Name
        {
            get { return Bruger.Brugernavn; }
        }

        public string Password
        {
            get { return Bruger.Kodeord; }
        }

  

        public string AuthenticationType
        {
            get { return "Basic"; }
        }

        public bool IsAuthenticated
        {
            get { return Bruger.Ansvarlig; }
        }
    }

}