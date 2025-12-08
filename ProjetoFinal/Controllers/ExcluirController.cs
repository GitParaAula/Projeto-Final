using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class ExcluirController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Excluir()
        {
            return View();
        }
    }
}
