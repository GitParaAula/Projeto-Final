using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroPlanoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AvisoCadastroPlano()
        {
            return View();
        }
    }
}
