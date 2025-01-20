using Microsoft.EntityFrameworkCore;
using ConfigApi.Model;

namespace ConfigApi.Repository
{

    public class Context(
        DbContextOptions<Context> options,
        IConfiguration configuration) : DbContext(options)
    {

        private readonly string _connectionString = configuration.GetConnectionString("database") 
            ?? throw new ArgumentException("database");

        private readonly string schema = "config";



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureConfig(modelBuilder);
            ConfigureClientConfig(modelBuilder);
        }



        private void ConfigureConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>(e =>
            {
                e.ToTable(nameof(Config), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureClientConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientConfig>(e =>
            {
                e.ToTable(nameof(ClientConfig), schema);
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.Enabled).HasDefaultValue(true);
                e.HasOne(e => e.Config);
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
