using Microsoft.AspNetCore.Routing.Constraints;
using SimpleRouting.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Documents",
        pattern: "documents/{controller}/{number}/{action}",
        defaults: new
        {
            controller = "invoices",
            action = "view",
            //HttpMethod = new HttpMethodRouteConstraint("GET"),
            customerConstraint = new UserAgentConstraint("Chrome"),
        },
        constraints: new
        {
            number = new RegexRouteConstraint("[a-z]{2}//d{5}")
        }); ;
});

app.MapControllerRoute(
    name: "catchAll", //название
    pattern: "{controller=Home}/{action=Index}/{*data}"); //когда точно не знаем сколько параметров

app.MapControllerRoute(
    name: "twoParameterRoute", //название
    pattern: "{controller=Home}/{action=Index}/{x}/{y}");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Home}/{action=Index}");

//Определение route template
app.MapControllerRoute(
    name: "default",
    //route template для запросов состоящих из двух сегментов
    pattern: "{controller=Home}/{action=Index}/{id?}",
    
    constraints: new
    {
        number = new IntRouteConstraint()
    });

app.Run();
