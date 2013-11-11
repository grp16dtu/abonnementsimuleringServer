using System;
using System.Collections.Generic;
using System.Data;
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
        public DateTime? Tid { get; set; }
        public string Varenavn { get; set; }
        public string Debitornavn { get; set; }
        public string Afdelingsnavn { get; set; }

        public static List<Datapunkt> OpretListe(DataSet datapunkterDatasaet)
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();

            foreach (DataRow raekke in datapunkterDatasaet.Tables[0].Rows)
            {
                Datapunkt datapunkt = new Datapunkt();
                datapunkt.Tid = null;

                if (raekke.Table.Columns.Contains("antal") && raekke["antal"] != DBNull.Value)
                    datapunkt.Antal = (decimal)raekke["antal"];

                if (raekke.Table.Columns.Contains("dkk") && raekke["dkk"] != DBNull.Value)
                    datapunkt.DKK = (decimal)raekke["dkk"];

                if (raekke.Table.Columns.Contains("tid") && raekke["tid"] != DBNull.Value)
                    datapunkt.Tid = (DateTime)raekke["tid"];

                if (raekke.Table.Columns.Contains("vare") && raekke["vare"] != DBNull.Value)
                    datapunkt.Varenavn = (string)raekke["vare"];

                if (raekke.Table.Columns.Contains("debitor") && raekke["debitor"] != DBNull.Value)
                    datapunkt.Debitornavn = (string)raekke["debitor"];

                if (raekke.Table.Columns.Contains("afdeling") && raekke["afdeling"] != DBNull.Value)
                    datapunkt.Afdelingsnavn = (string)raekke["afdeling"];

                datapunkter.Add(datapunkt);
            }

            return datapunkter;
        }
    }
}
