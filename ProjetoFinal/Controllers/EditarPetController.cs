using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.Repositorio;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class EditarPetController : Controller
    {
        private readonly EditarPetRepositorio _repositorio;

        public EditarPetController(EditarPetRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Consultar(string nome)
        {
            var lista = _repositorio.ListarPets(nome ?? "");
            return View(lista);
        }

        public IActionResult Editar(int id)
        {
            Pet pet = _repositorio.BuscarPorId(id);
            return View(pet);
        }

        [HttpPost]
        public IActionResult ConfirmarEdicao(Pet pet)
        {
            // ---------------------- 1) VALIDAR SE SÃO LETRAS / DÍGITOS ----------------------

            // Nome: apenas letras e espaços
            if (!System.Text.RegularExpressions.Regex.IsMatch(pet.Nome, @"^[A-Za-zÀ-ÿ\s]+$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(pet.Tipo, @"^[A-Za-zÀ-ÿ\s]+$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(pet.Raca, @"^[A-Za-zÀ-ÿ\s]+$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(pet.Porte, @"^[A-Za-zÀ-ÿ]$") || // apenas UMA letra
                !System.Text.RegularExpressions.Regex.IsMatch(pet.Idade.ToString(), @"^\d+$")) // apenas números
            {
                TempData["Erro"] = "Preencha os campos corretamente";
                return View("Editar", pet);
            }


            // ---------------------- 2) NOME <= 50 ----------------------
            if (pet.Nome.Length > 50)
            {
                TempData["Erro"] = "O nome só pode ter até 50 digitos.";
                return View("Editar", pet);
            }


            // ---------------------- 3) TIPO <= 40 ----------------------
            if (pet.Tipo.Length > 40)
            {
                TempData["Erro"] = "O tipo só pode ter até 40 digitos.";
                return View("Editar", pet);
            }


            // ---------------------- 4) RAÇA <= 30 ----------------------
            if (pet.Raca.Length > 30)
            {
                TempData["Erro"] = "A raça só pode ter até 30 digitos.";
                return View("Editar", pet);
            }


            // ---------------------- 5) PORTE = 1 DIGITO ----------------------
            if (pet.Porte.Length != 1)
            {
                TempData["Erro"] = "O Porte deve ter apenas 1 digito.";
                return View("Editar", pet);
            }

            if (pet.Porte != "G" && pet.Porte != "M" && pet.Porte != "P")
            {
                if (pet.Porte == "g" || pet.Porte == "m" || pet.Porte == "p")
                {
                    TempData["Erro"] = "Porte deve ser maiúsculo.";
                    return View("Editar", pet);
                }
                else
                {
                    TempData["Erro"] = "Porte Inválido.";
                    return View("Editar", pet);
                }                    
            }
            // ---------------------- 6) IDADE <= 3 DÍGITOS ----------------------
            if (pet.Idade.ToString().Length > 3)
            {
                TempData["Erro"] = "A idade só pode ter até 3 digitos.";
                return View("Editar", pet);
            }


            // ---------------------- 7) IDADE <= 600 ----------------------
            if (pet.Idade > 600)
            {
                TempData["Erro"] = "A idade máxima permitida é de 600 anos.";
                return View("Editar", pet);
            }


            // ---------------------- SE TUDO OK → ATUALIZAR ----------------------
            _repositorio.AtualizarPet(pet);

            return RedirectToAction("Consultar");
        }
    }
}