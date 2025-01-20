using Microsoft.EntityFrameworkCore;
using CouponApi.ValueObjects;
using CouponApi.Repository;
using CouponApi.Model;
using AutoMapper;

namespace CouponApi.Services
{

    public class CouponService(
        IMapper _mapper,
        Context _context) : ICouponService
    {

        public async Task<CouponVO?> GetByCode(string code)
        {
            try
            {
                var coupon = await _context.Set<Coupon>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Code == code);

                return coupon != null
                    ? _mapper.Map<CouponVO>(coupon)
                    : null;
            }
            catch (Exception ex)
            {
                throw;
            }
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
