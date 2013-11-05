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
        public HttpResponseMessage Get(int id) 
        {
            int economicAftalenummer = id;
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
