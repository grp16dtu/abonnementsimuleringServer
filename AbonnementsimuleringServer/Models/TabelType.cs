using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class TabelType
    {
        public string Navn { get; set; }
        public string SqlOpret { get; set; }

        public TabelType(string navn, string sqlOpret)
        {
            Navn = navn;
            SqlOpret = sqlOpret;
        }
    }
}