using Microsoft.AspNetCore.Routing.Constraints;
using SimpleRouting.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MyApp.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCulture = new[]
    {
            new CultureInfo("en"),
            new CultureInfo("ru"),
            new CultureInfo("kk"),
    };
    options.DefaultRequestCulture = new RequestCulture(culture: "kk", uiCulture: "kk");
    options.SupportedCultures = supportedCulture;
    options.SupportedUICultures = supportedCulture;

    //options.AddInitialRequestCultureProvider(new MyCultureProvider()); //через SQL запрос в базу данных

    options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(async context =>
    {
        var currentCulture = "kk";
        var segment = context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (segment.Length >= 1) //2
        {
            string lastSegment = segment[segment.Length - 1];
            currentCulture = lastSegment;
        }

        var requestCulture = new ProviderCultureResult(currentCulture);
        return await Task.FromResult(requestCulture);
    }));
});

builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession(); //ставится перед Routing либо перед StaticFiles

app.UseStaticFiles();

app.UseRouting();

var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(locOptions.Value);

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//        name: "Documents",
//        pattern: "documents/{controller}/{number}/{action}",
//        defaults: new
//        {
//            controller = "invoices",
//            action = "view",
//            //HttpMethod = new HttpMethodRouteConstraint("GET"),
//            customerConstraint = new UserAgentConstraint("Chrome"),
//        },
//        constraints: new
//        {
//            number = new RegexRouteConstraint("[a-z]{2}//d{5}")
//        }); ;
//});

//app.MapControllerRoute(
//    name: "catchAll", //название
//    pattern: "{controller=Home}/{action=Index}/{*data}"); //когда точно не знаем сколько параметров

//app.MapControllerRoute(
//    name: "twoParameterRoute", //название
//    pattern: "{controller=Home}/{action=Index}/{x}/{y}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "api/{controller=Home}/{action=Index}");

//app.MapControllerRoute(
//    name: "defaultApi",
//    pattern: "api/{action}",
//    defaults: new {controller= "Home"} );

//app.MapControllerRoute(
//    name: "default",
//    pattern: "api/{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "localization",
    pattern: "kk", 
    defaults: new { controller = "Home", action = "Index"}); //что бы можно было обращаться localhost:5144/kk-kz

app.MapControllerRoute(
    name: "localization",
    pattern: "ru",
    defaults: new { controller = "Home", action = "Index" }); //что бы можно было обращаться localhost:5144/kk-kz

app.MapControllerRoute(
    name: "localization",
    pattern: "en",
    defaults: new { controller = "Home", action = "Index" }); //что бы можно было обращаться localhost:5144/kk-kz

//Определение route template
app.MapControllerRoute(
    name: "default",
    //route template для запросов состоящих из двух сегментов
    pattern: "{controller=Home}/{action=Index}/{id?}"
    //,

    //constraints: new
    //{
    //    number = new IntRouteConstraint()
    //}
    );

app.Run();
