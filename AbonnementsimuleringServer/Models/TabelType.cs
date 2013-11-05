using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class Kundetabel
    {
        public string Navn { get; set; }
        public string Oprettelsesstreng { get; set; }

        public Kundetabel(string navn, string oprettelsesstreng)
        {
            Navn = navn;
            Oprettelsesstreng = oprettelsesstreng;
        }
    }
}