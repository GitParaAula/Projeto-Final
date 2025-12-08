using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class AtribuirPlanoController : Controller
    {
        private readonly AtribuirPlanoRepositorio _repo;

        public AtribuirPlanoController(AtribuirPlanoRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult AtribuirPlano()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AtribuirPlano(int codigoPet, int codigoPlano)
        {
            string resultado = _repo.AtribuirPlano(codigoPet, codigoPlano);

            TempData["Mensagem"] = resultado;

            if (resultado.Contains("incorreto"))
            {
                TempData["MensagemClasse"] = "alert-danger";
                return RedirectToAction("AtribuirPlano");
            }
            else
            {
                TempData["MensagemClasse"] = "alert-primary";
                return RedirectToAction("ConfirmacaoAtribuir", "ConfirmacaoAtribuir");
            }

        }
    }
}