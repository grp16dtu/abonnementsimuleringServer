using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    public class GenererNySimuleringController : ApiController
    {
        // GET api/generernysimulering
        public HttpResponseMessage Get(int id)
        {
            try
            {
                EconomicController economic = new EconomicController(387892, "DTU", "Trustno1");
                List<Transaktion> transaktioner = economic.GenererNySimulering(12, 1);
                return Request.CreateResponse(HttpStatusCode.OK, transaktioner);
            }

            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }

             
        }
    }
}
