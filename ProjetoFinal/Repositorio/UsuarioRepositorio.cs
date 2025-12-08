using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class UsuarioRepositorio
    {
        private readonly string _conexao;

        public UsuarioRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // LISTAR TODOS
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
                SELECT u.Codigo_Usuario, u.Nome, u.Cpf, u.Idade, u.Email,
                       e.Codigo_Endereco, e.Rua, e.Numero, e.Complemento
                FROM tbUsuario u
                INNER JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco";

                MySqlCommand cmd = new(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        Codigo_Usuario = Convert.ToInt32(reader["Codigo_Usuario"]),
                        Nome = reader["Nome"].ToString(),
                        Cpf = reader["Cpf"].ToString(),
                        Idade = Convert.ToInt32(reader["Idade"]),
                        Email = reader["Email"].ToString(),
                        Endereco = new Endereco
                        {
                            Codigo_Endereco = Convert.ToInt32(reader["Codigo_Endereco"]),
                            Rua = reader["Rua"].ToString(),
                            Numero = Convert.ToInt32(reader["Numero"]),
                            Complemento = reader["Complemento"].ToString()
                        }
                    });
                }
            }

            return lista;
        }

        // BUSCAR POR NOME
        public List<Usuario> BuscarPorNome(string nome)
        {
            List<Usuario> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
                SELECT u.Codigo_Usuario, u.Nome, u.Cpf, u.Idade, u.Email,
                       e.Codigo_Endereco, e.Rua, e.Numero, e.Complemento
                FROM tbUsuario u
                INNER JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
                WHERE u.Nome LIKE @nome";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        Codigo_Usuario = Convert.ToInt32(reader["Codigo_Usuario"]),
                        Nome = reader["Nome"].ToString(),
                        Cpf = reader["Cpf"].ToString(),
                        Idade = Convert.ToInt32(reader["Idade"]),
                        Email = reader["Email"].ToString(),
                        Endereco = new Endereco
                        {
                            Codigo_Endereco = Convert.ToInt32(reader["Codigo_Endereco"]),
                            Rua = reader["Rua"].ToString(),
                            Numero = Convert.ToInt32(reader["Numero"]),
                            Complemento = reader["Complemento"].ToString()
                        }
                    });
                }
            }

            return lista;
        }

        // BUSCAR POR ID
        public Usuario BuscarPorId(int id)
        {
            Usuario u = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
                SELECT u.Codigo_Usuario, u.Nome, u.Cpf, u.Idade, u.Email,
                       e.Codigo_Endereco, e.Rua, e.Numero, e.Complemento
                FROM tbUsuario u
                INNER JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
                WHERE u.Codigo_Usuario = @id";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    u.Codigo_Usuario = Convert.ToInt32(reader["Codigo_Usuario"]);
                    u.Nome = reader["Nome"].ToString();
                    u.Cpf = reader["Cpf"].ToString();
                    u.Idade = Convert.ToInt32(reader["Idade"]);
                    u.Email = reader["Email"].ToString();
                    u.Endereco = new Endereco
                    {
                        Codigo_Endereco = Convert.ToInt32(reader["Codigo_Endereco"]),
                        Rua = reader["Rua"].ToString(),
                        Numero = Convert.ToInt32(reader["Numero"]),
                        Complemento = reader["Complemento"].ToString()
                    };
                }
            }

            return u;
        }

        // EDITAR USUARIO + ENDEREÇO
        public void EditarUsuario(Usuario u)
        {
            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                // Atualiza endereco
                string sqlEnd = @"
                UPDATE tbEndereco
                   SET Rua = @rua,
                       Numero = @numero,
                       Complemento = @complemento
                 WHERE Codigo_Endereco = @codEnd";

                MySqlCommand cmdEnd = new(sqlEnd, con);
                cmdEnd.Parameters.AddWithValue("@rua", u.Endereco.Rua);
                cmdEnd.Parameters.AddWithValue("@numero", u.Endereco.Numero);
                cmdEnd.Parameters.AddWithValue("@complemento", u.Endereco.Complemento);
                cmdEnd.Parameters.AddWithValue("@codEnd", u.Endereco.Codigo_Endereco);
                cmdEnd.ExecuteNonQuery();

                // Atualiza usuario
                string sql = @"
                UPDATE tbUsuario
                   SET Nome = @nome,
                       Cpf = @cpf,
                       Idade = @idade,
                       Email = @email
                 WHERE Codigo_Usuario = @id";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@nome", u.Nome);
                cmd.Parameters.AddWithValue("@cpf", u.Cpf);
                cmd.Parameters.AddWithValue("@idade", u.Idade);
                cmd.Parameters.AddWithValue("@email", u.Email);
                cmd.Parameters.AddWithValue("@id", u.Codigo_Usuario);

                cmd.ExecuteNonQuery();
            }
        }
    }
}