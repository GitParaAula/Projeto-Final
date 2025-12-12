using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Models;
using ProjetoFinal.Repositorio;

namespace ProjetoFinal.Controllers
{
    public class CadastroPetController : Controller
    {
        private readonly CadastroPetRepositorio _repo;

        public CadastroPetController(CadastroPetRepositorio repo)
        {
            _repo = repo;
        }

        public IActionResult CadastroPet()
        {
            return View(new Pet());
        }

        [HttpPost]
        public IActionResult Salvar(Pet pet)
        {
            // ===============================
            // 1️⃣ VALIDAÇÃO DE FORMATO
            // ===============================

            bool nomeValido = Regex.IsMatch(pet.Nome ?? "", @"^[A-Za-zÀ-ÿ\s]+$");
            bool tipoValido = Regex.IsMatch(pet.Tipo ?? "", @"^[A-Za-zÀ-ÿ\s]+$");
            bool racaValida = Regex.IsMatch(pet.Raca ?? "", @"^[A-Za-zÀ-ÿ\s]+$");
            bool porteValido = Regex.IsMatch(pet.Porte ?? "", @"^[A-Za-z]$");
            bool idadeValida = Regex.IsMatch(pet.Idade.ToString(), @"^\d+$");

            if (!nomeValido || !tipoValido || !racaValida || !porteValido || !idadeValida)
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // 2️⃣ TAMANHO DO NOME
            // ===============================
            if (pet.Nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 digitos.";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // 3️⃣ TAMANHO DO TIPO
            // ===============================
            if (pet.Tipo.Length > 40)
            {
                TempData["Erro"] = "O tipo só pode ter até 40 digitos.";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // 4️⃣ TAMANHO DA RAÇA
            // ===============================
            if (pet.Raca.Length > 30)
            {
                TempData["Erro"] = "A raça só pode ter até 30 digitos.";
                return RedirectToAction("CadastroPet");
            }
            if (pet.Porte != "G" && pet.Porte != "M" && pet.Porte != "P")
            {
                if (pet.Porte == "g" || pet.Porte == "m" || pet.Porte == "p")
                {
                    TempData["Erro"] = "Porte deve ser maiúsculo.";
                    return View("CadastroPet");
                }
                else
                {
                    TempData["Erro"] = "Porte Inválido.";
                    return View("CadastroPet");
                }
            }
            // ===============================
            // 5️⃣ PORTE = 1 CARACTERE
            // ===============================
            if (pet.Porte.Length != 1)
            {
                TempData["Erro"] = "O Porte deve ter apenas um digito.";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // 6️⃣ IDADE ATÉ 3 DÍGITOS
            // ===============================
            if (pet.Idade.ToString().Length > 3)
            {
                TempData["Erro"] = "A idade só pode ter até 3 digitos";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // 7️⃣ IDADE MÁXIMA = 600
            // ===============================
            if (pet.Idade > 600)
            {
                TempData["Erro"] = "A idade máxima permitida é de 600 anos";
                return RedirectToAction("CadastroPet");
            }

            // ===============================
            // ✅ TUDO OK → SALVAR
            // ===============================
            _repo.CadastrarPet(pet);
            return RedirectToAction("ConfirmarFuncionarioPet", "HistoricoCadastroPet");
        }
    }
}
