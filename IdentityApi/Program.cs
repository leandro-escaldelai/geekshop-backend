using IdentityApi.Repository;
using IdentityApi.Services;
using IdentityApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

builder.ConfigureAuthentication();
builder.Logging.ConfigureAppLogger();
services.AddAutoMapper(
    AppDomain.CurrentDomain.GetAssemblies());
services.AddRepositoryContext();
services.AddLoginService();
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




