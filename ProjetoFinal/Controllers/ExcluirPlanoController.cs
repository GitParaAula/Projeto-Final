using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ExcluirPlanoController : Controller
    {
        private readonly ExcluirPlanoRepositorio _repositorio;

        public ExcluirPlanoController(IConfiguration config)
        {
            _repositorio = new ExcluirPlanoRepositorio(config);
        }

        // GET — LISTAR / FILTRAR
        public IActionResult ExcluirPlano(string nome = "")
        {
            var planos = _repositorio.Listar(nome);
            return View(planos);
        }

        // GET — EXCLUIR
        public IActionResult Excluir(int id)
        {
            _repositorio.Excluir(id);
            return RedirectToAction("ExcluirPlano");
        }
    }
}