using OrderApi.ValueObjects;

namespace OrderApi.Services
{

    public interface ICouponService
    {

        Task<CouponVO?> Get(string code);

    }

}
