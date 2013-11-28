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
    public class GenererTransaktionerBrugerindexTests
    {
        [TestMethod]
        public void GenTransBrugerindexNeg1Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * -1;

            decimal brugerIndex = -1;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        [TestMethod]
        public void GenTransBrugerindexNeg001Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * -0.01M;

            decimal brugerIndex = -0.01M;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        [TestMethod]
        public void GenTransBrugerindex0Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * 0;

            decimal brugerIndex = 0;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        [TestMethod]
        public void GenTransBrugerindex001Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * 0.01M;

            decimal brugerIndex = 0.01M;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        [TestMethod]
        public void GenTransBrugerindex357Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * 3.57M;

            decimal brugerIndex = 3.57M;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        [TestMethod]
        public void GenTransBrugerindex9999999999999Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 * 9999999999999;

            decimal brugerIndex = 9999999999999;

            BrugerindexTest(forventetAntal, forventetPris, brugerIndex);
        }

        private void BrugerindexTest(decimal forventetAntal, decimal forventetPris, decimal brugerIndex)
        {
            // arrange
            int simuleringsMaaneder = 12;
            

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = null;
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
