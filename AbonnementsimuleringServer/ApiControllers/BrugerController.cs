using AbonnementsimuleringServer.Autorisation;
using AbonnementsimuleringServer.Helpers;
using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AbonnementsimuleringServer.ApiControllers
{
    [BasicAuthentication]
    public class BrugerController : ApiController
    {
        [HttpGet]
        [ActionName("Hent")]
        public HttpResponseMessage Hent(string brugernavn, string kodeord)
        {

            if (brugernavn == "Anders" && kodeord =="Kode1234")
            {
                Bruger bruger = new Bruger();
                bruger.Fornavn = "Lasse";
                return Request.CreateResponse(HttpStatusCode.OK, bruger); 
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bruger ikke fundet");
        }

        [HttpGet]
        [ActionName("Opret")]
        public HttpResponseMessage Opret([FromUri]Bruger bruger)
        {
            int economicAftalenummer = Vaerktoejer.FindEconomicAftalenummer(HttpContext.Current.User.Identity.Name);

            MySQL mySql = new MySQL();
            mySql.OpretBruger(bruger, economicAftalenummer);
            
            return Request.CreateErrorResponse(HttpStatusCode.OK, "Oprettet");
        }

        [HttpGet]
        [ActionName("Rediger")]
        public HttpResponseMessage Rediger(Bruger bruger)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Rediger");
        }

        [HttpGet]
        [ActionName("Slet")]
        public HttpResponseMessage Slet(Bruger bruger)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "slet");
        }


    }
}
