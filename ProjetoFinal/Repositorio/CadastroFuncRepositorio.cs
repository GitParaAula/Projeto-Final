using MySql.Data.MySqlClient;
using ProjetoFinal.Models;
using System.Data;

namespace ProjetoFinal.Repositorio
{
    public class CadastroFuncRepositorio(IConfiguration configuration)
    {
        //campo privado para leitura e armazenamento da string de con
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");

        public Funcionario ObterFuncionario(string cpf)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new("SELECT * FROM tbFuncionario WHERE Cpf = @cpf", conexao);
                cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = cpf;

                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    Funcionario funcionario = null;

                    if (dr.Read())
                    {
                        funcionario = new Funcionario
                        {
                            Codigo_funcionario = Convert.ToInt32(dr["Codigo_funcionario"]),
                            Nome = dr["Nome"].ToString(),
                            Cpf = dr["Cpf"].ToString(),
                            Rg = dr["Rg"].ToString(),
                        };
                    }
                    return funcionario;
                }
            }
        }

        // MÉTODO DE CADASTRO DE USUÁRIO
        public void CadastrarFunc(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {   
                conexao.Open();
                MySqlCommand cmd = new("INSERT INTO tbFuncionario (Nome, Cpf, Rg) VALUES (@nome, @cpf, @rg)", conexao);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@cpf", MySqlDbType.Int64).Value = funcionario.Cpf;
                cmd.Parameters.Add("@rg", MySqlDbType.Int32).Value = funcionario.Rg;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public List<Funcionario> ListarTodos()
        {
            var lista = new List<Funcionario>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string sql = "SELECT Nome, Cpf, Rg FROM tbFuncionario";

                MySqlCommand cmd = new(sql, conexao);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Funcionario
                        {
                            Nome = reader["Nome"].ToString(),
                            Cpf = reader["Cpf"].ToString(),
                            Rg = reader["Rg"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
        public List<Funcionario> BuscarPorNome(string nome)
        {
            var lista = new List<Funcionario>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string sql = "SELECT Nome, Cpf, Rg FROM tbFuncionario WHERE Nome LIKE @nome";

                MySqlCommand cmd = new(sql, conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = "%" + nome + "%";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Funcionario
                        {
                            Nome = reader["Nome"].ToString(),
                            Cpf = reader["Cpf"].ToString(),
                            Rg = reader["Rg"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
