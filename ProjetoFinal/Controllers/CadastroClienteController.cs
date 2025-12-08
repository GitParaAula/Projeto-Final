using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroClienteController : Controller
    {
        private readonly CadastroClienteRepositorio _repo;

        public CadastroClienteController(CadastroClienteRepositorio repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(new Usuario { Endereco = new Endereco() });
        }

        [HttpPost]
        public IActionResult Salvar(Usuario usuario)
        {
            _repo.CadastrarCliente(usuario);
            return RedirectToAction("AvisoCadastro", "AvisoCadastro");
        }
        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }
    }
}
