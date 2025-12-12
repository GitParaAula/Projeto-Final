using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class HistoricoCadastroController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        // Abre a tela onde digita o código do funcionário
        public IActionResult ConfirmarFuncionario()
        {
            return View();
        }

        // Recebe o código e registra o histórico
        [HttpPost]
        public IActionResult Registrar(int Codigo_Funcionario)
        {
            string ultimoNomeFuncionario = "";

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                // 1️⃣ VERIFICAR SE O FUNCIONÁRIO EXISTE
                string sqlVerificaFun = @"SELECT COUNT(*) 
                                  FROM tbFuncionario 
                                  WHERE Codigo_Funcionario = @Codigo";

                using (MySqlCommand cmd = new MySqlCommand(sqlVerificaFun, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo", Codigo_Funcionario);
                    int qtd = Convert.ToInt32(cmd.ExecuteScalar());

                    if (qtd == 0)
                    {
                        TempData["Erro"] = "Este Funcionário não existe.";
                        return RedirectToAction("ConfirmarFuncionario");
                    }
                }

                // 2️⃣ Buscar o último funcionário cadastrado (do jeito que você já fazia)
                string sqlUltimoFun = "SELECT Nome FROM tbFuncionario ORDER BY Codigo_Funcionario DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sqlUltimoFun, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoNomeFuncionario = result.ToString();
                }

                // 3️⃣ Inserir no histórico
                string sqlInsert = @"INSERT INTO tbHistoricoCadastro 
                             (Codigo_Funcionario, TipoCadastro, Nome, DataCadastro) 
                             VALUES (@Codigo_Funcionario, 'Funcionario', @Nome, NOW())";

                using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_Funcionario", Codigo_Funcionario);
                    cmd.Parameters.AddWithValue("@Nome", ultimoNomeFuncionario);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            return RedirectToAction("Aviso", "Aviso");
        }

        // Tela de confirmação
        public IActionResult Sucesso()
        {
            return Content("Histórico de cadastro registrado com sucesso!");
        }
    }
}