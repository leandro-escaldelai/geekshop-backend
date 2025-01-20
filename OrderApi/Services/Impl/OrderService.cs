using OrderApi.ValueObjects;
using OrderApi.Repository;
using SecurityModels;
using OrderApi.Model;
using MessageBus;

namespace OrderApi.Services
{

    public class OrderService(
        IPublisher _messageBus,
        ISecurityContext _security,
        ICouponService _couponService,
        IShoppingCartService _cartService,
        Context _context) : IOrderService
    {

        public async Task Create()
        {
            var cart = await _cartService.Get();

            if (cart == null)
                throw new ArgumentException("Invalid cart");

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = _security.UserId,
                Discount = await GetDiscount(cart),
                Total = GetTotal(cart),
                Items = cart.Details.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price

                }).ToList()
            };

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            await _cartService.Clear();
            await _messageBus.Publish(new Message<int?>(order.Id), "created_order");
        }



        private async Task<decimal> GetDiscount(CartVO cart)
        {
            var code = cart.Header?.CouponCode;

            if (string.IsNullOrEmpty(code))
                return 0;

            var coupon = await _couponService.Get(code);

            if (coupon == null)
                return 0;

            var discountFactor = (1 - (coupon.DiscountAmount / 100)) ?? 1;
            var total = GetTotal(cart);

            return total * discountFactor;
        }

        private decimal GetTotal(CartVO cart)
        {
            return cart.Details.Sum(x => x.Quantity * x.Price) ?? 0;
        }

    }



    public static class OrderServiceExtensions
    {

        public static IServiceCollection AddOrderService(this IServiceCollection services)
        {
            return services.AddScoped<IOrderService, OrderService>();
        }

    }

}