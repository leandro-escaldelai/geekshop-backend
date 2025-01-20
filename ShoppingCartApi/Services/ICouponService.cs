using ShoppingCartApi.ValueObjects;

namespace ShoppingCartApi.Services
{

    public interface ICouponService
    {

        Task<CouponVO?> GetByCode(string code);

    }


}
