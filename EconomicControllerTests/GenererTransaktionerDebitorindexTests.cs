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
    public class GenererTransaktionerDebitorindexTests
    {
        [TestMethod]
        public void GenTransDebitorindexNeg1Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 / -1;

            decimal debitorIndex = -1;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        [TestMethod]
        public void GenTransDebitorindexNeg001Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 / -0.01M;
            forventetPris = decimal.Round(forventetPris, 2);

            decimal debitorIndex = -0.01M;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        [TestMethod]
        public void GenTransDebitorindex0Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100;

            decimal debitorIndex = 0;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        [TestMethod]
        public void GenTransDebitorindex001Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 / 0.01M;
            forventetPris = decimal.Round(forventetPris, 2);

            decimal debitorIndex = 0.01M;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        [TestMethod]
        public void GenTransDebitorindex357Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = 100M / 3.57M;
            forventetPris = decimal.Round(forventetPris, 2);
            forventetPris = forventetPris * forventetAntal;

            decimal debitorIndex = 3.57M;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        [TestMethod]
        public void GenTransDebitorindex999999999999999Test()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100 / 999999999999999;
            forventetPris = decimal.Round(forventetPris, 2);

            decimal debitorIndex = 9999999999999;

            DebitorindexTest(forventetAntal, forventetPris, debitorIndex);
        }

        private void DebitorindexTest(decimal forventetAntal, decimal forventetPris, decimal abonnentPrisIndex)
        {
            // arrange
            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

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
