using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroPetController : Controller
    {
        private readonly CadastroPetRepositorio _repo;

        public CadastroPetController(CadastroPetRepositorio repo)
        {
            _repo = repo;
        }

        public IActionResult CadastroPet()
        {
            return View(new Pet());
        }

        [HttpPost]
        public IActionResult Salvar(Pet pet)
        {
            _repo.CadastrarPet(pet);
            return RedirectToAction("AvisoCadastroPet", "AvisoCadastroPet");
        }
    }
}
