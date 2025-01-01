using ProductApi.Model;
using AutoMapper;

namespace ProductApi.ValueObjects
{

    public class ProductVO
    {

        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Category { get; set; }

        public string? ImageUrl { get; set; }

        public decimal? Price { get; set; }

    }


    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductVO, Product>().ReverseMap();
        }
    }

}
