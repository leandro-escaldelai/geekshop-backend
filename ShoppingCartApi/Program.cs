using ShoppingCartApi.Repository;
using ShoppingCartApi.Model;
using SecurityModels;
using LoggerModels;
using ShoppingCartApi.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var services = builder.Services;

builder.ConfigureAuthorization();
builder.Services.AddSingleton<IAppLogger, AppLogger>();
//builder.Logging.ConfigureAppLogger();
services.AddAutoMapper(
    AppDomain.CurrentDomain.GetAssemblies());
services.AddRepositoryContext();
services.AddSecurityContext();
services.AddProductService();
services.AddCouponService();
services.AddShoppingCartService();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();