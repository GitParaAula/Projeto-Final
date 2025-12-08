using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class PagamentoRepositorio
    {
        private readonly string _conexao;

        public PagamentoRepositorio(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("conexaoMySQL");
        }

        public void CadastrarPagamento(Pagamento pagamento)
        {
            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"INSERT INTO tbPagamento (Titular, Valor, MetodoPagamento, Codigo_Usuario)
                               VALUES (@titular, @valor, @metodo, @usuario)";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@titular", pagamento.Titular);
                cmd.Parameters.AddWithValue("@valor", pagamento.Valor);
                cmd.Parameters.AddWithValue("@metodo", pagamento.MetodoPagamento);
                cmd.Parameters.AddWithValue("@usuario", pagamento.Codigo_Usuario);

                cmd.ExecuteNonQuery();
            }
        }
        public List<Pagamento> HistoricoUltimos10(int codigoUsuario)
        {
            var lista = new List<Pagamento>();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
                    SELECT Codigo_Pagamento, Titular, Valor, MetodoPagamento, Codigo_Usuario
                    FROM tbPagamento
                    WHERE Codigo_Usuario = @codigoUsuario
                    ORDER BY Codigo_Pagamento DESC
                    LIMIT 10;
                ";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@codigoUsuario", codigoUsuario);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Pagamento
                        {
                            Codigo_Pagamento = Convert.ToInt32(reader["Codigo_Pagamento"]),
                            Titular = reader["Titular"].ToString(),
                            Valor = Convert.ToDecimal(reader["Valor"]),
                            MetodoPagamento = reader["MetodoPagamento"].ToString(),
                            Codigo_Usuario = Convert.ToInt32(reader["Codigo_Usuario"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}
