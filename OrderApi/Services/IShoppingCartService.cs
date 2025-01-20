using OrderApi.ValueObjects;

namespace OrderApi.Services
{

    public interface IShoppingCartService
    {

        Task<CartVO?> Get();

        Task Clear();

    }

}
