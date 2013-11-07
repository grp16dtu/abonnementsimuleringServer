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
        private enum Tabelnavne {Varer, Afdelinger, Debitorer }
        private List<Kundetabel> _kundetabeller = new List<Kundetabel>();

        private MySqlConnection _mySqlForbindelse { get; set; }
        private string _mySqlServerUrl { get; set; }
        private string _mySqlDatabase { get; set; }
        private string _mySqlBrugernavn { get; set; }
        private string _mySqlKodeord { get; set; }
        private int _economicAftalenummer;

        public MySQL(int economicAftalenummer)
        {
            _economicAftalenummer = economicAftalenummer;
            _mySqlServerUrl = "MYSQL5004.smarterasp.net";
            _mySqlDatabase = "db_9ac26b_abosim";
            _mySqlBrugernavn = "9ac26b_abosim";
            _mySqlKodeord = "Kode1234";
            Initialiser();
        }

        public MySQL()
        {
            _mySqlServerUrl = "MYSQL5004.smarterasp.net";
            _mySqlDatabase = "db_9ac26b_abosim";
            _mySqlBrugernavn = "9ac26b_abosim";
            _mySqlKodeord = "Kode1234";
            Initialiser();
        }

        private void Initialiser()
        {
            InitialiserForbindelse();
            InitialiserKundetabeller();
        }

        private void InitialiserForbindelse()
        {
            _mySqlForbindelse = new MySqlConnection("SERVER=" + _mySqlServerUrl + ";" + "DATABASE=" + _mySqlDatabase + ";" + "UID=" + _mySqlBrugernavn + ";" + "PASSWORD=" + _mySqlKodeord + ";");
        }

        private void InitialiserKundetabeller()
        {
            _kundetabeller.Add(new Kundetabel(_economicAftalenummer + "varer", "CREATE TABLE " + _economicAftalenummer + "varer (varenummer VARCHAR(25), varenavn VARCHAR(300), varekostpris DECIMAL, varesalgspris DECIMAL, varevolume DECIMAL, PRIMARY KEY (varenummer))"));
            _kundetabeller.Add(new Kundetabel(_economicAftalenummer + "debitorer", "CREATE TABLE " + _economicAftalenummer + "debitorer (debitornummer VARCHAR(9), debitornavn VARCHAR(255), debitoradresse VARCHAR(255), debitorbynavn VARCHAR(255), debitorland VARCHAR(255), debitoremail VARCHAR(255), debitorpostnummer VARCHAR(10), PRIMARY KEY (debitornummer))"));
            _kundetabeller.Add(new Kundetabel(_economicAftalenummer + "afdelinger", "CREATE TABLE " + _economicAftalenummer + "afdelinger (afdelingsnummer INT, afdelingsnavn VARCHAR(255), PRIMARY KEY (afdelingsnummer))"));
            _kundetabeller.Add(new Kundetabel(_economicAftalenummer + "transaktioner", "CREATE TABLE " + _economicAftalenummer + "transaktioner (aarMaaned DATETIME, debitornummer VARCHAR(10), varenummer VARCHAR(26), afdelingsnummer INT, antal DECIMAL, beloeb DECIMAL, PRIMARY KEY (aarMaaned, debitornummer, varenummer, afdelingsnummer))"));
        }

        public void IndsaetTransaktioner(List<Transaktion> transaktioner)
        {
            TilslutMysql();
            string mySqlStreng = MysqlStrengbyggerTransaktioner(transaktioner);
            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }

        public void IndsaetRelationeltData(EconomicUdtraek economicUdtraek)
        {
            TilslutMysql();
            IndsaetVarer(economicUdtraek);
            IndsaetDebitorer(economicUdtraek);
            IndsaetAfdelinger(economicUdtraek);
            AfbrydMysql();
        }

        public DataSet HentDatapunkterTidDkk()
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT SUM(beloeb) as sum, aarMaaned as tid FROM " + _economicAftalenummer + "simuleringsdata GROUP BY aarMaaned");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterTidAntal()
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT SUM(antal) as antal, aarMaaned as tid FROM " + _economicAftalenummer + "simuleringsdata GROUP BY aarMaaned");
            AfbrydMysql();
            return datasaet;
        }

        public void KlargoerTabeller()
        {
            TilslutMysql();
            foreach (var tabel in _kundetabeller)
            {
                if (TabelEksisterer(tabel.Navn))
                    ToemTabel(tabel.Navn);

                else
                    OpretTabel(tabel.Oprettelsesstreng);
            }

            OpretView();
            AfbrydMysql();
        }

        public void SletKundeTabeller()
        {
            TilslutMysql();
            foreach (var tabel in _kundetabeller)
            {
                SletTabel(tabel.Navn);
            }
            SletView();
            AfbrydMysql();
        }

        private void TilslutMysql()
        {
            _mySqlForbindelse.Open();
        }

        private void AfbrydMysql()
        {
            _mySqlForbindelse.Close();
        }

        private void OpretTabel(string oprettelsesstreng)
        {
            TilDatabase(oprettelsesstreng);
        }

        private void ToemTabel(string tabelnavn)
        {
            TilDatabase("TRUNCATE TABLE " + tabelnavn);
        }

        private void SletTabel(string tabelnavn)
        {
            TilDatabase("DROP TABLE " + tabelnavn);
        }

        private void OpretView()
        {
            TilDatabase("CREATE OR REPLACE VIEW " + _economicAftalenummer + "simuleringsdata AS SELECT aarMaaned, antal, beloeb, varekostpris, varesalgspris, varevolume, debitornavn, debitoradresse, debitorbynavn, debitorland, debitoremail, debitorpostnummer, af.afdelingsnavn FROM " + _economicAftalenummer + "transaktioner as tr NATURAL JOIN 387892varer as va NATURAL JOIN " + _economicAftalenummer + "debitorer as de LEFT JOIN " + _economicAftalenummer + "afdelinger af ON tr.afdelingsnummer = af.afdelingsnummer");
        }

        private void SletView()
        {
            TilDatabase("DROP VIEW " + _economicAftalenummer + "simuleringsdata");
        }

        private void IndsaetVarer(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Varer);
            TilDatabase(mySqlStreng);
        }

        private void IndsaetDebitorer(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Debitorer);
            TilDatabase(mySqlStreng);
        }

        private void IndsaetAfdelinger(EconomicUdtraek economicUdtraek)
        {
            string mySqlStreng = MysqlStrengbyggerRelationeltData(economicUdtraek, Tabelnavne.Afdelinger);
            TilDatabase(mySqlStreng);
        }

        private void TilDatabase(string mySqlStreng)
        {
            MySqlCommand mySqlKommando = new MySqlCommand(mySqlStreng, _mySqlForbindelse);
            mySqlKommando.ExecuteNonQuery();
        }

        private DataSet FraDatabase(string forespoergsel)
        {
            MySqlCommand mySqlKommando = new MySqlCommand(forespoergsel, _mySqlForbindelse);
            MySqlDataReader mySqlData = mySqlKommando.ExecuteReader();

            if (mySqlData.HasRows)
            {
                DataSet datasaet = new DataSet();
                datasaet.Load(mySqlData, LoadOption.PreserveChanges, "MySqlData");
                return datasaet;
            }

            mySqlData.Close();
            return null;
        }

        private bool TabelEksisterer(string tabelNavn)
        {
            DataSet resultat = FraDatabase("SHOW TABLES LIKE '" + tabelNavn + "'");
            return resultat != null;
        } 

        private string MysqlStrengbyggerRelationeltData(EconomicUdtraek economicUdtraek, Tabelnavne tabelnavne)
        {
            string mySqlStreng = "START TRANSACTION; ";

            switch (tabelnavne)
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

        public int? HentEconomicAftalenummer(string brugernavn, string kodeord)
        {
            TilslutMysql();
            string forespoergsel = "SELECT kodeord, aftalenummer FROM brugerautorisation WHERE brugernavn = '" + brugernavn + "'";
            DataSet dataSet = FraDatabase(forespoergsel);
            AfbrydMysql();

            if (dataSet != null && dataSet.Tables["MySqlData"].Rows[0]["kodeord"].ToString() == kodeord)
            {
                return (int)(dataSet.Tables["MySqlData"].Rows[0]["aftalenummer"]);
            }

            return null;
        }

        public DataSet HentBruger(string brugernavn, string kodeord)
        {
            TilslutMysql();
            string forespoergsel = "SELECT * FROM brugere WHERE brugernavn = '" + brugernavn + "'";
            DataSet dataSet = FraDatabase(forespoergsel);
            AfbrydMysql();

            if (dataSet != null && dataSet.Tables["MySqlData"].Rows[0]["kodeord"].ToString() == kodeord)
                return dataSet;
           
            else
                return null;
        }

        public void OpretBruger(Bruger bruger, int economicAftalenummer)
        {
            string forespoergsel;
            int economicAftalenummerId;
            DataSet dataSet;
            TilslutMysql();

            // Hent economicAftlenummerId ud fra economicAftlenummer
            string mySqlFindId = "SELECT economicAftalenummerId FROM economicAftalenumre WHERE aftalenummer = '" + economicAftalenummer + "'";
            dataSet = FraDatabase(mySqlFindId);
            if(dataSet != null)
            {
                economicAftalenummerId = (int)dataSet.Tables["MySqlData"].Rows[0]["economicAftalenummerId"];
            }
            else
            {
                forespoergsel = "INSERT INTO economicAftalnumre (aftalenummer) VALUES ('" + economicAftalenummer + "')";
                TilDatabase(forespoergsel);
                dataSet = FraDatabase(mySqlFindId);
                economicAftalenummerId = (int)dataSet.Tables["MySqlData"].Rows[0]["economicAftalenummerId"];
            }

            //Indsæt bruger
            forespoergsel = "INSERT INTO brugere (brugernavn, kodeord, brugerMedarbejdernummer, brugerFornavn, brugerEfternavn, economicAftalenummerId, erAnsvarlig) VALUES ";
            forespoergsel = forespoergsel + "('" + bruger.Brugernavn + "', '" + bruger.Kodeord + "', '" + bruger.MedarbejderNummer + "', '" + bruger.Fornavn + "', '" + bruger.Efternavn + "', '" + economicAftalenummerId + "', '" + Convert.ToInt32(bruger.Ansvarlig) + "')";
            Debug.WriteLine(forespoergsel);
            TilDatabase(forespoergsel);
            AfbrydMysql();
        }

        public void RedigerBruger(Bruger bruger)
        {
            TilslutMysql();
            string mySqlStreng = "UPDATE brugere SET brugernavn = '" + bruger.Brugernavn + "', kodeord = '" + bruger.Kodeord + "', brugerMedarbejdernummer ='" + bruger.MedarbejderNummer + "', brugerFornavn = '" + bruger.Fornavn + "', brugerEfternavn = '" + bruger.Efternavn + "', erAnsvarlig = '" + Convert.ToInt32(bruger.Ansvarlig) + "' WHERE brugernavn = '" + bruger.Brugernavn + "'";
            Debug.WriteLine(mySqlStreng);
            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }

        public void SletBruger(Bruger bruger)
        {
            TilslutMysql();
            string mySqlStreng = "DELETE FROM brugere WHERE brugernavn = '" + bruger.Brugernavn +"'";
            Debug.WriteLine(mySqlStreng);
            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }
    }
}
