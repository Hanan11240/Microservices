using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Core.Mappers;
using eCommerce.Infrastructure;
using FluentValidation.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


//Infrastructure Services
builder.Services.AddInfrastructure();
//Core Service
builder.Services.AddCore();

// add the controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//Auto Mapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(ApplicationUserMappingProfile).Assembly);
});

//Fluent validation
builder.Services.AddFluentValidationAutoValidation();


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
