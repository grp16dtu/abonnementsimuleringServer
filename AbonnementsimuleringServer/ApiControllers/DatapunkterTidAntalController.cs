using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.ApiControllers
{
    public class DatapunkterTidAntalController : ApiController
    {
        public HttpResponseMessage Get()
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();

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
    }
}
