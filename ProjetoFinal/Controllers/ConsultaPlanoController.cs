using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ConsultaPlanoController : Controller
    {
        private readonly PlanoRepositorio _repo;

        public ConsultaPlanoController(PlanoRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult ConsultaPlano(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                var filtrados = _repo.BuscarPorNome(nome);
                return View(filtrados);
            }

            var todos = _repo.ListarPlanos();
            return View(todos);
        }
    }
}
