using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace AbonnementsimuleringServer.Models
{
    public class MySQL
    {
        private MySqlConnection _mySqlForbindelse { get; set; }
        private string _mySqlServerUrl { get; set; }
        private string _mySqlDatabase { get; set; }
        private string _mySqlBrugernavn { get; set; }
        private string _mySqlKodeord { get; set; }

        private List<TabelType> _tabelTyper = new List<TabelType>();

        public MySQL()
        {
            _mySqlServerUrl = "MYSQL5004.smarterasp.net";
            _mySqlDatabase = "db_9ac26b_abosim";
            _mySqlBrugernavn = "9ac26b_abosim";
            _mySqlKodeord = "Kode1234";
            Initialiser();
            OpretTabelTyper();
        }

        private void Initialiser()
        {
            string tilslutning = "SERVER=" + _mySqlServerUrl + ";" + "DATABASE=" + _mySqlDatabase + ";" + "UID=" + _mySqlBrugernavn + ";" + "PASSWORD=" + _mySqlKodeord + ";";
            _mySqlForbindelse = new MySqlConnection(tilslutning);
        }

        private void Tilslut()
        {
            _mySqlForbindelse.Open();
        }

        private void Afbryd()
        {
            _mySqlForbindelse.Close();
        }

        private void ToemTabel(string tabelNavn)
        {
            EksekverNonForespoergsel("TRUNCATE TABLE " + tabelNavn);
        }

        public void IndsaetTransaktioner(List<Transaktion> transactions)
        {
            Tilslut();
            string mySqlStreng = "INSERT INTO transactions (yearMonth, departmentNumber, debtorNumber, productNumber, quantity, amount) VALUES";

            foreach (Transaktion transaction in transactions)
            {
                mySqlStreng += "('" + transaction.AarMaaned.ToString("yyyyMMdd") + "', '" + transaction.Afdelingsnummer + "', '" + transaction.Debitornummer + "', '" + transaction.Varenummer + "', '" + transaction.Antal + "', '" + transaction.Beloeb + "'),";
            }
            
            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1); // Slet sidste overflødige komma
            mySqlStreng += " ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity), amount= amount + VALUES(amount)";
            EksekverNonForespoergsel(mySqlStreng);
            Afbryd();
        }

        public DataSet HentAlt()
        {
            Tilslut();
            List<Datapunkt> datapunkter = new List<Datapunkt>();
            string forespoergsel = "SELECT yearMonth, Sum( quantity ) AS antal, SUM( amount ) AS sum FROM transactions GROUP BY yearMonth  ORDER BY yearMonth ASC LIMIT 10";
            DataSet datasaet = EksekverForespoergsel(forespoergsel);
            Afbryd();
            return datasaet;
        }

        private void EksekverNonForespoergsel(string mySqlStreng)
        {
            MySqlCommand mySqlKommando = new MySqlCommand(mySqlStreng, _mySqlForbindelse);
            mySqlKommando.ExecuteNonQuery();
        }

        private DataSet EksekverForespoergsel(string mySQLForespoergsel)
        {
            MySqlCommand cmd = new MySqlCommand(mySQLForespoergsel, _mySqlForbindelse);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.HasRows)
            {
                DataSet datasaet = new DataSet();
                datasaet.Load(dataReader, LoadOption.PreserveChanges, "MySqlData");
                return datasaet;
            }

            dataReader.Close();
            return null;
        }

        public void KlargoerKundeTabeller(int aftaleNummer)
        {
            Tilslut();

            foreach (var tabel in _tabelTyper)
            {
                string tabelNavn = aftaleNummer + tabel.Navn;

                if (TabelEksisterer(tabelNavn))
                {
                    ToemTabel(tabelNavn);
                }
                else
                {
                    string forespoergsel = "CREATE TABLE " + tabelNavn + tabel.SqlOpret;
                    EksekverForespoergsel(forespoergsel);
                }
            }



            Afbryd();
        }

        public void SletKundeTabeller(int aftaleNummer)
        {
            Tilslut();

            foreach (var tabel in _tabelTyper)
            {
                string tabelNavn = aftaleNummer + tabel.Navn;
                string forespoergsel = "DROP TABLE " + tabelNavn;
                EksekverNonForespoergsel(forespoergsel);
            }

            Afbryd();
        }

        private bool TabelEksisterer(string tabelNavn)
        {
            string forespoergsel = "SHOW TABLES LIKE '" + tabelNavn + "'";
            DataSet resultat = EksekverForespoergsel(forespoergsel);
            return resultat != null;
        } 

        private void OpretTabelTyper()
        {
            _tabelTyper.Add(new TabelType("varer", "(nummer VARCHAR(25), navn VARCHAR(300), kostpris DECIMAL, salgspris DECIMAL, volume DECIMAL, afdelingsnummer INT, PRIMARY KEY (nummer))"));
            _tabelTyper.Add(new TabelType("debitorer", "(nummer VARCHAR(9), navn VARCHAR(255), adresse VARCHAR(255), bynavn VARCHAR(255), land VARCHAR(255), email VARCHAR(255), postnummer VARCHAR(10), PRIMARY KEY (nummer))"));
            _tabelTyper.Add(new TabelType("afdelinger", "(nummer INT, navn VARCHAR(255), PRIMARY KEY (nummer))"));
            _tabelTyper.Add(new TabelType("transaktioner", "(aarMaaned DATETIME, debitornummer VARCHAR(10), varenummer VARCHAR(26), afdelingsnummer INT, antal DECIMAL, beloeb DECIMAL, PRIMARY KEY (aarMaaned, debitornummer, varenummer, afdelingsnummer))"));
        }
    }
}
