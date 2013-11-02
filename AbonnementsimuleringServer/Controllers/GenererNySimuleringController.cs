using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    public class GenererNySimuleringController : ApiController
    {
        // GET api/generernysimulering
        public IEnumerable<Transaktion> Get()
        {
            EconomicController economic = new EconomicController(387892, "DTU", "Trustno1");
            return economic.GenererNySimulering(12, 1); 
        }

        // GET api/generernysimulering/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/generernysimulering
        public void Post([FromBody]string value)
        {
        }

        // PUT api/generernysimulering/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/generernysimulering/5
        public void Delete(int id)
        {
        }
    }
}
