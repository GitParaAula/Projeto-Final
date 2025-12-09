using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class HistoricoCadastroClienteController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        // Abre a tela onde digita o código do funcionário
        public IActionResult ConfirmarCliente()
        {
            return View();
        }

        // Recebe o código e registra o histórico como CLIENTE
        [HttpPost]
        public IActionResult Registrar(int Codigo_Funcionario)
        {
            string ultimoNomeUsuario = "";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // Buscar o último USUÁRIO cadastrado
                string sqlUltimoUsuario = "SELECT Nome FROM tbUsuario ORDER BY Codigo_Usuario DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sqlUltimoUsuario, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoNomeUsuario = result.ToString();
                }

                // Inserir no Historico
                string sqlInsert = @"INSERT INTO tbHistoricoCadastro 
                                     (Codigo_Funcionario, TipoCadastro, Nome, DataCadastro) 
                                     VALUES (@Codigo_Funcionario, 'Cliente', @Nome, NOW())";

                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_Funcionario", Codigo_Funcionario);
                    cmd.Parameters.AddWithValue("@Nome", ultimoNomeUsuario);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            return RedirectToAction("AvisoCadastro", "AvisoCadastro");
        }
    }
}
