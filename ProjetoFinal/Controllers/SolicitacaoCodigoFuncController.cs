using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Controllers
{
    public class SolicitacaoCodigoFuncController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SolicitacaoCodigoFunc()
        {
            return View();
        }
    }
}
