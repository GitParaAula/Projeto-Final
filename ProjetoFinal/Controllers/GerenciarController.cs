using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class GerenciarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Gerenciar()
        {
            return View();
        }
    }
}
