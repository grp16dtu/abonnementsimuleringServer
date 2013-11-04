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
        public IEnumerable<Transaktion> Get(int id)
        {
            EconomicController economic = new EconomicController(387892, "DTU", "Trustno1");
            return economic.GenererNySimulering(12, 1);
        }
    }
}
