using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class GerenciarClienteController : Controller
    {
        private readonly UsuarioRepositorio _repo;

        public GerenciarClienteController(UsuarioRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult ConsultaCliente(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                return View(_repo.BuscarPorNome(nome));

            return View(_repo.ListarUsuarios());
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            return View(_repo.BuscarPorId(id));
        }

        [HttpPost]
        public IActionResult Editar(Usuario u)
        {
            _repo.EditarUsuario(u);
            return RedirectToAction("ConsultaCliente");
        }
    }
}