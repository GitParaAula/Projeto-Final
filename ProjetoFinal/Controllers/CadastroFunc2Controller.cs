using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class CadastroFunc2Controller : Controller
    {
        private readonly CadastroFuncRepositorio _CadastroFuncRepositorio;
        public CadastroFunc2Controller(CadastroFuncRepositorio CadastroFuncRepositorio)
        {
            // O construtor é chamado quando uma nova instância de LoginController é criada.
            _CadastroFuncRepositorio = CadastroFuncRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CadastroFunc2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastroFunc2(Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _CadastroFuncRepositorio.CadastrarFunc(funcionario);
                return RedirectToAction("Aviso", "Aviso");
            }

            return View(funcionario);
        }
    }
}

