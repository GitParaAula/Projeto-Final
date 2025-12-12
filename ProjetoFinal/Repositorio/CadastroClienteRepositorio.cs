using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class CadastroClienteRepositorio
    {
        private readonly string _conexaoMySQL;

        public CadastroClienteRepositorio(IConfiguration configuration)
        {
            _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");
        }

        public void CadastrarCliente(Usuario usuario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // 1. Salvar endereço
                string sqlEndereco = @"
                    INSERT INTO tbEndereco (Rua, Numero, Complemento)
                    VALUES (@rua, @numero, @complemento);
                    SELECT LAST_INSERT_ID();
                ";

                MySqlCommand cmdEnd = new(sqlEndereco, conexao);
                cmdEnd.Parameters.AddWithValue("@rua", usuario.Endereco.Rua);
                cmdEnd.Parameters.AddWithValue("@numero", usuario.Endereco.Numero);
                cmdEnd.Parameters.AddWithValue("@complemento",
                    usuario.Endereco.Complemento ?? (object)DBNull.Value);

                int codigoEndereco = Convert.ToInt32(cmdEnd.ExecuteScalar());

                // 2. Salvar usuário com FK
                string sqlUsuario = @"
                    INSERT INTO tbUsuario (Nome, Cpf, Idade, Email, Codigo_Endereco)
                    VALUES (@nome, @cpf, @idade, @email, @codEnd);
                ";

                MySqlCommand cmdUser = new(sqlUsuario, conexao);
                cmdUser.Parameters.AddWithValue("@nome", usuario.Nome);
                cmdUser.Parameters.AddWithValue("@cpf", usuario.Cpf);
                cmdUser.Parameters.AddWithValue("@idade", usuario.Idade);
                cmdUser.Parameters.AddWithValue("@email", usuario.Email);
                cmdUser.Parameters.AddWithValue("@codEnd", codigoEndereco);

                cmdUser.ExecuteNonQuery();
            }
        }
        public List<Usuario> ListarClientes()
        {
            var lista = new List<Usuario>();

            using (var con = new MySqlConnection(_conexaoMySQL))
            {
                con.Open();

                string sql =
                @"SELECT u.Nome, u.Cpf, u.Idade, u.Email,
                 e.Rua, e.Numero, e.Complemento
          FROM tbUsuario u
          LEFT JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Nome = reader["Nome"].ToString(),
                            Cpf = reader["Cpf"].ToString(),
                            Idade = Convert.ToInt32(reader["Idade"]),
                            Email = reader["Email"].ToString(),
                            Endereco = new Endereco
                            {
                                Rua = reader["Rua"]?.ToString(),
                                Numero = reader["Numero"] != DBNull.Value ? Convert.ToInt32(reader["Numero"]) : 0,
                                Complemento = reader["Complemento"]?.ToString()
                            }
                        });
                    }
                }
            }

            return lista;
        }
        public List<Usuario> BuscarPorNome(string nome)
        {
            var lista = new List<Usuario>();

            using (var con = new MySqlConnection(_conexaoMySQL))
            {
                con.Open();

                string sql = @"
            SELECT u.Nome, u.Cpf, u.Idade, u.Email,
                   e.Rua, e.Numero, e.Complemento
            FROM tbUsuario u
            LEFT JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
            WHERE u.Nome LIKE @Nome;
        ";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Nome", "%" + nome + "%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Nome = reader["Nome"].ToString(),
                            Cpf = reader["Cpf"].ToString(),
                            Idade = Convert.ToInt32(reader["Idade"]),
                            Email = reader["Email"].ToString(),

                            Endereco = new Endereco
                            {
                                Rua = reader["Rua"]?.ToString(),
                                Numero = reader["Numero"] != DBNull.Value ? Convert.ToInt32(reader["Numero"]) : 0,
                                Complemento = reader["Complemento"]?.ToString()
                            }
                        });
                    }
                }
            }

            return lista;
        }
        public bool CpfExiste(string cpf)
        {
            using (var con = new MySqlConnection(_conexaoMySQL))
            {
                con.Open();

                string sql = "SELECT COUNT(*) FROM tbUsuario WHERE Cpf = @cpf";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cpf", cpf);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}
