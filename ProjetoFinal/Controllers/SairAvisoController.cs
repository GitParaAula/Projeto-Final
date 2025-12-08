using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class SairAvisoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SairAviso()
        {
            return View();
        }
    }
}
