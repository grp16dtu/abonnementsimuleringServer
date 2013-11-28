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
    public class GenererTransaktionerSlutUdloebTests
    {
        [TestMethod]
        public void GenTransSlut1MndUdloab2MndrTest()
        {
            decimal forventetAntal = 10;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentSlut = abonnentStart.AddMonths(1);
            DateTime abonnentUdloeb = abonnentStart.AddMonths(2);

            SlutUdloebTest(forventetAntal, forventetPris, abonnentStart, abonnentSlut, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransSlut2MndrUdloab1MndTest()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentSlut = abonnentStart.AddMonths(2);
            DateTime abonnentUdloeb = abonnentStart.AddMonths(1);

            SlutUdloebTest(forventetAntal, forventetPris, abonnentStart, abonnentSlut, abonnentUdloeb);
        }
   
        private void SlutUdloebTest(decimal forventetAntal, decimal forventetPris, DateTime abonnentStart, DateTime abonnentSlut, DateTime abonnentUdloeb)
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
