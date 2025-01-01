using ProductApi.ValueObjects;

namespace ProductApi.Repository
{

    public interface IProductRepository
    {

        Task<IEnumerable<ProductVO>> GetAll();

        Task<ProductVO?> GetById(int id);
        
        Task<ProductVO> Create(ProductVO product);
        
        Task<ProductVO?> Update(ProductVO product);
        
        Task Delete(int id);

    }

}
