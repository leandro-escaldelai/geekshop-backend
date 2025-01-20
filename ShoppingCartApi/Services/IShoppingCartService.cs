using ShoppingCartApi.ValueObjects;

namespace ShoppingCartApi.Services
{

    public interface IShoppingCartService
    {

        Task<CartVO?> GetUserCart();

        Task<int> CountCartDetails();

        Task<CartVO> CreateOrUpdateCart(CartVO cartHeader);

        Task<CartDetailVO?> AddToCart(int productId);

        Task RemoveFromCart(int cartDetailsId);

        Task<CouponVO?> ApplyCoupon(string? code);

        Task RemoveCoupon();

        Task ClearCart();

    }

}
