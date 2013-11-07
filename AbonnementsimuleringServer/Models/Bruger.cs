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
        public int MedarbejderNummer { get; set; }
        public bool Ansvarlig { get; set; }
        public string Brugernavn { get; set; }
        public string Kodeord { get; set; }

        public Bruger(DataSet dataSet)
        {
            Fornavn = dataSet.Tables["MySqlData"].Rows[0]["brugerFornavn"].ToString();
            Efternavn = dataSet.Tables["MySqlData"].Rows[0]["brugerEfternavn"].ToString();
            MedarbejderNummer = (int)dataSet.Tables["MySqlData"].Rows[0]["brugerMedarbejdernummer"];
            Ansvarlig = (bool)dataSet.Tables["MySqlData"].Rows[0]["erAnsvarlig"];
            Brugernavn = dataSet.Tables["MySqlData"].Rows[0]["brugernavn"].ToString();
            Kodeord = dataSet.Tables["MySqlData"].Rows[0]["kodeord"].ToString();
        }

        public Bruger()
        { 
        }
    }
}