using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroPetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AvisoCadastroPet()
        {
            return View();
        }
    }
}
