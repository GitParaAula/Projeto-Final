using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ConsultarPetController : Controller
    {
        private readonly ConsultarPetRepositorio _repositorio;

        public ConsultarPetController(ConsultarPetRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Consultar(string nome)
        {
            var lista = _repositorio.ListarPets(nome ?? "");
            return View(lista);
        }
    }
}
