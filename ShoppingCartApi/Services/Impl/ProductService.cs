using ShoppingCartApi.ValueObjects;
using SecurityModels;
using ClientModels;

namespace ShoppingCartApi.Services
{

    public class ProductService(
        ISecurityContext _security,
        IConfiguration _configuration) : IProductService
    {

        private readonly string baseUrl = _configuration["Services:Product"] ?? throw new ArgumentNullException("ServicesProduct");

        public async Task<ProductVO?> GetById(int id)
        {
            using (var client = new HttpClient())
                return await client.Get<ProductVO>($"{baseUrl}/product/{id}", _security.Token);
        }

        public async Task<IEnumerable<ProductVO>> GetByIdList(IEnumerable<int> idList)
        {
            var query = QueryString.Create(idList.Distinct().Select(id => new KeyValuePair<string, string?>(nameof(idList), $"{id}")));

            using (var client = new HttpClient())
                return await client.Get<IEnumerable<ProductVO>>($"{baseUrl}/product/list" + query, _security.Token);
        }

    }



    public static class ProductServiceExtensions
    {

        public static IServiceCollection AddProductService(this IServiceCollection services)
        {
            return services
                .AddScoped<IProductService, ProductService>();
        }

    }

}
