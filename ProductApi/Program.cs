using ProductApi.Repository;
using ProductApi.Services;
using SecurityModels;
using ProductApi.Model;
using LoggerModels;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var services = builder.Services;

builder.ConfigureAuthorization();
builder.Services.AddSingleton<IAppLogger, AppLogger>();
//builder.Logging.ConfigureAppLogger();
services.AddAutoMapper(
    AppDomain.CurrentDomain.GetAssemblies());
services.AddRepositoryContext();
services.AddProductService();
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
