using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class EditarPlanoController : Controller
    {
        private readonly PlanoEditarRepositorio _repositorio;

        public EditarPlanoController(IConfiguration config)
        {
            _repositorio = new PlanoEditarRepositorio(config);
        }

        // Consulta com filtragem
        public IActionResult ConsultaPlano(string? nome)
        {
            var planos = _repositorio.ListarPlanos(nome);
            return View(planos);
        }

        // Editar plano (GET)
        public IActionResult Editar(int id)
        {
            var plano = _repositorio.ObterPorId(id);
            if (plano == null)
                return NotFound();

            return View(plano);
        }

        // Editar plano (POST)
        [HttpPost]
        public IActionResult Editar(Plano plano)
        {
            // --------------------- 1) VALIDAR LETRAS / NUMEROS / VIRGULA ---------------------

            // Nome = apenas letras e espaços
            bool nomeValido = System.Text.RegularExpressions.Regex.IsMatch(
                plano.Nome, @"^[A-Za-zÀ-ÿ\s]+$"
            );

            // Valor = apenas dígitos e vírgula
            bool valorValido = System.Text.RegularExpressions.Regex.IsMatch(
                plano.Valor.ToString().Replace(".", ","), @"^[0-9,]+$"
            );

            if (!nomeValido || !valorValido)
            {
                TempData["Erro"] = "Preencha os campos corretamente.";
                return View(plano);
            }


            // --------------------- 2) NOME <= 20 CARACTERES ---------------------
            if (plano.Nome.Length > 20)
            {
                TempData["Erro"] = "O nome só pode ter até 20 digitos.";
                return View(plano);
            }


            // --------------------- 3) VALOR <= 8 CARACTERES ---------------------
            string valorStr = plano.Valor.ToString().Replace(".", ",");
            if (valorStr.Length > 8)
            {
                TempData["Erro"] = "O valor só pode ter até 8 digitos.";
                return View(plano);
            }


            // --------------------- 4) REQUISITOS <= 65535 CARACTERES ---------------------
            if (!string.IsNullOrEmpty(plano.Requisitos) && plano.Requisitos.Length > 65535)
            {
                TempData["Erro"] = "Os requisitos só podem ter até 65.535 digitos.";
                return View(plano);
            }


            // --------------------- 5) ESTRUTURA DO VALOR = ######,## ---------------------
            // 1–6 números, vírgula, 2 números
            bool estruturaValor = System.Text.RegularExpressions.Regex.IsMatch(
                valorStr,
                @"^[0-9]{1,6},[0-9]{2}$"
            );

            if (!estruturaValor)
            {
                TempData["Erro"] = "O valor foi inserido com a estrutura errada, ele deve conter uma vírgula obrigatoriamente e deve conter dois números após a vírgula.";
                return View(plano);
            }


            // --------------------- 6) DURAÇÃO = DATA ATUAL + 1 ANO ---------------------
            DateTime hoje = DateTime.Today;
            DateTime esperado = new DateTime(hoje.Year + 1, hoje.Month, hoje.Day);

            if (plano.Duracao.Date != esperado)
            {
                TempData["Erro"] = "Os planos são anuais, portanto devem ter exatamente um ano de duração.";
                return View(plano);
            }

            // --------------------- 2.1) NOME JÁ EXISTE EM OUTRO PLANO ---------------------
            bool nomeDuplicado = _repositorio.NomeJaExiste(plano.Nome!, plano.Codigo_Plano);

            if (nomeDuplicado)
            {
                TempData["Erro"] = "Outro plano já está usando esse nome.";
                return View(plano);
            }

            // --------------------- SE TUDO OK, ATUALIZAR ---------------------
            _repositorio.AtualizarPlano(plano);
            TempData["Mensagem"] = "Plano atualizado com sucesso!";
            return RedirectToAction("ConsultaPlano");
        }
    }
}