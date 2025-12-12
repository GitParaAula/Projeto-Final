using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class LoginFunc2Controller : Controller
    {
        private readonly CadastroFuncRepositorio _CadastroFuncRepositorio;
        public LoginFunc2Controller(CadastroFuncRepositorio CadastroFuncRepositorio)
        {
            _CadastroFuncRepositorio = CadastroFuncRepositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginFunc2()
        {
            return View();
        }
        public IActionResult Login(string Nome, string Cpf)
        {
            var funcionario = _CadastroFuncRepositorio.ObterFuncionario(Cpf);
            if (funcionario != null && funcionario.Cpf == Cpf && funcionario.Nome == Nome)
            {
                return RedirectToAction("Menu", "Menu");
            }
            TempData["Erro"] = "Funcionário não encontrado.";
            return View("LoginFunc2");
        }
    }
}
