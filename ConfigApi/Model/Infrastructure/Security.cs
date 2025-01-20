namespace ConfigApi.Model
{

    public class Scopes
    {

        public const string Get = "config:get";

    }

    public class Policies
    {

        public class Scopes
        {
            public const string Get = "GetScope";
		}

    }



    public static class SecurityExtensions
    {

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.Scopes.Get, policy => policy.RequireClaim("scope", Scopes.Get));
            });
        }

    }


}
