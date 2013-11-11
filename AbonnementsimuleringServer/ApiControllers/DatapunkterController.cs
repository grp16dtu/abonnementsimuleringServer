using AbonnementsimuleringServer.Autorisation;
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
    public class DatapunkterController : ApiController
    {
        [HttpGet]
        [ActionName("HentAlle")]
        [BasicAuth]
        public HttpResponseMessage HentAlle()
        {   
            try
            {
                DatapunktLister datapunktListe = new DatapunktLister();

                ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
                MySQL mySql = new MySQL(identitet.EconomicAftalenummer);

                datapunktListe.TidAntal = Datapunkt.OpretListe(mySql.HentDatapunkterTidAntal());
                datapunktListe.TidDKK = Datapunkt.OpretListe(mySql.HentDatapunkterTidDkk());
                datapunktListe.VareAntal = Datapunkt.OpretListe(mySql.HentDatapunkterVareAntal());
                datapunktListe.VareDKK = Datapunkt.OpretListe(mySql.HentDatapunkterVareDkk());
                datapunktListe.AfdelingAntal = Datapunkt.OpretListe(mySql.HentDatapunkterAfdelingAntal());
                datapunktListe.AfdelingDKK = Datapunkt.OpretListe(mySql.HentDatapunkterAfdelingDkk());
                datapunktListe.DebitorAntal = Datapunkt.OpretListe(mySql.HentDatapunkterDebitorAntal());
                datapunktListe.DebitorDKK = Datapunkt.OpretListe(mySql.HentDatapunkterDebitorDkk());

                return Request.CreateResponse(HttpStatusCode.OK, datapunktListe);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Fejl: " + exception);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }
        }

        [HttpGet]
        [ActionName("HentOverblik")]
        [BasicAuth]
        public HttpResponseMessage HentOverblik()
        {
            try
            {
                ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
                MySQL mySql = new MySQL(identitet.EconomicAftalenummer);
                DataSet mySqlData = mySql.HentDatapunktsOverblikspunkter();
                List<Datapunktsgruppering> datapunktsoverblik = Datapunktsgruppering.HentListe(mySqlData);

                return Request.CreateResponse(HttpStatusCode.OK, datapunktsoverblik);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }


    }
}
