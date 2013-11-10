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
        [ActionName("TidAntal")]
        [BasicAuth]
        public HttpResponseMessage TidAntal()
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();

            ApiIdentitet identitet = (ApiIdentitet)HttpContext.Current.User.Identity;
            Debug.WriteLine(identitet.Name);
           
            try
            {
                MySQL mySql = new MySQL(387892);
                DataSet datapunkterDatasaet = mySql.HentDatapunkterTidAntal();

                foreach (DataRow raekke in datapunkterDatasaet.Tables[0].Rows)
                {
                    DateTime tid = (DateTime)raekke["tid"];
                    decimal antal = (decimal)raekke["antal"];
                    Datapunkt datapunkt = Datapunkt.DimKeyTidAntal(tid, antal);
                    datapunkter.Add(datapunkt);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, datapunkter); 
        }

        [HttpGet]
        [ActionName("TidDkk")]
        [BasicAuth]
        public HttpResponseMessage TidDkk()
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();

            try
            {
                MySQL mySql = new MySQL(387892);
                DataSet datapunkterDatasaet = mySql.HentDatapunkterTidDkk();

                foreach (DataRow raekke in datapunkterDatasaet.Tables[0].Rows)
                {
                    DateTime tid = (DateTime)raekke["tid"];
                    decimal dkk = (decimal)raekke["sum"];
                    Datapunkt datapunkt = Datapunkt.DimKeyTidDKK(tid, dkk);
                    datapunkter.Add(datapunkt);
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, datapunkter);
        }  
    }
}
