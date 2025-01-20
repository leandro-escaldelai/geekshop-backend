using OrderApi.Model;
using AutoMapper;

namespace OrderApi.ValueObjects
{

    public class OrderVO
    {

        public int? Id { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? UserId { get; set; }

        public decimal? Total { get; set; }

        public decimal? Discount { get; set; }

        public List<OrderItemVO> Items { get; set; } = new List<OrderItemVO>();


    }


    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<Order, OrderVO>()
                .ReverseMap();
        }

    }

}
