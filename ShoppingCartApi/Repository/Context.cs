using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Model;

namespace ShoppingCartApi.Repository
{

    public class Context(
        DbContextOptions<Context> options,
        IConfiguration configuration) : DbContext(options)
    {

        private readonly string _connectionString = configuration.GetConnectionString("database") 
            ?? throw new ArgumentException("database");

        private readonly string schema = "ShoppingCart";



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureCartHeader(modelBuilder);
            ConfigureCartDetail(modelBuilder);
        }

        private void ConfigureCartHeader(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartHeader>(e =>
            {
                e.ToTable(nameof(CartHeader), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureCartDetail(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartDetail>(e =>
            {
                e.ToTable(nameof(CartDetail), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();

                e.HasOne(e => e.CartHeader)
                    .WithMany(e => e.Details);
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
