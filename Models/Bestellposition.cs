namespace Restaurant.Models
{
    public class Bestellposition
    {
        public int ID_Bestellposition { get; set; }
        public int ID_Artikel { get; set; }
        public int ID_Bestellung { get; set; }
        public int Bezahlt { get; set; }
        public int Geliefert { get; set; }
        public Artikel Artikel { get; set; }
    }
}
