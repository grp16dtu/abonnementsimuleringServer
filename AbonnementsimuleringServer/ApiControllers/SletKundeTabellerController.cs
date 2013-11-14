using AbonnementsimuleringServer.Autorisation;
using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    [BasicAuth]
    public class SletKundeTabellerController : ApiController
    {
        public HttpResponseMessage Get() 
        {
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

            int economicAftalenummer = ((ApiIdentitet)HttpContext.Current.User.Identity).EconomicAftalenummer;
            try
            {
                MySQL mySql = new MySQL(economicAftalenummer);
                mySql.SletKundeTabeller();
                return Request.CreateResponse(HttpStatusCode.OK, "Tabeller slettet");
            }

            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }
        }
    }
}
