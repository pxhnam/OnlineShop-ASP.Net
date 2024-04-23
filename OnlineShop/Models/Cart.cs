namespace OnlineShop.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int IDProduct { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        public decimal intoMoney
        {
            get
            {
                return Quantity * Price;
            }
        }
    }
}