using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class PlanoEditarRepositorio
    {
        private readonly string _conexao;

        public PlanoEditarRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // Lista todos os planos ou filtra por nome
        public List<Plano> ListarPlanos(string? nome = null)
        {
            var planos = new List<Plano>();
            using var conexao = new MySqlConnection(_conexao);
            conexao.Open();

            string sql = "SELECT * FROM tbPlano";
            if (!string.IsNullOrEmpty(nome))
                sql += " WHERE Nome LIKE @nome";

            using var cmd = new MySqlCommand(sql, conexao);
            if (!string.IsNullOrEmpty(nome))
                cmd.Parameters.AddWithValue("@nome", $"%{nome}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                planos.Add(new Plano
                {
                    Codigo_Plano = reader.GetInt32("Codigo_Plano"),
                    Nome = reader.GetString("Nome"),
                    Valor = reader.GetDecimal("Valor"),
                    Duracao = reader.GetDateTime("Duracao"),
                    Requisitos = reader.GetString("Requisitos")
                });
            }

            return planos;
        }

        // Busca plano por ID
        public Plano ObterPorId(int id)
        {
            using var conexao = new MySqlConnection(_conexao);
            conexao.Open();

            string sql = "SELECT * FROM tbPlano WHERE Codigo_Plano = @id";
            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Plano
                {
                    Codigo_Plano = reader.GetInt32("Codigo_Plano"),
                    Nome = reader.GetString("Nome"),
                    Valor = reader.GetDecimal("Valor"),
                    Duracao = reader.GetDateTime("Duracao"),
                    Requisitos = reader.GetString("Requisitos")
                };
            }

            return null!;
        }

        // Atualiza plano
        public void AtualizarPlano(Plano plano)
        {
            using var conexao = new MySqlConnection(_conexao);
            conexao.Open();

            string sql = @"UPDATE tbPlano
                           SET Nome=@nome, Valor=@valor, Duracao=@duracao, Requisitos=@requisitos
                           WHERE Codigo_Plano=@id";

            using var cmd = new MySqlCommand(sql, conexao);
            cmd.Parameters.AddWithValue("@nome", plano.Nome);
            cmd.Parameters.AddWithValue("@valor", plano.Valor);
            cmd.Parameters.AddWithValue("@duracao", plano.Duracao);
            cmd.Parameters.AddWithValue("@requisitos", plano.Requisitos);
            cmd.Parameters.AddWithValue("@id", plano.Codigo_Plano);

            cmd.ExecuteNonQuery();
        }
    }
}