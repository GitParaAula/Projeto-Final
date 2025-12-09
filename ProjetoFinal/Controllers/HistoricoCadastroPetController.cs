using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class HistoricoCadastroPetController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        // Abre a página onde digita o código do funcionário
        public IActionResult ConfirmarFuncionarioPet()
        {
            return View();
        }

        // Recebe o código e registra o histórico de PET
        [HttpPost]
        public IActionResult RegistrarPet(int Codigo_Funcionario)
        {
            string ultimoNomePet = "";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // 1. Buscar o último PET cadastrado
                string sqlUltimoPet = "SELECT Nome FROM tbPet ORDER BY Codigo_Pet DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sqlUltimoPet, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoNomePet = result.ToString();
                }

                // 2. Inserir no histórico
                string sqlInsert = @"INSERT INTO tbHistoricoCadastro 
                                     (Codigo_Funcionario, TipoCadastro, Nome, DataCadastro) 
                                     VALUES (@Codigo_Funcionario, 'Pet', @Nome, NOW())";

                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_Funcionario", Codigo_Funcionario);
                    cmd.Parameters.AddWithValue("@Nome", ultimoNomePet);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            // Redireciona para uma tela de aviso
            return RedirectToAction("AvisoCadastroPet", "AvisoCadastroPet");
        }
    }
}