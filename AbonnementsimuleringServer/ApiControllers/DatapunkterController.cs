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
        public HttpResponseMessage HentAlle(int id)
        {   
            try
            {
                DatapunktLister datapunktListe = new DatapunktLister();

                ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
                int economicAftalenummer = identitet.EconomicAftalenummer;
                
                MySQL mySql = new MySQL(identitet.EconomicAftalenummer);
                datapunktListe.TidAntal = Datapunkt.OpretListe(mySql.HentDatapunkterTidAntal(economicAftalenummer, id));
                datapunktListe.TidDKK = Datapunkt.OpretListe(mySql.HentDatapunkterTidDkk(economicAftalenummer, id));
                datapunktListe.VareAntal = Datapunkt.OpretListe(mySql.HentDatapunkterVareAntal(economicAftalenummer, id));
                datapunktListe.VareDKK = Datapunkt.OpretListe(mySql.HentDatapunkterVareDkk(economicAftalenummer, id));
                datapunktListe.AfdelingAntal = Datapunkt.OpretListe(mySql.HentDatapunkterAfdelingAntal(economicAftalenummer, id));
                datapunktListe.AfdelingDKK = Datapunkt.OpretListe(mySql.HentDatapunkterAfdelingDkk(economicAftalenummer, id));
                datapunktListe.DebitorAntal = Datapunkt.OpretListe(mySql.HentDatapunkterDebitorAntal(economicAftalenummer, id));
                datapunktListe.DebitorDKK = Datapunkt.OpretListe(mySql.HentDatapunkterDebitorDkk(economicAftalenummer, id));

                return Request.CreateResponse(HttpStatusCode.OK, datapunktListe);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Fejl: " + exception);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }
        }

        [HttpGet]
        [ActionName("HentOversigt")]
        [BasicAuth]
        public HttpResponseMessage HentOversigt()
        {
            try
            {
                ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
                MySQL mySql = new MySQL(identitet.EconomicAftalenummer);
                DataSet mySqlData = mySql.HentDatapunktslisterOversigt();
                List<Datapunktsgruppering> datapunktslisterOversigt = Datapunktsgruppering.HentListe(mySqlData);

                return Request.CreateResponse(HttpStatusCode.OK, datapunktslisterOversigt);
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
        }


    }
}
