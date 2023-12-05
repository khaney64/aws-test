namespace WebApp
{
	public interface IAppConfiguration
	{
		string WebApiUrl { get; set; } 
		ProviderType Provider { get; set; }
	}

	public class AppConfiguration : IAppConfiguration
	{
		public AppConfiguration() { }

		public AppConfiguration (IConfiguration configuration)
		{
			var config = configuration.GetSection(Section).Get<AppConfiguration>();
			WebApiUrl = config.WebApiUrl;
			Provider = config.Provider;
		}

		public const string Section = "Configuration";
		public ProviderType Provider { get; set; } = ProviderType.MemCache;
		public string WebApiUrl { get; set; } = "http://webapi";
	}
}