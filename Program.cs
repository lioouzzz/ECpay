using Ecpay;
using Ecpay.Models;
using Ecpay.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder( args );

// 加入 Swagger (●´ω｀●)ゞ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllersWithViews();
ConnectionStrings.Ecpay = builder.Configuration.GetValue<string>( "ConnectionStrings:Ecpay" );

var ecpaySetting = builder.Configuration.GetSection( "EcpaySetting" ).Get<EcpaySettingModel>();

builder.Services.AddSingleton( ecpaySetting );

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<CreditCardService>();


var app = builder.Build();

// 啟用 Swagger
app.UseSwagger();
app.UseSwaggerUI( c =>
{
    c.SwaggerEndpoint( "/swagger/v1/swagger.json" , "My API V1" );
    c.RoutePrefix = "swagger"; // 預設就是 swagger
} );

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment() )
{
    app.UseExceptionHandler( "/Home/Error" );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default" ,
    pattern: "{controller=Home}/{action=Index}/{id?}" );

app.Run();
