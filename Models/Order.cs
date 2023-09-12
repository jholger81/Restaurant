namespace Restaurant.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public int TableNumber { get; set; }
        public int StaffNumber { get; set; }
        public List<Article> OrderList { get; set; }
    }
}
