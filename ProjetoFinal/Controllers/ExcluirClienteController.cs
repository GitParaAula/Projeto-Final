using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ExcluirClienteController : Controller
    {
        private readonly ExcluirClienteRepositorio _repositorio;

        public ExcluirClienteController(ExcluirClienteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // LISTAR + FILTRO
        public IActionResult ExcluirCliente(string nome = "")
        {
            var clientes = _repositorio.Listar(nome);
            return View(clientes);
        }

        // EXCLUSÃO
        public IActionResult Excluir(int id)
        {
            _repositorio.Excluir(id);
            return RedirectToAction("ExcluirCliente");
        }
    }
}