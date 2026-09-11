using BusinessLogicLayer;
using DataAccessLayer;
using eCommerce.ProductsService.API.APIEndPoints;
using FluentValidation.AspNetCore;
using ProductsService.API.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLayer();



builder.Services.AddControllers();



builder.Services.AddFluentValidationAutoValidation();


// add model binder to read values from json to enum
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
}); 
var app = builder.Build();



app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();



//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapProductAPIEndPoints();


app.Run();
