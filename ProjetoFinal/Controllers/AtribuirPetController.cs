using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class AtribuirPetController : Controller
    {
        private readonly AtribuirPetRepositorio _repo;

        public AtribuirPetController(AtribuirPetRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult AtribuirPet()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AtribuirPet(int codigoPet, int codigoUsuario)
        {
            string resultado = _repo.AtribuirPet(codigoPet, codigoUsuario);

            TempData["Mensagem"] = resultado;

            // Checar se é mensagem de erro
            if (resultado.Contains("encontrado"))
                TempData["MensagemClasse"] = "alert-danger"; // vermelho
            else
                TempData["MensagemClasse"] = "alert-primary"; // azul (sucesso)

            return RedirectToAction("AtribuirPet");
        }
    }
}