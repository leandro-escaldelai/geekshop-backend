namespace OrderApi.ValueObjects
{

    public class CartDetailVO
    {

        public int? Id { get; set; }

        public int? CartHeaderId { get; set; }

        public int? ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductImage { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

    }

}
