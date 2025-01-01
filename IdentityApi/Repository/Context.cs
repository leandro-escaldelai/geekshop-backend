using Microsoft.EntityFrameworkCore;
using IdentityApi.Model;

namespace IdentityApi.Repository
{

    public class Context(
        DbContextOptions<Context> options,
        IConfiguration configuration) : DbContext(options)
    {

        private readonly string _connectionString = configuration.GetConnectionString("database")
            ?? throw new ArgumentException("database");




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureGrantType(modelBuilder);
            ConfigureScope(modelBuilder);
            ConfigureClient(modelBuilder);
            ConfigureClientGrantType(modelBuilder);
            ConfigureClientScope(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureRole(modelBuilder);
            ConfigureUserRole(modelBuilder);
        }



        private void ConfigureGrantType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GrantType>(e =>
            {
                e.HasKey(x => x.Id);
            });
        }

        private void ConfigureScope(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scope>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureClient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureClientGrantType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientGrantType>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasOne(x => x.Client)
                    .WithMany(x => x.GrantTypes);
                e.HasOne(x => x.GrantType);
                    
            });
        }

        private void ConfigureClientScope(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientScope>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasOne(x => x.Client)
                    .WithMany(x => x.Scopes);
                e.HasOne(x => x.Scope);
            });
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }

        private void ConfigureUserRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).ValueGeneratedOnAdd();
                e.HasOne(x => x.Client);
                e.HasOne(x => x.User)
                    .WithMany(x => x.Roles);
                e.HasOne(x => x.Role);
                    
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
