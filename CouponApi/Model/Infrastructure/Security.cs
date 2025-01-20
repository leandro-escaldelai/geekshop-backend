using Microsoft.AspNetCore.Authorization;

namespace CouponApi.Model
{

    public class Roles
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }

    public class Scopes
    {

        public const string Get = "coupon:get";

    }

    public class Policies
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }



    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {

        public AdminAuthorizeAttribute()
        {
            Policy = Policies.Admin;
        }

    }

    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {

        public CustomerAuthorizeAttribute() 
        {
            Policy = Policies.Customer;
        }

    }



    public static class SecurityExtensions
    {

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Admin, policy => policy.RequireClaim("role", Roles.Admin));
                options.AddPolicy(Policies.Customer, policy => policy.RequireClaim("role", Roles.Customer, Roles.Admin));

                options.AddPolicy("GetScope", policy => policy.RequireClaim("scope", Scopes.Get));
            });
        }

    }


}
