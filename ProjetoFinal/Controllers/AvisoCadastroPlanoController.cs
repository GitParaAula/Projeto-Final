using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroPlanoController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        [HttpGet]
        public IActionResult AvisoCadastroPlano()
        {
            int codigoPlano = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT Codigo_Plano FROM tbPlano ORDER BY Codigo_Plano DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        codigoPlano = Convert.ToInt32(result);
                }

                con.Close();
            }

            // Envia o código do plano para a view
            ViewBag.CodigoPlano = codigoPlano;

            return View();
        }
    }
}