using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class ConsultarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Consultar()
        {
            return View();
        }
    }
}
