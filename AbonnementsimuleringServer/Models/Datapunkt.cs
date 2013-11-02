using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbonnementsimuleringServer.Models
{
    public class Datapunkt
    {
        // Key figures
        public decimal? Antal { get; set; }
        public decimal? DKK { get; set; }

        // Dimensioner
        public DateTime Tid { get; set; }
        public string Varenavn { get; set; }
        public string Debitornavn { get; set; }
        public string Afdelingsnavn { get; set; }

        public static Datapunkt DimKeyTidDKK(DateTime tid, decimal antal, decimal dkk)
        {
            Datapunkt datapunkt = new Datapunkt();
            datapunkt.Tid = tid;
            datapunkt.Antal = antal;
            datapunkt.DKK = dkk;
            return datapunkt;
        }
    }
}
