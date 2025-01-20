namespace ShoppingCartApi.Model
{

    public class CartHeader
    {

        public int? Id { get; set; }

        public int? UserId { get; set; }

        public string? CouponCode { get; set; }

        public List<CartDetail> Details { get; set; } = new List<CartDetail>();  

    }

}
