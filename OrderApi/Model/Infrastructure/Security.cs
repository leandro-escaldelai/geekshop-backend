using Microsoft.AspNetCore.Authorization;

namespace OrderApi.Model
{

    public class Roles
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }

    public class Scopes
    {

        public const string Get = "order:get";

        public const string Create = "order:create";

        public const string Update = "order:update";

        public const string Delete = "order:delete";

        public class Item
        {

            public const string Get = "order:item:get";

            public const string Create = "order:item:create";

            public const string Update = "order:item:update";

            public const string Delete = "order:item:delete";

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

                options.AddPolicy("GetScope", policy => policy.RequireClaim("scope", Scopes.Get));
                options.AddPolicy("CreateScope", policy => policy.RequireClaim("scope", Scopes.Create));
                options.AddPolicy("UpdateScope", policy => policy.RequireClaim("scope", Scopes.Update));
                options.AddPolicy("DeleteScope", policy => policy.RequireClaim("scope", Scopes.Delete));

                options.AddPolicy("GetItemScope", policy => policy.RequireClaim("scope", Scopes.Item.Get));
                options.AddPolicy("CreateItemScope", policy => policy.RequireClaim("scope", Scopes.Item.Create));
                options.AddPolicy("UpdateItemScope", policy => policy.RequireClaim("scope", Scopes.Item.Update));
                options.AddPolicy("DeleteItemScope", policy => policy.RequireClaim("scope", Scopes.Item.Delete));
            });
        }

    }

}
