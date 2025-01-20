using Microsoft.EntityFrameworkCore;
using ConfigApi.ValueObjects;
using ConfigApi.Repository;
using SecurityModels;
using ConfigApi.Model;
using AutoMapper;

namespace ConfigApi.Services
{

	public class ConfigService(
		ISecurityContext security,
		IMapper mapper,
		Context context) : IConfigService
	{

		public async Task<IEnumerable<ConfigVO>> GetList()
		{
			var data = await context.Set<ClientConfig>()
				.Include(e => e.Config)
				.Where(x => x.ClientId == security.UserId)
				.AsNoTracking()
				.ToListAsync();
			
			return mapper.Map<IEnumerable<ConfigVO>>(data);
		}

	}


	public static class ConfigServiceExtensions
	{

		public static IServiceCollection AddConfigService(this IServiceCollection services)
		{
			return services
				.AddScoped<IConfigService, ConfigService>();
		}

	}


}
