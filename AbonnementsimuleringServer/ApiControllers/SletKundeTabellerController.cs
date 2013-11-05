using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    public class SletKundeTabellerController : ApiController
    {
        public string Get(int id) 
        {
            int economicAftalenummer = id;
            try
            {
                MySQL mySql = new MySQL(economicAftalenummer);
                mySql.SletKundeTabeller();
                return "Tabeller slettet";
            }
            catch (Exception e)
            {
                return "Fejl: " + e.Message;
            }
        }
    }
}
