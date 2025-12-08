using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class AtualizarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Atualizar()
        {
            return View();
        }
    }
}
