using OrderApi.ValueObjects;
using SecurityModels;
using ClientModels;

namespace OrderApi.Services
{

    public class CouponService(
        ISecurityContext _security,
        IConfiguration _configuration) : ICouponService
    {

        private readonly string baseUrl = _configuration["Services:Coupon"] ?? throw new ArgumentNullException("ServicesCoupon");

        public async Task<CouponVO?> Get(string code)
        {
            using (var client = new HttpClient())
                return await client.Get<CouponVO>($"{baseUrl}/coupon/{code}", _security.Token);
        }

    }


    public static class CouponServiceExtensions
    {

        public static IServiceCollection AddCouponService(this IServiceCollection services)
        {
            return services
                .AddScoped<ICouponService, CouponService>();
        }

    }

}
