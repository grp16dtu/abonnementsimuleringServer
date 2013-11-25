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

/*
namespace EconomicControllerTests
{
    [TestClass]
    public class GenererTransaktionerIntervalTests
    {
        [TestMethod]
        public void BasalTest1UgeProportional()
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
            DateTime abonnentStart = DateTime.Parse("29-05-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("28-10-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Week";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 109.29M;
            decimal forventetPris = 10929.00M;


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

        [TestMethod]
        public void BasalTest1MaanedProportional()
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
            DateTime abonnentStart = DateTime.Parse("29-05-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("28-10-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 24.68M;
            decimal forventetPris = 2468.00M;


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

        [TestMethod]
        public void BasalTest1MaanedProportionalHalv()
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
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("14-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 2.26M;
            decimal forventetPris = 226;


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

        [TestMethod]
        public void BasalTest1MaanedFull()
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
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 500;


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

        [TestMethod]
        public void BasalTest2MaanederFull()
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
            DateTime abonnentStart = DateTime.Parse("29-05-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("28-10-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "TwoMonths";
            string abonnementOpkraevning = "Full";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 3 * vareAntal;
            decimal forventetPris = 3 * vareAntal * vareSalgsPris;


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

        [TestMethod]
        public void RabatTest1MaanedFull()
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
            decimal? abonnentRabat = 25;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 375;


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

        [TestMethod]
        public void SpecialPrisTest1MaanedFull()
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = 300;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 1500;


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

        [TestMethod]
        public void Index2Test1MaanedFull()
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = 2;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 250;


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

        [TestMethod]
        public void Index15Test1MaanedFull()
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = 15;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 33.35M;


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

        [TestMethod]
        public void BrugerIndex2Test1MaanedFull()
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 2;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 1000;


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

        [TestMethod]
        public void BrugerIndex15Test1MaanedFull()
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 15;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Parse("01-01-2014 00:00:00");
            DateTime abonnentSlut = DateTime.Parse("31-01-2014 00:00:00");
            DateTime? abonnentUdloeb = null;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
            bool abonnementKalenderAar = false;

            List<Abonnement> abonnementer = Vaerktoejer.GenererTestAbonnement(vareNummer, vareSalgsPris, vareAntal, debitorNummer, abonnentPrisIndex, abonnentSpecialPris, abonnentRabat, abonnentRabatUdloeb, abonnentStart, abonnentSlut, abonnentUdloeb, abonnementInterval, abonnementOpkraevning, abonnementKalenderAar);

            decimal forventetAntal = 5;
            decimal forventetPris = 7500;


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
*/