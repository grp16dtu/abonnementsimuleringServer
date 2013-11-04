using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;
using AbonnementsimuleringServer.EconomicSOAP;

namespace AbonnementsimuleringServer.Models
{
    public class MySQL
    {
        public enum Tabelnavne {Varer, Afdelinger, Debitorer }

        private MySqlConnection _mySqlForbindelse { get; set; }
        private string _mySqlServerUrl { get; set; }
        private string _mySqlDatabase { get; set; }
        private string _mySqlBrugernavn { get; set; }
        private string _mySqlKodeord { get; set; }
        private int _economicAftalenummer;

        private List<TabelType> _tabelTyper = new List<TabelType>();

        public MySQL(int economicAftalenummer)
        {
            _economicAftalenummer = economicAftalenummer;
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

        public void IndsaetTransaktioner(List<Transaktion> transaktioner)
        {
            Tilslut();
            string mySqlStreng = MysqlStrengbyggerTransaktioner(transaktioner);
            EksekverNonForespoergsel(mySqlStreng);
            Afbryd();
        }

        public void IndsaetRelationeltData(EconomicUdtraek economicUdtraek)
        {
            Tilslut();
            IndsaetVarer(economicUdtraek);
            IndsaetDebitorer(economicUdtraek);
            IndsaetAfdelinger(economicUdtraek);
            Afbryd();
        }

        public DataSet HentDatapunkterTidDkk()
        {
            Tilslut();
            DataSet dataSet = EksekverForespoergsel("SELECT SUM(beloeb) as sum, aarMaaned as tid FROM " + _economicAftalenummer + "simuleringsdata GROUP BY aarMaaned");
            Afbryd();
            return dataSet;
        }

        public DataSet HentDatapunkterTidAntal()
        {
            Tilslut();
            DataSet dataSet = EksekverForespoergsel("SELECT SUM(antal) as antal, aarMaaned as tid FROM " + _economicAftalenummer + "simuleringsdata GROUP BY aarMaaned");
            Afbryd();
            return dataSet;
        }

        public void KlargoerKundeTabeller()
        {
            Tilslut();
            foreach (var tabel in _tabelTyper)
            {
                string tabelNavn = _economicAftalenummer + tabel.Navn;

                if (TabelEksisterer(tabelNavn))
                    ToemTabel(tabelNavn);
                
                else
                {
                    string forespoergsel = "CREATE TABLE " + tabelNavn + tabel.SqlOpret;
                    EksekverForespoergsel(forespoergsel);
                }
            }

            OpretMySqlView();
            Afbryd();
        }

        public void SletKundeTabeller()
        {
            Tilslut();
            foreach (var tabel in _tabelTyper)
            {
                string tabelNavn = _economicAftalenummer + tabel.Navn;
                string forespoergsel = "DROP TABLE " + tabelNavn;
                EksekverNonForespoergsel(forespoergsel);
            }
            EksekverNonForespoergsel("DROP VIEW " + _economicAftalenummer + "simuleringsdata");
            Afbryd();
        }

        private void OpretMySqlView()
        {
            string forespoergsel = "CREATE OR REPLACE VIEW " + _economicAftalenummer + "simuleringsdata AS SELECT aarMaaned, antal, beloeb, varekostpris, varesalgspris, varevolume, debitornavn, debitoradresse, debitorbynavn, debitorland, debitoremail, debitorpostnummer, af.afdelingsnavn FROM " + _economicAftalenummer + "transaktioner as tr NATURAL JOIN 387892varer as va NATURAL JOIN " + _economicAftalenummer + "debitorer as de LEFT JOIN " + _economicAftalenummer + "afdelinger af ON tr.afdelingsnummer = af.afdelingsnummer";
            EksekverNonForespoergsel(forespoergsel);
        }

        private void IndsaetVarer(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Varer);
            EksekverNonForespoergsel(mySqlStreng);
        }

        private void IndsaetDebitorer(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Debitorer);
            EksekverNonForespoergsel(mySqlStreng);
        }

        private void IndsaetAfdelinger(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Afdelinger);
            EksekverNonForespoergsel(mySqlStreng);
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

        private bool TabelEksisterer(string tabelNavn)
        {
            string forespoergsel = "SHOW TABLES LIKE '" + tabelNavn + "'";
            DataSet resultat = EksekverForespoergsel(forespoergsel);
            return resultat != null;
        } 

        private void OpretTabelTyper()
        {
            _tabelTyper.Add(new TabelType("varer", "(varenummer VARCHAR(25), varenavn VARCHAR(300), varekostpris DECIMAL, varesalgspris DECIMAL, varevolume DECIMAL, PRIMARY KEY (varenummer))"));
            _tabelTyper.Add(new TabelType("debitorer", "(debitornummer VARCHAR(9), debitornavn VARCHAR(255), debitoradresse VARCHAR(255), debitorbynavn VARCHAR(255), debitorland VARCHAR(255), debitoremail VARCHAR(255), debitorpostnummer VARCHAR(10), PRIMARY KEY (debitornummer))"));
            _tabelTyper.Add(new TabelType("afdelinger", "(afdelingsnummer INT, afdelingsnavn VARCHAR(255), PRIMARY KEY (afdelingsnummer))"));
            _tabelTyper.Add(new TabelType("transaktioner", "(aarMaaned DATETIME, debitornummer VARCHAR(10), varenummer VARCHAR(26), afdelingsnummer INT, antal DECIMAL, beloeb DECIMAL, PRIMARY KEY (aarMaaned, debitornummer, varenummer, afdelingsnummer))"));
        }

        private string MysqlStrengbyggerRelationeltData(EconomicUdtraek economicUdtraek, Tabelnavne tabeller)
        {
            string mySqlStreng = "START TRANSACTION; ";

            switch (tabeller)
            {
                case Tabelnavne.Varer:
                    mySqlStreng = "INSERT INTO " + _economicAftalenummer + "varer (varenummer, varenavn, varekostpris, varesalgspris, varevolume) VALUES";
                    foreach (ProductData vare in economicUdtraek.Produkter)
                    {
                        mySqlStreng += "('" + vare.Number + "', '" + vare.Name + "', '" + vare.CostPrice + "', '" + vare.SalesPrice + "', '" + vare.Volume + "'),";
                    }
                    break;

                case Tabelnavne.Afdelinger:
                    mySqlStreng = "INSERT INTO " + _economicAftalenummer + "afdelinger (afdelingsnummer, afdelingsnavn) VALUES";
                    foreach (DepartmentData afdeling in economicUdtraek.Afdelinger)
                    {
                        mySqlStreng += "('" + afdeling.Number + "', '" + afdeling.Name + "'),";
                    }
                    break;

                case Tabelnavne.Debitorer:
                    mySqlStreng = "INSERT INTO " + _economicAftalenummer + "debitorer (debitornummer, debitornavn, debitoradresse, debitorbynavn, debitorland, debitoremail, debitorpostnummer) VALUES";

                    foreach (DebtorData debitor in economicUdtraek.Debitorer)
                    {
                        mySqlStreng += "('" + debitor.Number + "', '" + debitor.Name + "', '" + debitor.Address + "', '" + debitor.City + "', '" + debitor.Country + "', '" + debitor.Email + "', '" + debitor.PostalCode + "'),";
                    }

                    break;
                default:
                    break;
            }

            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1);
            mySqlStreng += "; COMMIT;";

            return mySqlStreng;
        }

        private string MysqlStrengbyggerTransaktioner(List<Transaktion> transaktioner)
        {
            string mySqlStreng = "START TRANSACTION; ";
            mySqlStreng = "INSERT INTO " + _economicAftalenummer + "transaktioner (aarMaaned, debitornummer, varenummer, afdelingsnummer, antal, beloeb) VALUES";

            foreach (Transaktion transaktion in transaktioner)
            {
                mySqlStreng += "('" + transaktion.AarMaaned.ToString("yyyyMMdd") + "', '" + transaktion.Debitornummer + "', '" + transaktion.Varenummer + "', '" + transaktion.Afdelingsnummer + "', '" + transaktion.Antal + "', '" + transaktion.Beloeb + "'),";
            }

            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1);
            mySqlStreng += " ON DUPLICATE KEY UPDATE antal = antal + VALUES(antal), beloeb = beloeb + VALUES(beloeb); COMMIT;";
            return mySqlStreng;
        }
    }
}
