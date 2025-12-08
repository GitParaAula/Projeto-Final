using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ClienteLoginConsultaController : Controller
    {
        private readonly LoginClienteRepositorio _repo;
        private readonly EditarPetRepositorio _petRepositorio;

        public ClienteLoginConsultaController(LoginClienteRepositorio repo, EditarPetRepositorio petRepositorio)
        {
            _repo = repo;
            _petRepositorio = petRepositorio;
        }

        [HttpGet]
        public IActionResult LoginCliente()
        {
            return View("~/Views/LoginCliente/LoginCliente.cshtml");
        }

        [HttpPost]
        public IActionResult LoginCliente(string usuario, string senha)
        {
            var cliente = _repo.BuscarCliente(usuario, senha);

            if (cliente == null)
            {
                TempData["Erro"] = "Nome ou senha incorretos.";
                return View("~/Views/LoginCliente/LoginCliente.cshtml");
            }

            return RedirectToAction("MostrarCliente", new { id = cliente.Codigo_Usuario });
        }

        public IActionResult MostrarCliente(int id)
        {
            var cliente = _repo.BuscarClientePorId(id);

            if (cliente == null)
            {
                TempData["Erro"] = "Cliente não encontrado.";
                return RedirectToAction("LoginCliente");
            }

            // Buscar pets com planos do cliente
            var petsComPlano = _petRepositorio.ListarPetsComPlanoPorUsuario(id);

            // Montar ViewModel
            var viewModel = new ClienteComPetsViewModel
            {
                Cliente = cliente,
                Pets = petsComPlano
            };

            return View("~/Views/LoginCliente/MostrarCliente.cshtml", viewModel);
        }
    }
}