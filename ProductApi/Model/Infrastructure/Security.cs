using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Model
{

    public class Roles
    {

        public const string Admin = "admin";

        public const string Customer = "customer";

    }

    public class Scopes
    {

        public const string Get = "product:get";

        public const string Create = "product:create";

        public const string Update = "product:update";

        public const string Delete = "product:delete";

    }

    public class Policies
    {

        public class Role
        {
            public const string Admin = "admin";

            public const string Customer = "customer";
        }

        public class Scope
		{
			public const string Get = "GetScope";

			public const string Create = "CreateScope";

			public const string Update = "UpdateScope";

			public const string Delete = "DeleteScope";
		}




	}



    public static class SecurityExtensions
    {

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Role.Admin, policy => policy.RequireClaim("role", Roles.Admin));
                options.AddPolicy(Policies.Role.Customer, policy => policy.RequireClaim("role", Roles.Customer, Roles.Admin));

                options.AddPolicy(Policies.Scope.Get, policy => policy.RequireClaim("scope", Scopes.Get));
                options.AddPolicy(Policies.Scope.Create, policy => policy.RequireClaim("scope", Scopes.Create));
                options.AddPolicy(Policies.Scope.Update, policy => policy.RequireClaim("scope", Scopes.Update));
                options.AddPolicy(Policies.Scope.Delete, policy => policy.RequireClaim("scope", Scopes.Delete));
            });
        }

    }


}
