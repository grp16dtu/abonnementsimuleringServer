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
        private enum Tabelnavne {Varer, Afdelinger, Debitorer}

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
        }

        private void InitialiserForbindelse()
        {
            _mySqlForbindelse = new MySqlConnection("SERVER=" + _mySqlServerUrl + ";" + "DATABASE=" + _mySqlDatabase + ";" + "UID=" + _mySqlBrugernavn + ";" + "PASSWORD=" + _mySqlKodeord + ";");
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

        public DataSet HentDatapunkterTidAntal(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " +economicAftalenummer+"_" +id+"_tidantal");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterTidDkk(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_tiddkk");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterAfdelingAntal(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_afdelingantal");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterAfdelingDkk(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_afdelingdkk");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterDebitorAntal(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_debitorantal");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterDebitorDkk(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_debitordkk");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterVareAntal(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_vareantal");
            AfbrydMysql();
            return datasaet;
        }

        public DataSet HentDatapunkterVareDkk(int economicAftalenummer, int id)
        {
            TilslutMysql();
            DataSet datasaet = FraDatabase("SELECT * FROM " + economicAftalenummer + "_" + id + "_varedkk");
            AfbrydMysql();
            return datasaet;
        }

        public void KlargoerTabeller(string tidsstempel)
        {
            TilslutMysql();
            KaldKlargoerTabellerRutine(_economicAftalenummer, tidsstempel);
            AfbrydMysql();
        }

        public void SletKundeTabeller()
        {
            TilslutMysql();
            KaldSletTabellerRutine(_economicAftalenummer);
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

        private void KaldKlargoerTabellerRutine(int economicAftalenummer, string tidsstempel)
        {
            MySqlCommand mySqlKommando = new MySqlCommand("KlargoerTabeller", _mySqlForbindelse);
            mySqlKommando.Parameters.AddWithValue("@economicAftalenummer", economicAftalenummer.ToString());
            mySqlKommando.Parameters.AddWithValue("@tidsstempel", tidsstempel);
            Debug.WriteLine(DateTime.Now.ToString());
            mySqlKommando.CommandType = CommandType.StoredProcedure; 
            mySqlKommando.ExecuteNonQuery(); 
        }

        private void KaldSletTabellerRutine(int economicAftalenummer)
        {
            MySqlCommand mySqlKommando = new MySqlCommand("SletTabeller", _mySqlForbindelse);
            mySqlKommando.Parameters.AddWithValue("@economicAftalenummer", economicAftalenummer.ToString());
            mySqlKommando.CommandType = CommandType.StoredProcedure;
            mySqlKommando.ExecuteNonQuery();
        }

        private void KaldOpretDatapunktslisterRutine(int economicAftalenummer)
        {
            MySqlCommand mySqlKommando = new MySqlCommand("OpretDatapunktslister", _mySqlForbindelse);
            mySqlKommando.Parameters.AddWithValue("@economicAftalenummer", economicAftalenummer.ToString());
            mySqlKommando.CommandType = CommandType.StoredProcedure;
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

        private DataRow LinjeFraDatabase(string forespoergsel)
        {
            MySqlCommand mySqlKommando = new MySqlCommand(forespoergsel, _mySqlForbindelse);
            MySqlDataReader mySqlData = mySqlKommando.ExecuteReader();

            if (mySqlData.HasRows)
            {
                DataSet datasaet = new DataSet();
                datasaet.Load(mySqlData, LoadOption.PreserveChanges, "MySqlData");
                return datasaet.Tables[0].Rows[0];
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

        public DataSet HentEconomicOplysninger(string brugernavn, string kodeord)
        {
            TilslutMysql();
            string forespoergsel = "SELECT kodeord, economicAftalenummer, economicBrugernavn, economicKodeord FROM brugerautorisation WHERE brugernavn = '" + brugernavn + "'";
            DataSet dataSet = FraDatabase(forespoergsel);
            AfbrydMysql();

            if (dataSet != null && dataSet.Tables["MySqlData"].Rows[0]["kodeord"].ToString() == kodeord)
            {
                return dataSet;
            }

            return null;
        }

        public int HentEconomicAftalenummer(string brugernavn)
        {
            TilslutMysql();
            string forespoergsel = "SELECT economicAftalenummer FROM brugerautorisation WHERE brugernavn = '" + brugernavn + "'";
            DataRow dataRow = LinjeFraDatabase(forespoergsel);
            AfbrydMysql();

            return (int)dataRow["economicAftalenummer"];
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

        public DataSet HentAlleBrugere()
        {
            TilslutMysql();
            string forespoergsel = "SELECT * FROM brugere";
            DataSet mySqlData = FraDatabase(forespoergsel);
            AfbrydMysql();
            return mySqlData;
        }

        public void OpretBruger(Bruger bruger, int economicAftalenummer)
        {
            string forespoergsel;
            int economicAftalenummerId;
            DataSet dataSet;
            TilslutMysql();

            // Hent economicAftlenummerId ud fra economicAftlenummer
            string mySqlFindId = "SELECT economicAftalenummerId FROM economicAftalenumre WHERE economicAftalenummer = '" + economicAftalenummer + "'";
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

        public void SletBruger(string brugernavn)
        {
            TilslutMysql();
            string mySqlStreng = "DELETE FROM brugere WHERE brugernavn = '" + brugernavn +"'";
            Debug.WriteLine(mySqlStreng);
            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }

        public bool BrugerEksisterer(string brugernavn)
        {
            TilslutMysql();
            DataSet mySqlData = FraDatabase("SELECT * FROM brugere WHERE brugernavn='" + brugernavn + "' LIMIT 1");
            AfbrydMysql();
            return mySqlData != null;
        }

        public bool AftalenummerEksisterer(int aftalenummer)
        {
            TilslutMysql();
            Debug.WriteLine("hej1");
            DataSet mySqlData = FraDatabase("SELECT * FROM economicaftalenumre WHERE economicAftalenummer='" + aftalenummer + "' LIMIT 1");
            Debug.WriteLine("hej");
            AfbrydMysql();
            return mySqlData != null;
        }

        public void OpretAftalenummer(int economicaftalenummer, string economicbrugernavn, string economickodeord)
        {
            TilslutMysql();
            string mySqlStreng = "INSERT INTO economicaftalenumre (economicAftalenummerId, economicAftalenummer,economicBrugernavn,economicKodeord) VALUES('0','"+economicaftalenummer+"','"+economicbrugernavn+"','"+economickodeord+"')";
            Debug.WriteLine(mySqlStreng);
            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }

        public void IndsaetDatapunkter(List<Datapunkt> datapunkter, string tabelnavn)
        {
            TilslutMysql();
            string mySqlStreng = "INSERT INTO " + tabelnavn + " (antal, dkk, tid, varenavn, debitornavn, afdelingsnavn) VALUES ";

            foreach (Datapunkt datapunkt in datapunkter)
            {
                string tid = "";
                if (datapunkt.Tid != null)
                {
                    DateTime temp = (DateTime)datapunkt.Tid;
                    tid = temp.ToString("yyyy-MM-dd HH:mm:ss");
                }

                mySqlStreng = mySqlStreng + "('" + datapunkt.Antal + "', '" + datapunkt.DKK + "', '" + tid + "', '" + datapunkt.Varenavn + "', '" + datapunkt.Debitornavn + "', '" + datapunkt.Afdelingsnavn + "'),";
            }

            mySqlStreng = mySqlStreng.Remove(mySqlStreng.Length - 1, 1);

            TilDatabase(mySqlStreng);
            AfbrydMysql();
        }

        public DataSet HentSimuleringsId(string tidsstempel)
        {
            TilslutMysql();
            string mySqlStreng = "SELECT simuleringsid FROM " + _economicAftalenummer + "simuleringsoversigt WHERE tidsstempel = '" + tidsstempel + "'";
            DataSet mySqlData = FraDatabase(mySqlStreng);
            AfbrydMysql();
            return mySqlData;
        }

        public DataSet HentDatapunktslisterOversigt()
        {
            TilslutMysql();
            string mySqlStreng = "SELECT * FROM " + _economicAftalenummer + "simuleringsoversigt ORDER BY tidsstempel DESC";
            DataSet mySqlData = FraDatabase(mySqlStreng);
            AfbrydMysql();
            return mySqlData;
        }

        internal void OpretDatapunktslister()
        {
            TilslutMysql();
            KaldOpretDatapunktslisterRutine(_economicAftalenummer);
            AfbrydMysql();
        }
    }
}
