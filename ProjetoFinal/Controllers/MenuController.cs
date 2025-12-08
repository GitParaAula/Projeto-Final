using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Menu()
        {
            return View();
        }
    }
}
