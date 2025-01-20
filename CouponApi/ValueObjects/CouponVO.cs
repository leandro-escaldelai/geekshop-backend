using CouponApi.Model;
using AutoMapper;

namespace CouponApi.ValueObjects
{

    public class CouponVO
    {

        public int? Id { get; set; }

        public string? Code { get; set; }

        public decimal? DiscountAmount { get; set; }

    }



    public class CouponProfile : Profile
    {

        public CouponProfile()
        {
            CreateMap<Coupon, CouponVO>()
                .ReverseMap();
        }

    }

}
