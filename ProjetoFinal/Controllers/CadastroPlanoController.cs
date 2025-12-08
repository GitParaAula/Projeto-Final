using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroPlanoController : Controller
    {
        private readonly CadastroPlanoRepositorio _repo;

        public CadastroPlanoController(CadastroPlanoRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult CadastroPlano()
        {
            return View(new Plano());
        }

        [HttpPost]
        public IActionResult Salvar(Plano plano)
        {
            _repo.CadastrarPlano(plano);
            return RedirectToAction("AvisoCadastroPlano", "AvisoCadastroPlano");
        }
    }
}
