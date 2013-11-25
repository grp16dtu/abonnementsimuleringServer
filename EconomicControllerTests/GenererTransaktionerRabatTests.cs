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
    public class GenererTransaktionerRabatTests
    {
        [TestMethod]
        public void GenTransRabatNeg50Test()
        {
            decimal? abonnentRabat = -50;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M-(decimal)abonnentRabat/100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabatNeg001Test()
        {
            decimal? abonnentRabat = -0.01M;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabat0Test()
        {
            decimal? abonnentRabat = 0;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabat001Test()
        {
            decimal? abonnentRabat = 0.01M;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabat50Test()
        {
            decimal? abonnentRabat = 50;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabat9999Test()
        {
            decimal? abonnentRabat = 99.99M;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        [TestMethod]
        public void GenTransRabat100Test()
        {
            decimal? abonnentRabat = 100;
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * (1M - (decimal)abonnentRabat / 100M);

            DebitorindexTest(forventetAntal, forventetPris, abonnentRabat);
        }

        private void DebitorindexTest(decimal forventetAntal, decimal forventetPris, decimal? abonnentRabat)
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal abonnentPrisIndex = 1;
            decimal? abonnentSpecialPris = null;
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
