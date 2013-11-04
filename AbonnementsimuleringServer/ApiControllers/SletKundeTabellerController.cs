using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbonnementsimuleringServer.Controllers
{
    public class SletKundeTabellerController : ApiController
    {
        public bool Get(int id) 
        {
            try
            {
                MySQL mySql = new MySQL(id);
                mySql.SletKundeTabeller();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
