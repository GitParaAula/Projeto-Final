using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class ExcluirFuncionarioRepositorio
    {
        private readonly string _conexao;

        public ExcluirFuncionarioRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // LISTAR POR NOME
        public List<Funcionario> Listar(string nome)
        {
            List<Funcionario> lista = new();

            using (MySqlConnection cn = new(_conexao))
            {
                cn.Open();

                string query = "SELECT * FROM tbFuncionario WHERE Nome LIKE @nome";

                using (MySqlCommand cmd = new(query, cn))
                {
                    cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Funcionario
                            {
                                Codigo_funcionario = dr.GetInt32("Codigo_Funcionario"),
                                Nome = dr.GetString("Nome"),
                                Cpf = dr["Cpf"].ToString(),
                                Rg = dr["Rg"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }


        // EXCLUIR
        public void Excluir(int id)
        {
            using (MySqlConnection cn = new(_conexao))
            {
                cn.Open();

                string query = "DELETE FROM tbFuncionario WHERE Codigo_Funcionario = @id";

                using (MySqlCommand cmd = new(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}