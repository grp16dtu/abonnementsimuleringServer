using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class DatapunktLister
    {
        public List<Datapunkt> TidAntal { get; set; }
        public List<Datapunkt> TidDKK { get; set; }
        public List<Datapunkt> VareAntal { get; set; }
        public List<Datapunkt> VareDKK { get; set; }
        public List<Datapunkt> AfdelingAntal { get; set; }
        public List<Datapunkt> AfdelingDKK { get; set; }
        public List<Datapunkt> DebitorAntal { get; set; }
        public List<Datapunkt> DebitorDKK { get; set; }

        public DatapunktLister()
        {
            TidAntal = new List<Datapunkt>();
            TidDKK = new List<Datapunkt>();
            VareAntal = new List<Datapunkt>();
            VareDKK = new List<Datapunkt>();
            AfdelingAntal = new List<Datapunkt>();
            AfdelingDKK = new List<Datapunkt>();
            DebitorAntal = new List<Datapunkt>();
            DebitorDKK = new List<Datapunkt>();
        }
    }
}