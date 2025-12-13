using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ProjetoFinal.Controllers
{
    public class AvisoCadastroPetController : Controller
    {
        private readonly string connectionString =
            "server=localhost;database=dbTcm;uid=root;pwd=12345678;";

        [HttpGet]
        public IActionResult AvisoCadastroPet()
        {
            int codigoPet = 0;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                string sql = "SELECT Codigo_Pet FROM tbPet ORDER BY Codigo_Pet DESC LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        codigoPet = Convert.ToInt32(result);
                }

                con.Close();
            }

            // Envia o código do pet para a view
            ViewBag.CodigoPet = codigoPet;

            return View();
        }
    }
}
