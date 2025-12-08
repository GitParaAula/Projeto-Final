using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ExcluirPetController : Controller
    {
        private readonly PetRepositorio _repo;

        public ExcluirPetController(PetRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult ExcluirPet(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                var filtrados = _repo.BuscarPorNome(nome);
                return View(filtrados);
            }

            var todos = _repo.ListarPets();
            return View(todos);
        }

        [HttpPost]
        public IActionResult ConfirmarExcluir(int id)
        {
            _repo.ExcluirPet(id);
            return RedirectToAction("ExcluirPet");
        }
    }
}