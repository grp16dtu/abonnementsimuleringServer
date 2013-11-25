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
    public class GenererTransaktionerSpecPrisTests
    {
        [TestMethod]
        public void GenTransSpecPrisNeg50Test()
        {
            decimal abonnentSpecialPris = -50;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPrisNeg001Test()
        {
            decimal abonnentSpecialPris = -0.01M;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPris0Test()
        {
            decimal abonnentSpecialPris = 0;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPris001Test()
        {
            decimal abonnentSpecialPris = 0.01M;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPris357Test()
        {
            decimal abonnentSpecialPris = 3.57M;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPris50Test()
        {
            decimal abonnentSpecialPris = 50;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        [TestMethod]
        public void GenTransSpecPris999999999999999Test()
        {
            decimal abonnentSpecialPris = 999999999999999;
            decimal forventetAntal = 5;
            decimal forventetPris = abonnentSpecialPris * forventetAntal;

            DebitorindexTest(forventetAntal, forventetPris, abonnentSpecialPris);
        }

        private void DebitorindexTest(decimal forventetAntal, decimal forventetPris, decimal abonnentSpecialPris)
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal abonnentPrisIndex = 1;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentUdloeb = abonnentStart.AddMonths(1);
            DateTime abonnentSlut = DateTime.MaxValue;

            string abonnementInterval = "Month";
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
