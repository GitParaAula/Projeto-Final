using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class LoginClienteRepositorio
    {
        private readonly string _conexao;

        public LoginClienteRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        public Usuario BuscarCliente(string nome, string senha)
        {
            using (MySqlConnection conn = new MySqlConnection(_conexao))
            {
                conn.Open();

                string sql = @"
                    SELECT u.*, e.*
                    FROM tbUsuario u
                    LEFT JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
                    WHERE u.Nome = @Nome";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", nome);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Usuario> lista = new List<Usuario>();

                    while (reader.Read())
                    {
                        lista.Add(new Usuario
                        {
                            Codigo_Usuario = reader.GetInt32("Codigo_Usuario"),
                            Nome = reader.GetString("Nome"),
                            Cpf = reader["Cpf"].ToString(), // CPF é bigint
                            Idade = reader.GetInt32("Idade"),
                            Email = reader.GetString("Email"),

                            Endereco = new Endereco
                            {
                                Codigo_Endereco = reader["Codigo_Endereco"] != DBNull.Value ? reader.GetInt32("Codigo_Endereco") : 0,
                                Rua = reader["Rua"] != DBNull.Value ? reader.GetString("Rua") : "",
                                Numero = reader["Numero"] != DBNull.Value ? reader.GetInt32("Numero") : 0,
                                Complemento = reader["Complemento"] != DBNull.Value ? reader.GetString("Complemento") : ""
                            }
                        });
                    }

                    // Agora percorre a lista procurando nome + senha (cpf)
                    foreach (var cli in lista)
                    {
                        if (cli.Cpf == senha) // senha é o CPF
                            return cli;
                    }

                    // se não achou nenhum que bata nome + senha
                    return null;
                }
            }
        }
        public Usuario BuscarClientePorId(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_conexao))
            {
                conn.Open();

                string sql = @"
            SELECT u.*, e.*
            FROM tbUsuario u
            LEFT JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
            WHERE u.Codigo_Usuario = @ID";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            Codigo_Usuario = reader.GetInt32("Codigo_Usuario"),
                            Nome = reader.GetString("Nome"),
                            Cpf = reader["Cpf"].ToString(),
                            Idade = reader.GetInt32("Idade"),
                            Email = reader.GetString("Email"),

                            Endereco = new Endereco
                            {
                                Codigo_Endereco = reader["Codigo_Endereco"] != DBNull.Value ? reader.GetInt32("Codigo_Endereco") : 0,
                                Rua = reader["Rua"] != DBNull.Value ? reader.GetString("Rua") : "",
                                Numero = reader["Numero"] != DBNull.Value ? reader.GetInt32("Numero") : 0,
                                Complemento = reader["Complemento"] != DBNull.Value ? reader.GetString("Complemento") : ""
                            }
                        };
                    }
                }
            }

            return null;
        }
    }
}