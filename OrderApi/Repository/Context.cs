using Microsoft.EntityFrameworkCore;
using OrderApi.Model;

namespace OrderApi.Repository
{

    public class Context(
        DbContextOptions<Context> options,
        IConfiguration configuration) : DbContext(options)
    {

        private readonly string _connectionString = configuration.GetConnectionString("database") 
            ?? throw new ArgumentException("database");

        private readonly string schema = "Order";



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureOrder(modelBuilder);
            ConfigureOrderItem(modelBuilder);
        }

        private void ConfigureOrder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable(nameof(Order), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureOrderItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.ToTable(nameof(OrderItem), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();

                e.HasOne(e => e.Order)
                    .WithMany(e => e.Items);
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
