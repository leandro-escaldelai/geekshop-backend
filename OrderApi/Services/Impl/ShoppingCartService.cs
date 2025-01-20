using OrderApi.ValueObjects;
using SecurityModels;
using ClientModels;

namespace OrderApi.Services
{

    public class ShoppingCartService(
        ISecurityContext _security,
        IConfiguration _configuration) : IShoppingCartService
    {

        private readonly string baseUrl = _configuration["Services:ShoppingCart"] ?? throw new ArgumentNullException("ServicesShoppingCart");


        public async Task Clear()
        {
            using (var client = new HttpClient())
                await client.Delete($"{baseUrl}/shopping-cart/item/all", _security.Token);
        }

        public async Task<CartVO?> Get()
        {
            using (var client = new HttpClient())
                return await client.Get<CartVO>($"{baseUrl}/shopping-cart", _security.Token);
        }

    }


    public static class ShoppingCartServiceExtensions
    {

        public static IServiceCollection AddShoppingCartService(this IServiceCollection services)
        {
            return services
                .AddScoped<IShoppingCartService, ShoppingCartService>();
        }

    }

}
