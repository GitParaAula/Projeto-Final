using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class AvisoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Aviso()
        {
            return View();
        }
    }
}
