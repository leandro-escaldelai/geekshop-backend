using ShoppingCartApi.Model;
using AutoMapper;

namespace ShoppingCartApi.ValueObjects
{

    public class CartDetailVO
    {

        public int? Id { get; set; }

        public int? CartHeaderId { get; set; }

        public int? ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductImage { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

    }



    public class CartDetailProfile : Profile
    {
        public CartDetailProfile()
        {
            CreateMap<CartDetail, CartDetailVO>()
                .ForPath(d => d.CartHeaderId, o => o.MapFrom(s => s.CartHeader.Id))
                .ReverseMap();
        }
    }

}
