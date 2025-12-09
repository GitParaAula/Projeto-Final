using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroFunc3Controller : Controller
    {
        private readonly CadastroFuncRepositorio _CadastroFuncRepositorio;
        public CadastroFunc3Controller(CadastroFuncRepositorio CadastroFuncRepositorio)
        {
            // O construtor é chamado quando uma nova instância de LoginController é criada.
            _CadastroFuncRepositorio = CadastroFuncRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CadastroFunc3()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastroFunc3(Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _CadastroFuncRepositorio.CadastrarFunc(funcionario);
                return RedirectToAction("ConfirmarFuncionario2","HistoricoCadastro2");
            }

            return View(funcionario);
        }
    }
}
