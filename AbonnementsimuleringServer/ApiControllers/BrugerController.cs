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
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

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
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

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
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

            if (bruger == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data forkert");

            int economicAftalenummer = identitet.EconomicAftalenummer;

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
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

            if (bruger == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");

            try
            {
                MySQL mySql = new MySQL();
                int brugerAftalenummer = mySql.HentEconomicAftalenummer(bruger.Brugernavn);
                int ansvarligAftalenummer = identitet.EconomicAftalenummer;

                if (brugerAftalenummer != ansvarligAftalenummer)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");

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
        public HttpResponseMessage Slet(string id)
        {
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

            if (id == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");

            try
            {
                MySQL mySql = new MySQL();
                int brugerAftalenummer = mySql.HentEconomicAftalenummer(id);
                int ansvarligAftalenummer = identitet.EconomicAftalenummer;

                if (brugerAftalenummer != ansvarligAftalenummer)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bruger data forkert");
                
                mySql.SletBruger(id);
                return Request.CreateResponse(HttpStatusCode.OK,"Bruger slettet");
            }
            
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }
    }
}
