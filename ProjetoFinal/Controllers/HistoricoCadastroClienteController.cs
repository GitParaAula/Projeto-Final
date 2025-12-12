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
        [HttpPost]
        public IActionResult Registrar(int Codigo_Funcionario)
        {
            string ultimoNomeUsuario = "";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // 1️⃣ VERIFICAR SE O FUNCIONÁRIO EXISTE
                string sqlExiste = "SELECT COUNT(*) FROM tbFuncionario WHERE Codigo_Funcionario = @codigo";

                using (MySqlCommand cmdExiste = new MySqlCommand(sqlExiste, con))
                {
                    cmdExiste.Parameters.AddWithValue("@codigo", Codigo_Funcionario);

                    int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    if (existe == 0)
                    {
                        TempData["Erro"] = "Funcionário não encontrado.";
                        return RedirectToAction("ConfirmarCliente");
                    }
                }

                // 2️⃣ BUSCAR O ÚLTIMO CLIENTE CADASTRADO
                string sqlUltimoUsuario =
                    "SELECT Nome FROM tbUsuario ORDER BY Codigo_Usuario DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sqlUltimoUsuario, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoNomeUsuario = result.ToString();
                }

                // 3️⃣ INSERIR NO HISTÓRICO
                string sqlInsert = @"
            INSERT INTO tbHistoricoCadastro
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
