using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Controllers
{
    public class HistoricoCadastroConsultaController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        // ===============================
        // LISTAR ÚLTIMOS 10 REGISTROS
        // ===============================
        public IActionResult Listar(int codigoFuncionario)
        {
            List<HistoricoCadastro> lista = new();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = @"
                    SELECT Codigo_HistoricoCadastro, TipoCadastro, Nome, DataCadastro
                    FROM tbHistoricoCadastro
                    WHERE Codigo_Funcionario = @codigo
                    ORDER BY Codigo_HistoricoCadastro DESC
                    LIMIT 10";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codigo", codigoFuncionario);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new HistoricoCadastro
                            {
                                Codigo_HistoricoCadastro = reader.GetInt32("Codigo_HistoricoCadastro"),
                                TipoCadastro = reader.GetString("TipoCadastro"),
                                Nome = reader.GetString("Nome"),
                                DataCadastro = reader.GetDateTime("DataCadastro")
                            });
                        }
                    }
                }

                con.Close();
            }

            return View("ResultadoHistorico", lista);
        }
    }
}