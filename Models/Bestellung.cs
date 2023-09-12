namespace Restaurant.Models
{
    public class Bestellung
    {
        public int ID_Bestellung { get; set; }
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public List<Artikel> OrderList { get; set; }
    }
}
