using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class ComprarPlanoController : Controller
    {
        private readonly PlanoRepositorio _repositorio;

        public ComprarPlanoController(PlanoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult Index(string nome)
        {
            List<Plano> planos;

            if (!string.IsNullOrEmpty(nome))
                planos = _repositorio.BuscarPorNome(nome); // pesquisa pelo nome
            else
                planos = _repositorio.ListarPlanos(); // traz todos

            return View(planos);
        }


        public IActionResult Comprar(int id)
        {
            // Aqui você pode redirecionar para a view de compra
            // Por exemplo: return RedirectToAction("ConfirmarCompra", "Compra", new { planoId = id });
            TempData["PlanoSelecionado"] = id;
            return RedirectToAction("ConfirmarCompra", "Compra");
        }
    }
}