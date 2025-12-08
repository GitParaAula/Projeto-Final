using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class CadastrarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
    }
}
