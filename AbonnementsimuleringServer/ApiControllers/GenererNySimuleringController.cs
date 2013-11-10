using AbonnementsimuleringServer.Autorisation;
using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    [BasicAuth]
    public class GenererNySimuleringController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
                int economicAftalenummer = identitet.EconomicAftalenummer;
                string economicBrugernavn = identitet.Name;
                string economicKodeord = identitet.Password;
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
