using OrderApi.Model;
using AutoMapper;

namespace OrderApi.ValueObjects
{

    public class OrderItemVO
    {

        public int? Id { get; set; }
        
        public int? ProductId { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

    }


    public class OrderItemProfile : Profile
    {

        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemVO>()
                .ReverseMap()
                .ForMember(d => d.Order, o => o.Ignore());
        }

    }

}
