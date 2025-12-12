using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class GerenciarClienteController : Controller
    {
        private readonly UsuarioRepositorio _repo;

        public GerenciarClienteController(UsuarioRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult ConsultaCliente(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                return View(_repo.BuscarPorNome(nome));

            return View(_repo.ListarUsuarios());
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            return View(_repo.BuscarPorId(id));
        }

        [HttpPost]
        public IActionResult Editar(Usuario u)
        {
            // --------------------- 1) VALIDAR CAMPOS ---------------------

            // Nome: apenas letras e espaços
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Nome, @"^[A-Za-zÀ-ÿ\s]+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(u);
            }

            // CPF: apenas dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Cpf, @"^\d+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(u);
            }

            // Idade: apenas dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Idade.ToString(), @"^\d+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(u);
            }

            // Rua: apenas letras e espaços
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Endereco.Rua, @"^[A-Za-zÀ-ÿ\s]+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(u);
            }

            // Número: apenas dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Endereco.Numero.ToString(), @"^\d+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(u);
            }


            // --------------------- 2) TAMANHO DOS CAMPOS ---------------------

            // Nome <= 50
            if (u.Nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 digitos.";
                return View(u);
            }

            // CPF deve ter exatamente 11 dígitos
            if (u.Cpf.Length != 11)
            {
                TempData["Erro"] = "O cpf deve ter 11 números.";
                return View(u);
            }

            // Idade deve ter no máximo 3 dígitos
            if (u.Idade.ToString().Length > 3)
            {
                TempData["Erro"] = "A idade deve ter no máximo 3 digitos.";
                return View(u);
            }

            // Idade <= 130
            if (u.Idade > 130)
            {
                TempData["Erro"] = "A idade máxima permitida é de 130 anos.";
                return View(u);
            }

            // Email <= 50
            if (u.Email.Length > 50)
            {
                TempData["Erro"] = "O email deve ter no maximo 50 caracteres.";
                return View(u);
            }

            // Rua <= 60
            if (u.Endereco.Rua.Length > 60)
            {
                TempData["Erro"] = "Rua deve ter no maximo 60 caracteres.";
                return View(u);
            }

            // Número <= 5 dígitos
            if (u.Endereco.Numero.ToString().Length > 5)
            {
                TempData["Erro"] = "O Número só pode ter no máximo 5 digitos.";
                return View(u);
            }

            // Complemento <= 30
            if (u.Endereco.Complemento != null && u.Endereco.Complemento.Length > 30)
            {
                TempData["Erro"] = "O campo complemento deve ter no máximo 30 digitos.";
                return View(u);
            }


            // --------------------- 3) CPF DUPLICADO ---------------------

            if (_repo.CpfExisteEmOutroUsuario(u.Cpf, u.Codigo_Usuario))
            {
                TempData["Erro"] = "Este Cpf já está em uso.";
                return View(u);
            }


            // --------------------- 4) VALIDAR ESTRUTURA DO EMAIL ---------------------

            // Qualquer prefixo + @ + qualquer prefixo + mail.com
            if (!System.Text.RegularExpressions.Regex.IsMatch(u.Email, @"^.+@.+mail\.com$"))
            {
                TempData["Erro"] = "Estrutura de email invalida.";
                return View(u);
            }


            // --------------------- 5) SE TUDO OK → SALVAR ---------------------

            _repo.EditarUsuario(u);
            return RedirectToAction("ConsultaCliente");
        }
    }
}