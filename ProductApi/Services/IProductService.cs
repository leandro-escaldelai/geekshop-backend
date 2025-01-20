using ProductApi.ValueObjects;

namespace ProductApi.Services
{

    public interface IProductService
    {

        Task<IEnumerable<ProductVO>> GetAll();

        Task<IEnumerable<ProductVO>> GetByIdList(IEnumerable<int> idList);

        Task<ProductVO?> GetById(int id);
        
        Task<ProductVO> Create(ProductVO product);
        
        Task<ProductVO?> Update(ProductVO product);
        
        Task Delete(int id);

    }

}
