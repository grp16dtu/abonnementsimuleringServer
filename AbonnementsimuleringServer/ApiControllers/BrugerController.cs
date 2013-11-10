using AbonnementsimuleringServer.Autorisation;
using AbonnementsimuleringServer.Helpers;
using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AbonnementsimuleringServer.ApiControllers
{
    [BasicAuth]
    public class BrugerController : ApiController
    {
        [HttpGet]
        [ActionName("Hent")]
        public HttpResponseMessage Hent(string brugernavn, string kodeord)
        {
            try
            {
                MySQL mySql = new MySQL();
                DataSet brugerdata = mySql.HentBruger(brugernavn, kodeord);

                if(brugerdata == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ingen bruger fundet");

                Bruger bruger = new Bruger(brugerdata);
                return Request.CreateResponse(HttpStatusCode.OK, bruger);     
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }

        [HttpGet]
        [ActionName("HentAlle")]
        [BasicAuth]
        public HttpResponseMessage HentAlle()
        {
            List<Bruger> brugere = new List<Bruger>();
            try
            {
                MySQL mySql = new MySQL();
                DataSet brugerdata = mySql.HentAlleBrugere();

                brugere = Bruger.ListeAfBrugere(brugerdata);
                return Request.CreateResponse(HttpStatusCode.OK, brugere);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }



        [HttpPost]
        [ActionName("Opret")]
        [BasicAuth]
        public HttpResponseMessage Opret([FromBody]Bruger bruger)
        {
            if (bruger == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data forkert");

            int economicAftalenummer = ((ApiIdentitet)(HttpContext.Current.User.Identity)).EconomicAftalenummer;

            try
            {
                MySQL mySql = new MySQL();

                if (mySql.BrugerEksisterer(bruger.Brugernavn))
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Bruger allerede oprettet");

                else 
                {
                    mySql.OpretBruger(bruger, economicAftalenummer);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "Bruger oprettet");
                }
            }

            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
            
            
        }

        [HttpPut]
        [ActionName("Rediger")]
        [BasicAuth]
        public HttpResponseMessage Rediger([FromBody]Bruger bruger)
        {
            if (bruger == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");

            try
            {
                MySQL mySql = new MySQL();
                mySql.RedigerBruger(bruger);
                return Request.CreateResponse(HttpStatusCode.OK,"Bruger redigeret");
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }


        }

        [HttpDelete]
        [ActionName("Slet")]
        [BasicAuth]
        public HttpResponseMessage Slet([FromUri]Bruger bruger)
        {
            if (bruger == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");

            try
            {
                MySQL mySql = new MySQL();
                mySql.SletBruger(bruger);
                return Request.CreateResponse(HttpStatusCode.OK,"Bruger slettet");
            }
            
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}
