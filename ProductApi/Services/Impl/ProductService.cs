using Microsoft.EntityFrameworkCore;
using ProductApi.ValueObjects;
using ProductApi.Repository;
using ProductApi.Model;
using LoggerModels;
using AutoMapper;

namespace ProductApi.Services
{

    public class ProductService(
        IAppLogger _logger,
        Context _context, 
        IMapper _mapper) : IProductService
    {

        public async Task<IEnumerable<ProductVO>> GetAll()
        {
            var data = await _context.Set<Product>()
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductVO>>(data);
        }

        public async Task<IEnumerable<ProductVO>> GetByIdList(IEnumerable<int> idList)
        {
            var data = await _context.Set<Product>()
                .AsNoTracking()
                .Where(x => idList.Contains(x.Id ?? 0))
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductVO>>(data);
        }

        public async Task<ProductVO?> GetById(int id)
        {
            _logger.Debug($"ProductRepository.GetById: Id={id}");

            var product = await _context.Set<Product>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            return product != null
                ? _mapper.Map<ProductVO>(product)
                : null;
        }

        public async Task<ProductVO> Create(ProductVO data)
        {
            var product = _mapper.Map<Product>(data);

            await _context.Set<Product>().AddAsync(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO?> Update(ProductVO data)
        {
            var newData = await _context.Set<Product>().FindAsync(data.Id);

            if (newData == null)
                return null;

            _mapper.Map(data, newData);
            _context.Set<Product>().Update(newData);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(newData);
        }

        public async Task Delete(int id)
        {
            var data = await _context.Set<Product>().FindAsync(id);

            if (data == null)
                return;

            _context.Set<Product>().Remove(data);
            await _context.SaveChangesAsync();
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
