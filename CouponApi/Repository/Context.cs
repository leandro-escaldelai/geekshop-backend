using Microsoft.EntityFrameworkCore;
using CouponApi.Model;

namespace CouponApi.Repository
{

    public class Context(
        DbContextOptions<Context> options,
        IConfiguration configuration) : DbContext(options)
    {

        private readonly string _connectionString = configuration.GetConnectionString("database") 
            ?? throw new ArgumentException("database");

        private readonly string schema = "Coupon";



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCoupon(modelBuilder);
        }



        private void ConfigureCoupon(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>(e =>
            {
                e.ToTable(nameof(Coupon), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

    }



    public static class ContextExtensions
    {

        public static IServiceCollection AddRepositoryContext(this IServiceCollection services)
        {
            return services.AddDbContext<Context>(
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped
            );
        }

    }

}
