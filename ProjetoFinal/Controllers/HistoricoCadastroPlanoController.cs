using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class HistoricoCadastroPlanoController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        // Abre tela para digitar código do funcionário
        public IActionResult ConfirmarFuncionarioPlano()
        {
            return View();
        }

        // Recebe código e registra o histórico para PLANO
        [HttpPost]
        public IActionResult RegistrarPlano(int Codigo_Funcionario)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // 1️⃣ VERIFICAR SE FUNCIONÁRIO EXISTE
                string sqlFuncionario =
                    "SELECT COUNT(*) FROM tbFuncionario WHERE Codigo_Funcionario = @Codigo";

                using (MySqlCommand cmd = new MySqlCommand(sqlFuncionario, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo", Codigo_Funcionario);

                    int existeFuncionario = Convert.ToInt32(cmd.ExecuteScalar());

                    if (existeFuncionario == 0)
                    {
                        TempData["Erro"] = "Funcionário não encontrado.";
                        return RedirectToAction("ConfirmarFuncionarioPlano");
                    }
                }

                // 2️⃣ BUSCAR O ÚLTIMO PLANO CADASTRADO
                string ultimoNomePlano = "";

                string sqlUltimoPlano =
                    "SELECT Nome FROM tbPlano ORDER BY Codigo_Plano DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sqlUltimoPlano, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoNomePlano = result.ToString();
                }

                // 3️⃣ INSERIR NO HISTÓRICO
                string sqlInsert = @"INSERT INTO tbHistoricoCadastro
                             (Codigo_Funcionario, TipoCadastro, Nome, DataCadastro)
                             VALUES (@Codigo_Funcionario, 'Plano', @Nome, NOW())";

                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_Funcionario", Codigo_Funcionario);
                    cmd.Parameters.AddWithValue("@Nome", ultimoNomePlano);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            return RedirectToAction("AvisoCadastroPlano", "AvisoCadastroPlano");
        }
    }
}