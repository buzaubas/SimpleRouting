using Microsoft.AspNetCore.Mvc;

namespace SimpleRouting.Controllers
{
    public class Invoices : Controller
    {
        public IActionResult View(string number)
        {
            ViewBag.Number = number;    
            return View();
        }
    }
}
