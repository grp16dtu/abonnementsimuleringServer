using AbonnementsimuleringServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomicControllerTests
{
    public class Vaerktoejer
    {
        public static List<Abonnement> GenererTestAbonnement(string vareNummer, decimal vareSalgsPris, decimal vareAntal, string debitorNummer, decimal? abonnentPrisIndex, decimal? abonnentSpecialPris, decimal? abonnentRabat, DateTime? abonnentRabatUdloeb, DateTime abonnentStart, DateTime abonnentSlut, DateTime? abonnentUloeb, string abonnementInterval, string abonnementOpkraevning, bool abonnementKalenderAar)
        {
            List<Abonnement> abonnementer = new List<Abonnement>();

            Vare produkt = new Vare(0, null, vareNummer, vareSalgsPris, 0, null);
            Varelinje varelinje = new Varelinje(0, 0, vareAntal, null, produkt, null);
            Debitor debitor = new Debitor(null, 0, null, null, null, null, null, null, null, debitorNummer, null, null);
            Abonnent abonnent = new Abonnent(0, debitor, abonnentRabat, abonnentRabatUdloeb, abonnentSlut, abonnentUloeb, null, abonnentPrisIndex, DateTime.MinValue, abonnentSpecialPris, abonnentStart);
            Abonnement abonnement = new Abonnement(0, null, 0, abonnementKalenderAar, abonnementInterval, abonnementOpkraevning);
            abonnement.Varelinjer.Add(varelinje);
            abonnement.Abonnenter.Add(abonnent);
            abonnementer.Add(abonnement);

            return abonnementer;
        }
    }
}
