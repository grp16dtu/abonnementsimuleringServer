using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;

namespace AbonnementsimuleringServer.Models
{
    public class MySQL
    {
        private MySqlConnection _mySqlForbindelse { get; set; }
        private string _mySqlServerUrl { get; set; }
        private string _mySqlDatabase { get; set; }
        private string _mySqlBrugernavn { get; set; }
        private string _mySqlKodeord { get; set; }

        public MySQL()
        {
            _mySqlServerUrl = "mysql23.unoeuro.com";
            _mySqlDatabase = "redlaz_dk_db";
            _mySqlBrugernavn = "redlaz_dk";
            _mySqlKodeord = "Iben1234";
            Initialiser();
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

        private void ToemTransaktioner()
        {
            EksekverNonForespoergsel("TRUNCATE TABLE transactions");
        }

        public void IndsaetTransaktioner(List<Transaktion> transactions)
        {
            ToemTransaktioner();
            string mySqlStreng = "INSERT INTO transactions (yearMonth, departmentNumber, debtorNumber, productNumber, quantity, amount) VALUES";

            foreach (Transaktion transaction in transactions)
            {
                mySqlStreng += "('" + transaction.AarMaaned.ToString("yyyyMMdd") + "', '" + transaction.Afdelingsnummer + "', '" + transaction.Debitornummer + "', '" + transaction.Varenummer + "', '" + transaction.Antal + "', '" + transaction.Beloeb + "'),";
            }
            
            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1); // Slet sidste overflødige komma
            mySqlStreng += " ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity), amount= amount + VALUES(amount)";
            EksekverNonForespoergsel(mySqlStreng);
        }

        public void IndsaetTransaktioner2(List<Transaktion> transactions)
        {
            ToemTransaktioner();
            string mySqlStreng = "";

            foreach (Transaktion transaction in transactions)
            {
                mySqlStreng += "('" + transaction.AarMaaned.ToString("yyyyMMdd") + "', '" + transaction.Afdelingsnummer + "', '" + transaction.Debitornummer + "', '" + transaction.Varenummer + "', '" + transaction.Antal + "', '" + transaction.Beloeb + "'),";
            }

            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1); // Slet sidste overflødige komma
            //mySqlStreng += " ON DUPLICATE KEY UPDATE quantity = quantity + VALUES(quantity), amount= amount + VALUES(amount)";
            EksekverNonForespoergsel(mySqlStreng);
        }

        public DataSet HentAlt()
        {
            List<Datapunkt> datapunkter = new List<Datapunkt>();
            string forespoergsel = "SELECT yearMonth, Sum( quantity ) AS antal, SUM( amount ) AS sum FROM transactions GROUP BY yearMonth  ORDER BY yearMonth ASC LIMIT 10";
            DataSet datasaet = EksekverForespoergsel(forespoergsel);
            return datasaet;
        }

        private void EksekverNonForespoergsel(string mySqlStreng)
        {
            Tilslut();
            MySqlCommand mySqlKommando = new MySqlCommand(mySqlStreng, _mySqlForbindelse);
            mySqlKommando.ExecuteNonQuery();
            Afbryd();
        }

        private DataSet EksekverForespoergsel(string mySQLForespoergsel)
        {
            Tilslut();
            MySqlCommand cmd = new MySqlCommand(mySQLForespoergsel, _mySqlForbindelse);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            DataSet datasaet = new DataSet();
            datasaet.Load(dataReader, LoadOption.PreserveChanges, "MySqlData");
            Afbryd();
            return datasaet;
        }
    }
}
