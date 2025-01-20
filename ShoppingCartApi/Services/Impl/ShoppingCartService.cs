using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.ValueObjects;
using ShoppingCartApi.Repository;
using ShoppingCartApi.Model;
using SecurityModels;
using AutoMapper;

namespace ShoppingCartApi.Services
{

    public class ShoppingCartService(
        IProductService _productService,
        ICouponService _couponService,
        ISecurityContext _security,
        IMapper _mapper,
        Context _context) : IShoppingCartService
    {

        public async Task<CartVO?> GetUserCart()
        {
            var cart = await GetOrCreateCart();
            var vo = _mapper.Map<CartVO>(cart);
            var products = await _productService.GetByIdList(
                cart.Details.Select(x => x.ProductId.Value));

            foreach (var detail in vo.Details)
            {
                var product = products.FirstOrDefault(x => x.Id == detail.ProductId);

                detail.ProductName = product?.Name;
                detail.ProductImage = product?.ImageUrl;
            }

            return vo;
        }

        public async Task<int> CountCartDetails()
        {
            return (await _context.Set<CartDetail>()
                .Where(x => x.CartHeader.UserId == _security.UserId)
                .SumAsync(x => x.Quantity)) ?? 0;
        }

        public async Task<CouponVO?> ApplyCoupon(string? code)
        {
            if (string.IsNullOrEmpty(code))
            {
                await RemoveCoupon();
                return null;
            }

            var coupon = await _couponService.GetByCode(code);

            if (coupon == null)
                return null;

            var cart = await GetOrCreateCart();

            cart.CouponCode = coupon.Code;
            await _context.SaveChangesAsync();

            return coupon;
        }

        public async Task<CartDetailVO?> AddToCart(int productId)
        {
            var product = await _productService.GetById(productId);

            if (product == null)
                return null;

            var header = await GetOrCreateCart(false);
            var detail = await _context
                .Set<CartDetail>()
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId &&
                    x.CartHeader.Id == header.Id);

            if (detail == null)
            {
                detail = new CartDetail
                {
                    CartHeader = header,
                    ProductId = productId,
                    Price = product.Price,
                    Quantity = 1
                };
                await _context.AddAsync(detail);
            }
            else
            {
                detail.Quantity++;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<CartDetailVO>(detail);
        }

        public async Task ClearCart()
        {
            if (!await CartExists())
                return;

            var cart = await _context.Set<CartHeader>()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.UserId == _security.UserId);

            _context.RemoveRange(cart.Details);
            await _context.SaveChangesAsync();
        }

        public async Task<CartVO> CreateOrUpdateCart(CartVO cart)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveCoupon()
        {
            if (!await CartExists())
                return;

            var cart = await _context.Set<CartHeader>()
                .FirstAsync(x => x.UserId == _security.UserId);

            cart.CouponCode = null;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCart(int cartDetailsId)
        {
            var detail = await _context.Set<CartDetail>()
                .FirstOrDefaultAsync(x => x.Id == cartDetailsId);

            if (detail == null)
                return;

            if (detail.Quantity == 1)
                _context.Remove(detail);
            else
                detail.Quantity--;

            await _context.SaveChangesAsync();
        }



        private async Task<CartHeader> GetOrCreateCart(bool includeDetails = true)
        {
            var query = _context.Set<CartHeader>()
                .AsQueryable();

            if (includeDetails)
                query = query.Include(x => x.Details);

            var cart = await query
                .FirstOrDefaultAsync(x => x.UserId == _security.UserId);

            if (cart != null)
                return cart;

            cart = new CartHeader { UserId = _security.UserId };

            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        private async Task<bool> CartExists()
        {
            return await _context.Set<CartHeader>()
                .AnyAsync(x => x.UserId == _security.UserId);
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
