using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ProjetoFinal.Controllers
{
    public class RedirecionamentoMenuController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        [HttpGet]
        public IActionResult RedirecionamentoMenu()
        {
            int ultimoCodigoFuncionario = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT Codigo_Funcionario FROM tbFuncionario ORDER BY Codigo_Funcionario DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        ultimoCodigoFuncionario = Convert.ToInt32(result);
                }
            }

            // Envia o código para a view
            ViewBag.CodigoFuncionario = ultimoCodigoFuncionario;

            return View();
        }
    }
}
