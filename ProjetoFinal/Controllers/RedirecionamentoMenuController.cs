using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class RedirecionamentoMenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RedirecionamentoMenu()
        {
            return View();
        }
    }
}
