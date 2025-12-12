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
            bool nomeValido = !string.IsNullOrWhiteSpace(funcionario.Nome) &&
                              funcionario.Nome.All(c => char.IsLetter(c) || c == ' ');
            bool cpfValidoNumerico = funcionario.Cpf != null &&
                                     funcionario.Cpf.All(char.IsDigit);
            bool rgValidoNumerico = funcionario.Rg != null &&
                                    funcionario.Rg.All(char.IsDigit);

            if (!nomeValido || !cpfValidoNumerico || !rgValidoNumerico)
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View("CadastroFunc3", funcionario);
            }
            if (funcionario.Nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 dígitos.";
                return View("CadastroFunc3", funcionario);
            }
            if (funcionario.Cpf.Length != 11)
            {
                TempData["Erro"] = "O CPF deve ter 11 números.";
                return View("CadastroFunc3", funcionario);
            }
            if (funcionario.Rg.Length != 9)
            {
                TempData["Erro"] = "O RG deve ter 9 números.";
                return View("CadastroFunc3", funcionario);
            }
            var funcionarioExistente = _CadastroFuncRepositorio.ObterFuncionario(funcionario.Cpf);

            if (funcionarioExistente != null)
            {
                TempData["Erro"] = "CPF já cadastrado.";
                return View("CadastroFunc3", funcionario);
            }
            if (ModelState.IsValid)
            {
                _CadastroFuncRepositorio.CadastrarFunc(funcionario);
                return RedirectToAction("ConfirmarFuncionario2", "HistoricoCadastro2");
            }

            return View("CadastroFunc3", funcionario);
        }
    }
}
