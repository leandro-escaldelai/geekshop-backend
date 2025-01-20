using CouponApi.ValueObjects;

namespace CouponApi.Services
{

    public interface ICouponService
    {

        Task<CouponVO?> GetByCode(string code);

    }

}
