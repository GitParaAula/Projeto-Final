using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class CadastroPlanoRepositorio
    {
        private readonly string _conexao;

        public CadastroPlanoRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        public void CadastrarPlano(Plano plano)
        {
            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"INSERT INTO tbPlano 
                               (Nome, Valor, Duracao, Requisitos) 
                               VALUES (@Nome, @Valor, @Duracao, @Requisitos)";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Nome", plano.Nome);
                cmd.Parameters.AddWithValue("@Valor", plano.Valor);
                cmd.Parameters.AddWithValue("@Duracao", plano.Duracao);
                cmd.Parameters.AddWithValue("@Requisitos", plano.Requisitos);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
