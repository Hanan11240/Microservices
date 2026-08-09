using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Core.Mappers;
using eCommerce.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//Infrastructure Services
builder.Services.AddInfrastructure();
//Core Service
builder.Services.AddCore();

// add the controllers
builder.Services.AddControllers();

//Auto Mapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(ApplicationUserMappingProfile).Assembly);
});
var app = builder.Build();

// Middleware
app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();


app.Run();
