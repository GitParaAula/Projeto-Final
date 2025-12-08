using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ConsultaClienteController : Controller
    {
        private readonly CadastroClienteRepositorio _repo;

        public ConsultaClienteController(CadastroClienteRepositorio repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult ConsultaCliente(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                var filtrados = _repo.BuscarPorNome(nome);
                return View(filtrados);
            }

            var todos = _repo.ListarClientes();
            return View(todos);
        }
    }
}
