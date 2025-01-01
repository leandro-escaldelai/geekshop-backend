using Microsoft.EntityFrameworkCore;
using ProductApi.ValueObjects;
using ProductApi.Services;
using ProductApi.Model;
using AutoMapper;

namespace ProductApi.Repository
{

    public class ProductRepository(
        IAppLogger _logger,
        Context _context, 
        IMapper _mapper) : IProductRepository
    {

        public async Task<IEnumerable<ProductVO>> GetAll()
        {
            var data = await _context.Products
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductVO>>(data);
        }

        public async Task<ProductVO?> GetById(int id)
        {
            _logger.Debug($"ProductRepository.GetById: Id={id}");

            var product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return product != null
                ? _mapper.Map<ProductVO>(product)
                : null;
        }

        public async Task<ProductVO> Create(ProductVO data)
        {
            var product = _mapper.Map<Product>(data);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO?> Update(ProductVO data)
        {
            var newData = await _context.Products.FindAsync(data.Id);

            if (newData == null)
                return null;

            _mapper.Map(data, newData);
            _context.Products.Update(newData);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(newData);
        }

        public async Task Delete(int id)
        {
            var data = await _context.Products.FindAsync(id);

            if (data == null)
                return;

            _context.Products.Remove(data);
            await _context.SaveChangesAsync();
        }

    }


    public static class ProductRepositoryExtensions
    {

        public static IServiceCollection AddProductRepository(this IServiceCollection services)
        {
            return services
                .AddScoped<IProductRepository, ProductRepository>();
        }

    }

}
