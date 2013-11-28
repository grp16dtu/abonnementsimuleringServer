using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AbonnementsimuleringServer.ApiControllers;
using AbonnementsimuleringServer.Autorisation;
using AbonnementsimuleringServer.Controllers;
using AbonnementsimuleringServer.EconomicSOAP;
using AbonnementsimuleringServer.Helpers;
using AbonnementsimuleringServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace EconomicControllerTests
{
    [TestClass]
    public class GenererTransaktionerTests
    {
        [TestMethod]
        public void GenTransIntervalWeekTest()
        {
            decimal forventetAntal = 5 * 53;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "Week";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalTwoWeeksTest()
        {
            decimal forventetAntal = 5 * 27;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "TwoWeeks";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalFourWeeksTest()
        {
            decimal forventetAntal = 5 * 14;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "FourWeeks";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalMonthTest()
        {
            decimal forventetAntal = 5 * 12;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "Month";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalEightWeeksTest()
        {
            decimal forventetAntal = 5 * 7;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "EightWeeks";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalTwoMonthsTest()
        {
            decimal forventetAntal = 5 * 6;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "TwoMonths";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalQuarterTest()
        {
            decimal forventetAntal = 5 * 4;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "Quarter";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalHalfYearTest()
        {
            decimal forventetAntal = 5 * 2;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "HalfYear";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalYearTest()
        {
            decimal forventetAntal = 5 * 1;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "Year";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalTwoYearsTest()
        {
            decimal forventetAntal = 5 * 1;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "TwoYears";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalThreeYearsTest()
        {
            decimal forventetAntal = 5 * 1;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "ThreeYears";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalFourYearsTest()
        {
            decimal forventetAntal = 5 * 1;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "FourYears";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }

        [TestMethod]
        public void GenTransIntervalFiveYearsTest()
        {
            decimal forventetAntal = 5 * 1;
            decimal forventetPris = forventetAntal * 100;
            string abonnementInterval = "FiveYears";

            IntervalTest(forventetAntal, forventetPris, abonnementInterval);
        }
   
        private void IntervalTest(decimal forventetAntal, decimal forventetPris, string abonnementInterval)
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentSlut = DateTime.MaxValue;
            DateTime? abonnentUdloeb = DateTime.MaxValue;

            string abonnementOpkraevning = "Full";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);


            // act
            EconomicController ec = new EconomicController(387892, "DTU", "Trustno1");
            List<Transaktion> faktisk = ec.GenererTransaktioner(abonnementer, simuleringsMaaneder, brugerIndex);


            // assert
            decimal faktiskPris = 0, faktiskAntal = 0;
            foreach (Transaktion transaktion in faktisk)
            {
                faktiskPris = faktiskPris + transaktion.Beloeb;
                faktiskAntal = faktiskAntal + (decimal)transaktion.Antal;
            }

            Assert.AreEqual(forventetPris, faktiskPris);
            Assert.AreEqual(forventetAntal, faktiskAntal);
        }
    
    }
}
