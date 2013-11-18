using System;
namespace AbonnementsimuleringServer.Models
{
    public class Transaktion
    {
        public DateTime AarMaaned { get; set; }
        public string Debitornummer { get; set; }
        public string Varenummer { get; set; }
        public int? Afdelingsnummer { get; set; }
        public decimal? Antal { get; set; }
        public decimal Beloeb { get; set; }

        public Transaktion(){} // Tom konstruktør nødvendigt for serialisering til WEB API

        public Transaktion(DateTime aarMaaned, string debitornummer, string varenummer, decimal? antal, decimal beloeb, int? afdelingsnummer)
        {
            AarMaaned = new DateTime(aarMaaned.Year, aarMaaned.Month, 1);
            Debitornummer = debitornummer;
            Varenummer = varenummer;
            Antal = antal;
            Beloeb = beloeb;
            Afdelingsnummer = afdelingsnummer;
        }
    }
}
