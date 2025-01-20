using AutoMapper;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.ValueObjects
{

    public class CartVO
    {

        public CartHeaderVO? Header { get; set; }

        public IEnumerable<CartDetailVO> Details { get; set; } = new List<CartDetailVO>();

    }



    public class CartProfile : Profile
    {

        public CartProfile()
        {
            CreateMap<CartHeader, CartVO>()
                .ForMember(d => d.Header, o => o.MapFrom(s => s))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.Details));
        }   

    }

}
