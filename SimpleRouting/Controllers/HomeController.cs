﻿using Microsoft.AspNetCore.Mvc;
using SimpleRouting.Models;
using System.Diagnostics;

namespace SimpleRouting.Controllers
{
    [Route("[controller]")] //если существует контроллер с именем указанным здесь и при этом маршрутизации по умолчанию включены при обращений возникнет ошибка
    [Route("[main]")]
    [Route("[store]")] //может также использоваться для сокрытия полной ссылки
    [Route("~/api")] //без наименования контроллера
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        [Route("[controller]/{int:0}/{name: maxlength(10)}")]
        [Route("", Name="INDEXER")]
        public IActionResult Index()
        {
            //ViewBag.A = a;
            return View();
        }

        [Route("[controller]/[action]")] //маршрутизация с помощью атрибутов имеет приоритет
        public IActionResult Privacy(params string[] obj)
        {
            string controller = RouteData.Values["controller"].ToString();
            string action = RouteData.Values["action"].ToString();

            //return View();
            return Content($"controller: {controller} | action: {action}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}