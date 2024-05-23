using IceCreamShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Dynamic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<IceCreamShopContext>(options => {
    options.UseNpgsql("Host=localhost;Port=5432;Database=IceCreamShop;Username=postgres;Password=sabinamini04");
});
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}");
});
app.Run();


//var env = app.Environment;
//if (env.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}