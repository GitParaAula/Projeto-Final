using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class EditarFuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _repo;

        public EditarFuncionarioController(FuncionarioRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var funcionario = _repo.BuscarPorId(id);
            return View(funcionario);
        }

        [HttpPost]
        public IActionResult Editar(Funcionario f)
        {
            // 1️⃣ Validar Nome: apenas letras e espaços
            if (!System.Text.RegularExpressions.Regex.IsMatch(f.Nome, @"^[A-Za-zÀ-ÿ\s]+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(f);
            }

            // 2️⃣ Nome <= 50 caracteres
            if (f.Nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 dígitos.";
                return View(f);
            }

            // 3️⃣ CPF: apenas dígitos e exatamente 11
            if (!System.Text.RegularExpressions.Regex.IsMatch(f.Cpf, @"^\d{11}$"))
            {
                TempData["Erro"] = "O cpf deve ter 11 números.";
                return View(f);
            }

            // 4️⃣ RG: apenas dígitos e exatamente 9
            if (!System.Text.RegularExpressions.Regex.IsMatch(f.Rg, @"^\d{9}$"))
            {
                TempData["Erro"] = "O rg deve ter 9 números.";
                return View(f);
            }

            // 5️⃣ Verificar CPF duplicado (exceto o próprio registro)
            if (_repo.CpfExisteEmOutroFuncionario(f.Cpf, f.Codigo_funcionario))
            {
                TempData["Erro"] = "Cpf já cadastrado em outro registro.";
                return View(f);
            }

            // 6️⃣ Se tudo estiver certo → salvar
            _repo.EditarFuncionario(f);

            return RedirectToAction("ConsultaFuncionario", "ConsultaFuncionario");
        }
    }
}
