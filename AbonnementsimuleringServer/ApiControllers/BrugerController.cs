using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.ApiControllers
{
    public class BrugerController : ApiController
    {
        [HttpGet]
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
    }
}
