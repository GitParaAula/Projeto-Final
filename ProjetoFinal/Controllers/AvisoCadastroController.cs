using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        [HttpGet]
        public IActionResult AvisoCadastro()
        {
            int codigoCliente = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT Codigo_Usuario FROM tbUsuario ORDER BY Codigo_Usuario DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        codigoCliente = Convert.ToInt32(result);
                }

                con.Close();
            }

            // Envia o código para a view
            ViewBag.CodigoCliente = codigoCliente;

            return View();
        }
    }
}
