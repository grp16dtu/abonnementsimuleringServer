using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AbonnementsimuleringServer.Models
{
    public class Datapunktsgruppering
    {
        public int Id { get; set; }
        public DateTime Dato { get; set; }

        private Datapunktsgruppering(int id, DateTime dato)
        {
            Id = id;
            Dato = dato;
        }

        public static List<Datapunktsgruppering> HentListe(DataSet mySqlData)
        {
            List<Datapunktsgruppering> datapunktsgrupperingsliste = new List<Datapunktsgruppering>();

            foreach (DataRow datapunktData in mySqlData.Tables[0].Rows)
            {
                datapunktsgrupperingsliste.Add(new Datapunktsgruppering(Convert.ToInt32(datapunktData["simuleringsid"]), Convert.ToDateTime(datapunktData["tidsstempel"])));
            }

            return datapunktsgrupperingsliste;
        }
    }
}