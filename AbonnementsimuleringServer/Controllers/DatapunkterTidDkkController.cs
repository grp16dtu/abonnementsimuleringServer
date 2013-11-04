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
            DataSet dataSet = mySql.DatapunkterTidDkk();

            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                datapunkter.Add(Datapunkt.DimKeyTidDKK((DateTime)item["tid"], (decimal)item["sum"]));
            }

            return datapunkter;
        }
    }
}
