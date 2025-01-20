using CouponApi.Repository;
using CouponApi.Services;
using CouponApi.Model;
using SecurityModels;

namespace CouponApi;

public class Startup
{

	private readonly IConfiguration configuration;
	private readonly string corsConfig = "AllowAllOrigins";

	public Startup(IConfiguration configuration)
	{
		this.configuration = configuration;
	}



	public void ConfigureServices(IServiceCollection services)
	{
		ConfigureCors(services);
		services.AddAutoMapper(
			AppDomain.CurrentDomain.GetAssemblies());
		services.ConfigureAuthentication(configuration);
		services.ConfigureAuthorization();
		services.AddRepositoryContext();
		services.AddCouponService();
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseCors(corsConfig);
		app.UseRouting();
		app.UseAuthentication();
		app.UseAuthorization();
		ConfigureSwagger(app);
		app.UseEndpoints(e => e.MapControllers());
	}



	private void ConfigureCors(IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy(corsConfig, config =>
				 config.SetIsOriginAllowed(origin => true)
					   .AllowAnyMethod()
					   .AllowAnyHeader()
					   .AllowCredentials()
			);
		});
	}

	private void ConfigureSwagger(IApplicationBuilder app)
	{
		if (configuration["UseSwagger"] != "true")
			return;

		app.UseSwagger();
		app.UseSwaggerUI();
	}

}
