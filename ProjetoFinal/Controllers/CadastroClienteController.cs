using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroClienteController : Controller
    {
        private readonly CadastroClienteRepositorio _repo;

        public CadastroClienteController(CadastroClienteRepositorio repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(new Usuario { Endereco = new Endereco() });
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Salvar(Usuario usuario)
        {
            // 🔹 Captura dos valores crus digitados
            string nome = Request.Form["Nome"];
            string cpf = Request.Form["Cpf"];
            string idadeRaw = Request.Form["Idade"];
            string email = Request.Form["Email"];
            string rua = Request.Form["Endereco.Rua"];
            string numeroRaw = Request.Form["Endereco.Numero"];
            string complemento = Request.Form["Endereco.Complemento"];

            // 🔹 Regex base
            Regex letrasEspacos = new(@"^[A-Za-zÀ-ÿ ]+$");
            Regex apenasDigitos = new(@"^[0-9]+$");

            // 1️⃣ Validação de composição
            if (!letrasEspacos.IsMatch(nome) ||
                !apenasDigitos.IsMatch(cpf) ||
                !apenasDigitos.IsMatch(idadeRaw) ||
                !letrasEspacos.IsMatch(rua) ||
                !apenasDigitos.IsMatch(numeroRaw))
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return View("CadastroCliente", usuario);
            }

            // 2️⃣ Nome ≤ 50
            if (nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 digitos.";
                return View("CadastroCliente", usuario);
            }

            // 3️⃣ CPF = 11 dígitos
            if (cpf.Length != 11)
            {
                TempData["Erro"] = "O cpf deve ter 11 números.";
                return View("CadastroCliente", usuario);
            }

            // 4️⃣ Idade ≤ 3 dígitos
            if (idadeRaw.Length > 3)
            {
                TempData["Erro"] = "A idade deve ter no máximo 3 digitos";
                return View("CadastroCliente", usuario);
            }

            int idade = int.Parse(idadeRaw);

            // 5️⃣ Idade ≤ 130
            if (idade > 130)
            {
                TempData["Erro"] = "A idade máxima permitida é de 130 anos";
                return View("CadastroCliente", usuario);
            }

            // 6️⃣ Email ≤ 50
            if (email.Length > 50)
            {
                TempData["Erro"] = "O email deve ter no maximo 50 caracteres";
                return View("CadastroCliente", usuario);
            }

            // 7️⃣ Rua ≤ 60
            if (rua.Length > 60)
            {
                TempData["Erro"] = "Rua deve ter no maximo 60 caracteres";
                return View("CadastroCliente", usuario);
            }

            // 8️⃣ Número ≤ 5 dígitos
            if (numeroRaw.Length > 5)
            {
                TempData["Erro"] = "O Número só pode ter no máximo 5 digitos";
                return View("CadastroCliente", usuario);
            }

            // 9️⃣ Complemento ≤ 30
            if (!string.IsNullOrEmpty(complemento) && complemento.Length > 30)
            {
                TempData["Erro"] = "O campo complemente deve ter no máximo 30 digitos";
                return View("CadastroCliente", usuario);
            }

            // 🔟 CPF já existente
            if (_repo.CpfExiste(cpf))
            {
                TempData["Erro"] = "Este Cpf já está em uso";
                return View("CadastroCliente", usuario);
            }

            // 1️⃣1️⃣ Estrutura de email
            Regex emailRegex = new(@"^.+@.+mail\.com$");
            if (!emailRegex.IsMatch(email))
            {
                TempData["Erro"] = "Estrutura de email invalida.";
                return View("CadastroCliente", usuario);
            }

            // 🔹 Conversões finais
            usuario.Idade = idade;
            usuario.Endereco.Numero = int.Parse(numeroRaw);

            // 🔹 Salvar
            _repo.CadastrarCliente(usuario);

            return RedirectToAction("ConfirmarCliente", "HistoricoCadastroCliente");
        }
        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View();
        }
    }
}
