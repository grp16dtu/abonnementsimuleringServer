using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class Bruger
    {
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public int? MedarbejderNummer { get; set; }
        public bool Ansvarlig { get; set; }
        public string Brugernavn { get; set; }
        public string Kodeord { get; set; }

        public Bruger()
        {
        }
        
        public Bruger(DataSet dataSet)
        {
            Fornavn = dataSet.Tables["MySqlData"].Rows[0]["brugerFornavn"].ToString();
            Efternavn = dataSet.Tables["MySqlData"].Rows[0]["brugerEfternavn"].ToString();

            if (!Convert.IsDBNull(dataSet.Tables["MySqlData"].Rows[0]["brugerMedarbejdernummer"]))
                MedarbejderNummer = Convert.ToInt32(dataSet.Tables["MySqlData"].Rows[0]["brugerMedarbejdernummer"]);
            
            Ansvarlig = (bool)dataSet.Tables["MySqlData"].Rows[0]["erAnsvarlig"];
            Brugernavn = dataSet.Tables["MySqlData"].Rows[0]["brugernavn"].ToString();
            Kodeord = dataSet.Tables["MySqlData"].Rows[0]["kodeord"].ToString();
        }

        public static List<Bruger> ListeAfBrugere(DataSet mySqlData)
        {
            List<Bruger> brugere = new List<Bruger>();
            foreach (DataRow brugerData in mySqlData.Tables["MySqlData"].Rows)
            {
                Bruger bruger = new Bruger();

                bruger.Fornavn = brugerData["brugerFornavn"].ToString();
                bruger.Efternavn = brugerData["brugerEfternavn"].ToString();

                if (!Convert.IsDBNull(brugerData["brugerMedarbejdernummer"]))
                    bruger.MedarbejderNummer = Convert.ToInt32(brugerData["brugerMedarbejdernummer"]);

                bruger.Ansvarlig = (bool)brugerData["erAnsvarlig"];
                bruger.Brugernavn = brugerData["brugernavn"].ToString();
                bruger.Kodeord = brugerData["kodeord"].ToString();
                brugere.Add(bruger);
            }
            return brugere;
        }
    }
}