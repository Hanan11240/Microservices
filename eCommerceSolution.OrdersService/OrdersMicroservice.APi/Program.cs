using eCommerce.OrdersMicroservice.BusinessLogicLayer.HttpClients;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.Policies;
using eCommerce.OrdersMicroserviceBusinessLogicLayer;
using eCommerce.OrdersMicroserviceDataAccessLayer;
using eCommerceOrdersMicroservice.API.Middleware;
using FluentValidation.AspNetCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
builder.Services.AddSingleton<IUsersMicroservicePolicies, UsersMicroservicePolicies>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
builder.Services.AddHttpClient<UsersMicroserviceClients>(client =>{
    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
    
})
   .AddPolicyHandler((provider, request) => 
     provider.GetRequiredService<IUsersMicroservicePolicies>().GetRetryPolicy()

  )
   .AddPolicyHandler((provider, request) =>
     provider.GetRequiredService<IUsersMicroservicePolicies>().GetCircuitBreakerPolicy()

  ).AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IUsersMicroservicePolicies>().GetTimeoutPolicy())
   ;


builder.Services.AddHttpClient<ProductsMicroserviceClient>(client => {
    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
}).AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetFallbackPolicy())

   .AddPolicyHandler(
   builder.Services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetBulkheadIsolationPolicy())
  ;

var app = builder.Build();



app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();
app.UseSwagger();




// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
