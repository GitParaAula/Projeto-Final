using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class ExcluirClienteRepositorio
    {
        private readonly string _conexao;

        public ExcluirClienteRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }
        public List<Usuario> Listar(string nome)
        {
            List<Usuario> lista = new List<Usuario>();

            using (MySqlConnection conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                string sql = @"
            SELECT 
                u.Codigo_Usuario,
                u.Nome,
                u.Cpf,
                u.Idade,
                u.Email,
                e.Codigo_Endereco,
                e.Rua,
                e.Numero,
                e.Complemento
            FROM tbUsuario u
            LEFT JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco
            WHERE u.Nome LIKE @nome";

                using (MySqlCommand cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Usuario cliente = new Usuario();

                            cliente.Codigo_Usuario = dr.GetInt32("Codigo_Usuario");
                            cliente.Nome = dr.GetString("Nome");
                            cliente.Cpf = dr["Cpf"].ToString();
                            cliente.Idade = dr.GetInt32("Idade");
                            cliente.Email = dr.GetString("Email");

                            // Montando o ENDEREÇO
                            cliente.Endereco = new Endereco
                            {
                                Codigo_Endereco = dr["Codigo_Endereco"] != DBNull.Value ? dr.GetInt32("Codigo_Endereco") : 0,
                                Rua = dr["Rua"] != DBNull.Value ? dr.GetString("Rua") : "",
                                Numero = dr["Numero"] != DBNull.Value ? dr.GetInt32("Numero") : 0,
                                Complemento = dr["Complemento"] != DBNull.Value ? dr.GetString("Complemento") : ""
                            };

                            lista.Add(cliente);
                        }
                    }
                }
            }

            return lista;
        }
        public void Excluir(int codigoUsuario)
        {
            using (var conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();
                using (var trans = conexao.BeginTransaction())
                {
                    try
                    {
                        // 1) Buscar o Codigo_Endereco associado ao usuário (se existir)
                        int codigoEndereco = 0;
                        using (var cmdBuscar = new MySqlCommand(
                            "SELECT Codigo_Endereco FROM tbUsuario WHERE Codigo_Usuario = @id",
                            conexao, trans))
                        {
                            cmdBuscar.Parameters.AddWithValue("@id", codigoUsuario);
                            var result = cmdBuscar.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                codigoEndereco = Convert.ToInt32(result);
                        }

                        // 2) Excluir o usuário
                        using (var cmdDelUser = new MySqlCommand(
                            "DELETE FROM tbUsuario WHERE Codigo_Usuario = @id",
                            conexao, trans))
                        {
                            cmdDelUser.Parameters.AddWithValue("@id", codigoUsuario);
                            cmdDelUser.ExecuteNonQuery();
                        }

                        // 3) Excluir o endereço (se encontrado)
                        if (codigoEndereco > 0)
                        {
                            using (var cmdDelEnd = new MySqlCommand(
                                "DELETE FROM tbEndereco WHERE Codigo_Endereco = @end",
                                conexao, trans))
                            {
                                cmdDelEnd.Parameters.AddWithValue("@end", codigoEndereco);
                                cmdDelEnd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        try { trans.Rollback(); } catch { /* ignorar se rollback falhar */ }
                        throw; // relança para o controller/log tratar
                    }
                }
            }
        }
        public void ExcluirCliente(int codigoUsuario)
        {
            using (MySqlConnection conexao = new MySqlConnection(_conexao))
            {
                conexao.Open();

                // 1) Buscar o código do endereço antes de excluir
                int codigoEndereco = 0;

                string sqlEndereco = "SELECT Codigo_Endereco FROM tbUsuario WHERE Codigo_Usuario = @cod";

                using (MySqlCommand cmdEnd = new MySqlCommand(sqlEndereco, conexao))
                {
                    cmdEnd.Parameters.AddWithValue("@cod", codigoUsuario);
                    var result = cmdEnd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                        codigoEndereco = Convert.ToInt32(result);
                }

                // 2) Excluir o usuário
                string sqlUsuario = "DELETE FROM tbUsuario WHERE Codigo_Usuario = @cod";

                using (MySqlCommand cmdUser = new MySqlCommand(sqlUsuario, conexao))
                {
                    cmdUser.Parameters.AddWithValue("@cod", codigoUsuario);
                    cmdUser.ExecuteNonQuery();
                }

                // 3) Excluir o endereço (se existir)
                if (codigoEndereco > 0)
                {
                    string sqlDelEnd = "DELETE FROM tbEndereco WHERE Codigo_Endereco = @end";

                    using (MySqlCommand cmdDelEnd = new MySqlCommand(sqlDelEnd, conexao))
                    {
                        cmdDelEnd.Parameters.AddWithValue("@end", codigoEndereco);
                        cmdDelEnd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}