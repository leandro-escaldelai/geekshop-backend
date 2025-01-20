namespace ConfigApi.Model
{

	public class ClientConfig
	{

		public int? Id { get; set; }

		public int? ClientId { get; set; }

		public string? Name { get; set; }

		public string? Value { get; set; }

		public bool? Enabled { get; set; }

		public Config? Config { get; set; }

	}

}
