namespace AbonnementsimuleringServer.Models
{
    public class Varelinje
    {
        public int Id { get; set; }
        public int Nummer { get; set; }

        public decimal Antal { get; set; }
        public decimal? Saerpris { get; set; }
        public Vare Produkt { get; set; }
        public Afdeling Afdeling { get; set; }

        public Varelinje(int id, int number, decimal quantity, decimal? specialPrice, Vare product, Afdeling afdeling)
        {
            Id = id;
            Nummer = number;
            Antal = quantity;
            Saerpris = specialPrice;
            Produkt = product;
            Afdeling = afdeling;
        }
    }
}
