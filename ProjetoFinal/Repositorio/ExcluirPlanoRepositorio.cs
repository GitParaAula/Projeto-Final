using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class ExcluirPlanoRepositorio
    {
        private readonly string _conexao;

        public ExcluirPlanoRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // LISTAR PLANOS COM FILTRO
        public List<Plano> Listar(string nome)
        {
            List<Plano> lista = new List<Plano>();

            using (MySqlConnection cn = new MySqlConnection(_conexao))
            {
                cn.Open();

                string sql =
                    "SELECT * FROM tbPlano " +
                    "WHERE Nome LIKE @nome";

                MySqlCommand cmd = new MySqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                MySqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Plano p = new Plano
                    {
                        Codigo_Plano = dr.GetInt32("Codigo_Plano"),
                        Nome = dr.GetString("Nome"),
                        Valor = dr.GetDecimal("Valor"),
                        Duracao = dr.GetDateTime("Duracao"),
                        Requisitos = dr.GetString("Requisitos")
                    };

                    lista.Add(p);
                }
            }

            return lista;
        }

        // EXCLUIR PLANO
        public void Excluir(int id)
        {
            using (MySqlConnection cn = new MySqlConnection(_conexao))
            {
                cn.Open();

                string sql = "DELETE FROM tbPlano WHERE Codigo_Plano = @id";

                MySqlCommand cmd = new MySqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}