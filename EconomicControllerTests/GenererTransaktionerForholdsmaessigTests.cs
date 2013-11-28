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
    public class GenererTransaktionerForholdsmaessigTests
    {
        [TestMethod]
        public void GenTransForholdsmaessigHalvMndTest()
        {
            decimal forventetAntal = 5M * 16M / 31M;
            forventetAntal = decimal.Round(forventetAntal, 2);
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = NaesteFoersteJuli();
            DateTime abonnentUdloeb = NaesteFoersteJuli().AddDays(15);

            ForholdsmaessigTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransForholdsmaessigHelMndTest()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = NaesteFoersteJuli();
            DateTime abonnentUdloeb = NaesteFoersteJuli().AddDays(30);

            ForholdsmaessigTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransForholdsmaessigHalvandenMndTest()
        {
            decimal forventetAntal = (5M * 16M / 31M) + 5M;
            forventetAntal = decimal.Round(forventetAntal, 2);
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = NaesteFoersteJuli();
            DateTime abonnentUdloeb = NaesteFoersteJuli().AddDays(30).AddDays(16);

            ForholdsmaessigTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }
   
        private void ForholdsmaessigTest(decimal forventetAntal, decimal forventetPris, DateTime abonnentStart, DateTime abonnentUdloeb)
        {
            // arrange
            int simuleringsMaaneder = 14;
            decimal brugerIndex = 1;

            string vareNummer = "10";
            decimal vareSalgsPris = 100;
            decimal vareAntal = 5;

            string debitorNummer = "1";

            decimal? abonnentPrisIndex = null;
            decimal? abonnentSpecialPris = null;
            decimal? abonnentRabat = null;
            DateTime? abonnentRabatUdloeb = null;
            DateTime abonnentSlut = DateTime.MaxValue;

            string abonnementInterval = "Month";
            string abonnementOpkraevning = "Proportional";
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

        private DateTime NaesteFoersteJuli()
        {
            DateTime nu = DateTime.Now;
            int aar = nu.Year;

            if (nu.Month >= 7)
            {
                aar++;
            }

            return new DateTime(aar, 7, 1);
        }
    }
}
