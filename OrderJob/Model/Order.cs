namespace OrderJob.Model
{

    public class Order
    {

        public int? Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? UserId { get; set; }
        
        public decimal? Total { get; set; }
        
        public decimal? Discount { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }

}
