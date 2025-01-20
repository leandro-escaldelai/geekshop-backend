using ShoppingCartApi.Model;
using AutoMapper;

namespace ShoppingCartApi.ValueObjects
{

    public class CartHeaderVO
    {

        public int? Id { get; set; }

        public int? UserId { get; set; }

        public string? CouponCode { get; set; }


    }



    public class CartHeaderProfile : Profile
    {
        public CartHeaderProfile()
        {
            CreateMap<CartHeader, CartHeaderVO>()
                .ReverseMap();
        }
    }

}
