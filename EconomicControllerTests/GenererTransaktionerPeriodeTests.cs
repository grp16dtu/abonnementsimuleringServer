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
    public class GenererTransaktionerPeriodeTests
    {
        [TestMethod]
        public void GenTransPeriode1MndFraNuTest()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentUdloeb = abonnentStart.AddMonths(1);

            PeriodeTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransPeriode1MndFraOm4MndrTest()
        {
            decimal forventetAntal = 5;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now.AddMonths(4);
            DateTime abonnentUdloeb = abonnentStart.AddMonths(1);

            PeriodeTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransPeriode12MndrFraNuTest()
        {
            decimal forventetAntal = 5 * 12;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now;
            DateTime abonnentUdloeb = abonnentStart.AddMonths(12);

            PeriodeTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }

        [TestMethod]
        public void GenTransPeriode12MndFraOm4MndrTest()
        {
            decimal forventetAntal = 5 * 8;
            decimal forventetPris = forventetAntal * 100;

            DateTime abonnentStart = DateTime.Now.AddMonths(4);
            DateTime abonnentUdloeb = abonnentStart.AddMonths(12);

            PeriodeTest(forventetAntal, forventetPris, abonnentStart, abonnentUdloeb);
        }
   
        private void PeriodeTest(decimal forventetAntal, decimal forventetPris, DateTime abonnentStart, DateTime abonnentUdloeb)
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
