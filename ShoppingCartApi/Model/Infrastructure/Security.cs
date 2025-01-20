using Microsoft.AspNetCore.Authorization;

namespace ShoppingCartApi.Model
{

    public class Roles
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }

    public class Scopes
    {

        public class Header
        {

            public const string Get = "shoppingcart:header:get";

            public const string Create = "shoppingcart:header:create";

            public const string Update = "shoppingcart:header:update";

            public const string Delete = "shoppingcart:header:delete";

        }

        public class Detail
        {

            public const string Get = "shoppingcart:detail:get";

            public const string Create = "shoppingcart:detail:create";

            public const string Update = "shoppingcart:detail:update";

            public const string Delete = "shoppingcart:detail:delete";

        }

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

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Admin, policy => policy.RequireClaim("role", Roles.Admin));
                options.AddPolicy(Policies.Customer, policy => policy.RequireClaim("role", Roles.Customer, Roles.Admin));

                options.AddPolicy("GetHeaderScope", policy => policy.RequireClaim("scope", Scopes.Header.Get));
                options.AddPolicy("CreateHeaderScope", policy => policy.RequireClaim("scope", Scopes.Header.Create));
                options.AddPolicy("UpdateHeaderScope", policy => policy.RequireClaim("scope", Scopes.Header.Update));
                options.AddPolicy("DeleteHeaderScope", policy => policy.RequireClaim("scope", Scopes.Header.Delete));

                options.AddPolicy("GetDetailScope", policy => policy.RequireClaim("scope", Scopes.Detail.Get));
                options.AddPolicy("CreateDetailScope", policy => policy.RequireClaim("scope", Scopes.Detail.Create));
                options.AddPolicy("UpdateDetailScope", policy => policy.RequireClaim("scope", Scopes.Detail.Update));
                options.AddPolicy("DeleteDetailScope", policy => policy.RequireClaim("scope", Scopes.Detail.Delete));
            });
        }

    }

}
