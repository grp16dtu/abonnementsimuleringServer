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
    
    public class SimuleringController : ApiController
    {
        readonly int ANTAL_MAANEDER_AT_SIMULERE_OVER = 12;

        [HttpGet]
        [ActionName("Ny")]
        public HttpResponseMessage Ny(int id)
        {
            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            if (!identitet.Bruger.Ansvarlig)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Du har ikke rettigheder til dette");

            int brugerindex = id;
            try
            {
                EconomicController economic = new EconomicController(identitet.EconomicAftalenummer, identitet.EconomicBrugernavn, identitet.EconomicKodeord);
                bool success = economic.GenererNySimulering(ANTAL_MAANEDER_AT_SIMULERE_OVER, brugerindex);

                if (success)
                    return Request.CreateResponse(HttpStatusCode.OK, "Simulering genereret");

                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Fejl ved generering af simulering");
            }

            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }

             
        }
    }
}
