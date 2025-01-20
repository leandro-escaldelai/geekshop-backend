using ConfigApi.ValueObjects;

namespace ConfigApi.Services
{

	public interface IConfigService
	{

		Task<IEnumerable<ConfigVO>> GetList();

	}

}
