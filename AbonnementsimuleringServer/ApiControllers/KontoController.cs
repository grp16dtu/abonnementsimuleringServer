using AbonnementsimuleringServer.Helpers;
using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AbonnementsimuleringServer.ApiControllers
{
    public class KontoController : ApiController
    {
        [HttpPost]
        [ActionName("Opret")]
        public HttpResponseMessage Opret([FromBody]Konto abosimKonto)
        {
            if (abosimKonto == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data mangler eller ukorrekt formateret");

            try
            {
                MySQL mySql = new MySQL();
                if (mySql.AftalenummerEksisterer(abosimKonto.EconomicAftalenummer))
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Aftalenummer allerede i brug");

                if(mySql.BrugerEksisterer(abosimKonto.AbosimBruger.Brugernavn))
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Brugernavn allerede i brug");

                mySql.OpretAftalenummer(abosimKonto.EconomicAftalenummer, abosimKonto.EconomicBrugernavn, abosimKonto.EconomicKodeord);
                mySql.OpretBruger(abosimKonto.AbosimBruger, abosimKonto.EconomicAftalenummer);
                return Request.CreateResponse(HttpStatusCode.OK,"Aftalenummer og bruger oprettet");
            }

            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}
