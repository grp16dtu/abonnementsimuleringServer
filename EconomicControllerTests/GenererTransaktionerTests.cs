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
        public void GenerelTest()
        {
            // arrange
            EconomicController ec = new EconomicController(387892, "DTU", "Trustno1");

            int simuleringsMaaneder = 12;
            decimal brugerIndex = 1;

            List<Abonnement> abonnementer = new List<Abonnement>();
            
            Vare produkt = new Vare(42, "Gummiand", "4", 100, 0, null);
            Varelinje varelinje = new Varelinje(3, 1, "Gummiand", 5, null, produkt, null);
            Debitor debitor = new Debitor("Vinkelvej 10", 645112, "56956956", "Roskilde", null, null, null, "kiaer@innologic.dk", "Jensen og Co.", "1", "4000", null);
            Abonnent abonnent = new Abonnent(12, debitor, 25, null, DateTime.Parse("28-10-2014 00:00:00"), null, null, null, DateTime.Parse("28-10-2013 00:00:00"), null, DateTime.Parse("29-05-2014 00:00:00"));
            Abonnement abonnement = new Abonnement(3, "Bummelum", 3, false, "FourWeeks", "Full");
            abonnement.Varelinjer.Add(varelinje);
            abonnement.Abonnenter.Add(abonnent);
            abonnementer.Add(abonnement);
            // TODO: Indsæt flere abonnementer...

            List<Transaktion> forventet = new List<Transaktion>();
            Transaktion transaktion = new Transaktion(DateTime.Parse("01.05.2014 00:00:00"), "1", "4", 5, 375, null);
            forventet.Add(transaktion);
            // TODO: Indsæt flere transaktioner...


            // act
            List<Transaktion> faktisk = ec.GenererTransaktioner(abonnementer, simuleringsMaaneder, brugerIndex);


            // assert
            List<Transaktion> forskelForventetFaktisk = forventet.Except(faktisk).ToList();
            List<Transaktion> forskelFaktiskForventet = faktisk.Except(forventet).ToList();
            if(forskelFaktiskForventet.Count > 0 || forskelForventetFaktisk.Count > 0)
                Assert.Fail("Faktisk resultat afviger fra forventet med " + (forskelFaktiskForventet.Count + forskelForventetFaktisk.Count) + " transaktioner.");
        }
    }
}
