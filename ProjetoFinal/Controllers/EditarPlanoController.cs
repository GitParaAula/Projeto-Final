using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class EditarPlanoController : Controller
    {
        private readonly PlanoEditarRepositorio _repositorio;

        public EditarPlanoController(IConfiguration config)
        {
            _repositorio = new PlanoEditarRepositorio(config);
        }

        // Consulta com filtragem
        public IActionResult ConsultaPlano(string? nome)
        {
            var planos = _repositorio.ListarPlanos(nome);
            return View(planos);
        }

        // Editar plano (GET)
        public IActionResult Editar(int id)
        {
            var plano = _repositorio.ObterPorId(id);
            if (plano == null)
                return NotFound();

            return View(plano);
        }

        // Editar plano (POST)
        [HttpPost]
        public IActionResult Editar(Plano plano)
        {
            if (!ModelState.IsValid)
                return View(plano);

            _repositorio.AtualizarPlano(plano);
            TempData["Mensagem"] = "Plano atualizado com sucesso!";
            return RedirectToAction("ConsultaPlano");
        }
    }
}