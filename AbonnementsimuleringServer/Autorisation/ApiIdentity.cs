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

        public ApiIdentitet(Bruger bruger, int economicAftalenummer)
        {
            if (bruger == null)
                throw new ArgumentNullException("user");

            this.Bruger = bruger;
            EconomicAftalenummer = economicAftalenummer;
        }

        public string Name
        {
            get { return this.Bruger.Brugernavn; }
        }

        public string Password
        {
            get { return this.Bruger.Kodeord; }
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