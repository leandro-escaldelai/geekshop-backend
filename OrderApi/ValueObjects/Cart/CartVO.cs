namespace OrderApi.ValueObjects
{

    public class CartVO
    {

        public CartHeaderVO? Header { get; set; }

        public IEnumerable<CartDetailVO> Details { get; set; } = new List<CartDetailVO>();

    }

}
