using ShoppingCartApi.ValueObjects;

namespace ShoppingCartApi.Services
{

    public interface IProductService
    {

        Task<ProductVO?> GetById(int id);

        Task<IEnumerable<ProductVO>> GetByIdList(IEnumerable<int> idList);

    }

}
