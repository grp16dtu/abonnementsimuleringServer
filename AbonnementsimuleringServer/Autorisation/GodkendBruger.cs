using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiContrib.MessageHandlers;

namespace AbonnementsimuleringServer.Autorisation
{
    public class MyBasicAuthenticationHandler : BasicAuthenticationHandler
    {
        protected override bool Authorize(string username, string password)
        {
            if (username == "myusername" && password == "mypassword") return true;  
                return false;
        }

        protected override string Realm
        {
            get { return "Abonnement simulering"; }
        }
    }
}