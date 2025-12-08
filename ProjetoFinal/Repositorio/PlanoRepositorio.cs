using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class PlanoRepositorio
    {
        private readonly string _conexaoMySQL;

        public PlanoRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public List<Plano> ListarPlanos()
        {
            var lista = new List<Plano>();

            using (var con = new MySqlConnection(_conexaoMySQL))
            {
                con.Open();

                string sql = @"SELECT Codigo_Plano, Nome, Valor, Duracao, Requisitos FROM tbPlano";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Plano
                        {
                            Codigo_Plano = Convert.ToInt32(reader["Codigo_Plano"]),
                            Nome = reader["Nome"].ToString(),
                            Valor = Convert.ToDecimal(reader["Valor"]),
                            Duracao = Convert.ToDateTime(reader["Duracao"]),
                            Requisitos = reader["Requisitos"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public List<Plano> BuscarPorNome(string nome)
        {
            var lista = new List<Plano>();

            using (var con = new MySqlConnection(_conexaoMySQL))
            {
                con.Open();

                string sql = @"
                    SELECT Codigo_Plano, Nome, Valor, Duracao, Requisitos
                    FROM tbPlano
                    WHERE Nome LIKE @Nome;
                ";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Nome", "%" + nome + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Plano
                        {
                            Codigo_Plano = Convert.ToInt32(reader["Codigo_Plano"]),
                            Nome = reader["Nome"].ToString(),
                            Valor = Convert.ToDecimal(reader["Valor"]),
                            Duracao = Convert.ToDateTime(reader["Duracao"]),
                            Requisitos = reader["Requisitos"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
