using ConfigApi.Model;
using AutoMapper;

namespace ConfigApi.ValueObjects
{

	public class ConfigVO
	{

		public string? Name { get; set; }

		public string? Value { get; set; }

	}



	public class ConfigProfile : Profile
	{

		public ConfigProfile()
		{
			CreateMap<ClientConfig, ConfigVO>()
				.ForMember(d => d.Value, o => o.MapFrom(s => GetValue(s)));
		}



		private string? GetValue(ClientConfig data)
		{
			return data.Config?.Value ?? data.Value;
		}

	}

}
