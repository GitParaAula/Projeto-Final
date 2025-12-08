using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class ConfirmarPagamentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ConfirmarPagamento()
        {
            return View();
        }
    }
}
