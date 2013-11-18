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

        public override bool Equals(Object obj)
        {
            Transaktion t = (Transaktion)obj;
            if (t == null)
                return false;
            else
                return AarMaaned.Equals(t.AarMaaned)
                    && Debitornummer.Equals(t.Debitornummer)
                    && Varenummer.Equals(t.Varenummer)
                    && Afdelingsnummer.Equals(t.Afdelingsnummer)
                    && Antal.Equals(t.Antal)
                    && Beloeb.Equals(t.Beloeb);
        }

        public override int GetHashCode()
        {
            return AarMaaned.GetHashCode() + Debitornummer.GetHashCode() + Varenummer.GetHashCode() + Beloeb.GetHashCode();
        }
    }
}
