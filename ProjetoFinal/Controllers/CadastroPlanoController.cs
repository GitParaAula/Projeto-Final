using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroPlanoController : Controller
    {
        private readonly CadastroPlanoRepositorio _repo;

        public CadastroPlanoController(CadastroPlanoRepositorio repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult CadastroPlano()
        {
            return View(new Plano());
        }

        [HttpPost]
        public IActionResult Salvar(Plano plano)
        {
            // 🔴 PEGAR O VALOR EXATAMENTE COMO O USUÁRIO DIGITOU
            string nomeDigitado = Request.Form["Nome"];
            string valorDigitado = Request.Form["Valor"];
            string requisitosDigitado = Request.Form["Requisitos"];

            // 1️⃣ SOMENTE LETRAS / ESPAÇOS (NOME) E NÚMEROS / VÍRGULA (VALOR)
            if (!Regex.IsMatch(nomeDigitado ?? "", @"^[A-Za-zÀ-ÿ\s]+$") ||
                !Regex.IsMatch(valorDigitado ?? "", @"^[0-9,]+$"))
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return RedirectToAction("CadastroPlano");
            }

            // 2️⃣ TAMANHO DO NOME ≤ 20
            if (nomeDigitado.Length > 20)
            {
                TempData["Erro"] = "O nome só pode ter até 20 digitos.";
                return RedirectToAction("CadastroPlano");
            }

            // 3️⃣ TAMANHO TOTAL DO VALOR ≤ 8 (VÍRGULA CONTA)
            if (valorDigitado.Length > 8)
            {
                TempData["Erro"] = "O valor só pode ter até 8 digitos.";
                return RedirectToAction("CadastroPlano");
            }

            // 4️⃣ TAMANHO DOS REQUISITOS ≤ 65.535
            if ((requisitosDigitado?.Length ?? 0) > 65535)
            {
                TempData["Erro"] = "Os requisitos só podem ter até 65.535 digitos.";
                return RedirectToAction("CadastroPlano");
            }

            // 5️⃣ FORMATO DO VALOR: 1 A 6 NÚMEROS + "," + 2 NÚMEROS
            if (!Regex.IsMatch(valorDigitado, @"^\d{1,6},\d{2}$"))
            {
                TempData["Erro"] =
                    "O valor foi inserido com a estrutura errada, ele deve conter uma vírgula obrigatoriamente e deve conter dois número após a vírgula.";
                return RedirectToAction("CadastroPlano");
            }

            // 6️⃣ DURAÇÃO = DATA ATUAL + 1 ANO
            DateTime dataEsperada = DateTime.Today.AddYears(1);

            if (plano.Duracao.Date != dataEsperada.Date)
            {
                TempData["Erro"] =
                    "Os planos são anuais, por tanto deve ter exatamente um ano de duração";
                return RedirectToAction("CadastroPlano");
            }
            // 7️⃣ 🚨 VERIFICAR SE JÁ EXISTE PLANO COM ESSE NOME
            string conexao = HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                .GetConnectionString("conexaoMySQL");

            using (var con = new MySqlConnection(conexao))
            {
                con.Open();

                string sql = "SELECT COUNT(*) FROM tbPlano WHERE Nome = @nome";

                using (var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nome", nomeDigitado);

                    int existe = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existe > 0)
                    {
                        TempData["Erro"] = "Outro plano já está usando esse nome";
                        return RedirectToAction("CadastroPlano");
                    }
                }
            }

            // ✅ CONVERTER VALOR STRING → DECIMAL (pt-BR)
            plano.Valor = decimal.Parse(valorDigitado,
                new System.Globalization.CultureInfo("pt-BR"));

            // ✅ TUDO OK → SALVA
            _repo.CadastrarPlano(plano);
            return RedirectToAction("ConfirmarFuncionarioPlano", "HistoricoCadastroPlano");
        }
    }
}
