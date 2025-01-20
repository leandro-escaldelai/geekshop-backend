using AutoMapper;
namespace OrderApi.ValueObjects
{

    public class CartHeaderVO
    {

        public int? Id { get; set; }

        public int? UserId { get; set; }

        public string? CouponCode { get; set; }


    }

}
