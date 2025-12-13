using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ProjetoFinal.Controllers
{
    public class AvisoController : Controller
    {
        private readonly string _conexao;

        public AvisoController(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        [HttpGet]
        public IActionResult Aviso()
        {
            int codigoFuncionario = 0;

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"SELECT Codigo_funcionario 
                               FROM tbFuncionario 
                               ORDER BY Codigo_funcionario DESC 
                               LIMIT 1";

                using (var cmd = new MySqlCommand(sql, con))
                {
                    var resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                        codigoFuncionario = Convert.ToInt32(resultado);
                }
            }

            // ENVIA PARA A VIEW
            ViewBag.CodigoFuncionario = codigoFuncionario;

            return View();
        }
    }
}
