using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AvisoCadastro()
        {
            return View();
        }
    }
}
