namespace ShoppingCartApi.Model
{

    public class CartDetail
    {

        public int? Id { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public CartHeader? CartHeader { get; set; }

    }

}
