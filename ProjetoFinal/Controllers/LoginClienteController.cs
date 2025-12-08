using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class LoginClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginCliente()
        {
            return View();
        }
    }
}
