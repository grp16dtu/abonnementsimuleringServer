using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    public class DatapunkterTidDkkController : ApiController
    {
        public List<Datapunkt> Get()
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();
            
            MySQL mySql = new MySQL(387892);
            DataSet datapunkterDatasaet = mySql.HentDatapunkterTidDkk();

            foreach (DataRow raekke in datapunkterDatasaet.Tables[0].Rows)
            {
                DateTime tid = (DateTime)raekke["tid"];
                decimal dkk = (decimal)raekke["sum"];
                Datapunkt datapunkt = Datapunkt.DimKeyTidDKK(tid, dkk);
                datapunkter.Add(datapunkt);
            }

            return datapunkter;
        }
    }
}
