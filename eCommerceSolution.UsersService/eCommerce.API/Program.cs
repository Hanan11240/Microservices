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

//Api eplorer swagger
builder.Services.AddEndpointsApiExplorer();

// add  swagger generation services
builder.Services.AddSwaggerGen();

// Add cors services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

app.UseSwagger(); //adds endpoint that  can serve the swagger json
app.UseSwaggerUI(); //Adds swagger ui

app.UseCors();


// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();




app.Run();
