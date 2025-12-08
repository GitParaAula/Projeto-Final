using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class EditarFuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _repo;

        public EditarFuncionarioController(FuncionarioRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var funcionario = _repo.BuscarPorId(id);
            return View(funcionario);
        }

        [HttpPost]
        public IActionResult Editar(Funcionario f)
        {
            _repo.EditarFuncionario(f);
            return RedirectToAction("ConsultaFuncionario", "ConsultaFuncionario");
        }
    }
}
