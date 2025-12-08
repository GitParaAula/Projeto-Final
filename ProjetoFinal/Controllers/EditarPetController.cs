using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class EditarPetController : Controller
    {
        private readonly EditarPetRepositorio _repositorio;

        public EditarPetController(EditarPetRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Consultar(string nome)
        {
            var lista = _repositorio.ListarPets(nome ?? "");
            return View(lista);
        }

        public IActionResult Editar(int id)
        {
            Pet pet = _repositorio.BuscarPorId(id);
            return View(pet);
        }

        [HttpPost]
        public IActionResult ConfirmarEdicao(Pet pet)
        {
            // Atualiza o pet no banco
            _repositorio.AtualizarPet(pet);

            // Redireciona para a página de consulta (com reload da lista)
            return RedirectToAction("Consultar");
        }
    }
}