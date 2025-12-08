using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class ConfirmacaoAtribuirController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ConfirmacaoAtribuir()
        {
            return View();
        }
    }
}
