using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class FuncionarioRepositorio
    {
        private readonly string _conexao;

        public FuncionarioRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // LISTAR TODOS
        public List<Funcionario> ListarFuncionarios()
        {
            List<Funcionario> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();
                string sql = @"SELECT Codigo_Funcionario, Nome, Cpf, Rg FROM tbFuncionario";

                MySqlCommand cmd = new(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Funcionario
                    {
                        Codigo_funcionario = Convert.ToInt32(reader["Codigo_Funcionario"]),
                        Nome = reader["Nome"].ToString(),
                        Cpf = reader["Cpf"].ToString(),
                        Rg = reader["Rg"].ToString()
                    });
                }
            }

            return lista;
        }

        // BUSCAR POR NOME
        public List<Funcionario> BuscarPorNome(string nome)
        {
            List<Funcionario> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();
                string sql =
                    @"SELECT Codigo_Funcionario, Nome, Cpf, Rg 
                      FROM tbFuncionario 
                      WHERE Nome LIKE @nome";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Funcionario
                    {
                        Codigo_funcionario = Convert.ToInt32(reader["Codigo_Funcionario"]),
                        Nome = reader["Nome"].ToString(),
                        Cpf = reader["Cpf"].ToString(),
                        Rg = reader["Rg"].ToString()
                    });
                }
            }

            return lista;
        }

        // BUSCAR POR ID
        public Funcionario BuscarPorId(int id)
        {
            Funcionario f = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();
                string sql =
                    @"SELECT Codigo_Funcionario, Nome, Cpf, Rg 
                      FROM tbFuncionario 
                      WHERE Codigo_Funcionario = @id";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    f.Codigo_funcionario = Convert.ToInt32(reader["Codigo_Funcionario"]);
                    f.Nome = reader["Nome"].ToString();
                    f.Cpf = reader["Cpf"].ToString();
                    f.Rg = reader["Rg"].ToString();
                }
            }

            return f;
        }

        // EDITAR FUNCIONARIO
        public void EditarFuncionario(Funcionario f)
        {
            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();
                string sql =
                    @"UPDATE tbFuncionario
                      SET Nome = @nome,
                          Cpf = @cpf,
                          Rg = @rg
                      WHERE Codigo_Funcionario = @id";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@nome", f.Nome);
                cmd.Parameters.AddWithValue("@cpf", f.Cpf);
                cmd.Parameters.AddWithValue("@rg", f.Rg);
                cmd.Parameters.AddWithValue("@id", f.Codigo_funcionario);

                cmd.ExecuteNonQuery();
            }
        }
        // VERIFICAR SE CPF EXISTE EM OUTRO REGISTRO
        public bool CpfExisteEmOutroFuncionario(string cpf, int idAtual)
        {
            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();
                string sql = @"SELECT COUNT(*) 
                       FROM tbFuncionario 
                       WHERE Cpf = @cpf AND Codigo_Funcionario <> @idAtual";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@idAtual", idAtual);

                int qtd = Convert.ToInt32(cmd.ExecuteScalar());
                return qtd > 0;
            }
        }
    }
}