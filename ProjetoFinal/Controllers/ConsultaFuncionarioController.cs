using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ConsultaFuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _repo;

        public ConsultaFuncionarioController(FuncionarioRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult ConsultaFuncionario(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                var filtrados = _repo.BuscarPorNome(nome);
                return View(filtrados);
            }

            var todos = _repo.ListarFuncionarios();
            return View(todos);
        }
    }
}
