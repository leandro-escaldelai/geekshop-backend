using System.Net.Http.Headers;

namespace SecurityModels;

public class SecurityContext : ISecurityContext
{

    private readonly AuthenticationHeaderValue? authHeader;
		private readonly HttpContext? context;

		public SecurityContext(IHttpContextAccessor context)
		{
			this.context = context.HttpContext;
		}

		public SecurityContext(AuthenticationHeaderValue authHeader)
		{
			this.authHeader = authHeader;
			context = null;
		}



		public int? UserId => GetUserId();

    public AuthenticationHeaderValue? Token => GetAuthentication();



    private int? GetUserId()
    {
        var claim = context?.User?.Claims?
            .FirstOrDefault(x => x.Type == "nameid");

        if (claim == null)
            return null;

        return int.TryParse(claim.Value, out int value) ? value : null;
    }

    private AuthenticationHeaderValue? GetAuthentication()
    {
        if (authHeader != null)
				return authHeader;

			var authorization = $"{context?.Request.Headers["Authorization"]}";

        if (string.IsNullOrEmpty(authorization))
            return null;

        authorization = authorization
            .Replace("Bearer ", "")
            .Replace("bearer ", "")
            .Replace("BEARER ", "");

        return new AuthenticationHeaderValue("Bearer", authorization);
    }

}


public static class SecurityContextExtensions
{

    public static void AddSecurityContext(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ISecurityContext, SecurityContext>();
    }

}
