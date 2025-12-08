using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ExcluirFuncionarioController : Controller
    {
        private readonly ExcluirFuncionarioRepositorio _repo;

        public ExcluirFuncionarioController(IConfiguration config)
        {
            _repo = new ExcluirFuncionarioRepositorio(config);
        }

        // LISTAGEM
        [HttpGet]
        public IActionResult ExcluirFuncionario(string nome = "")
        {
            var lista = _repo.Listar(nome);
            return View(lista);
        }

        // EXCLUIR
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            _repo.Excluir(id);
            return RedirectToAction("ExcluirFuncionario");
        }
    }
}