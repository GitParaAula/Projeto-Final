using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class CadastroPetRepositorio
    {
        private readonly string _conexao;

        public CadastroPetRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        public void CadastrarPet(Pet pet)
        {
            using (var conn = new MySqlConnection(_conexao))
            {
                conn.Open();

                string sql = @"
                    INSERT INTO tbPet 
                    (Raca, Tipo, Porte, Nome, Idade) 
                    VALUES 
                    (@raca, @tipo, @porte, @nome, @idade)";

                MySqlCommand cmd = new(sql, conn);

                cmd.Parameters.AddWithValue("@raca", pet.Raca);
                cmd.Parameters.AddWithValue("@tipo", pet.Tipo);
                cmd.Parameters.AddWithValue("@porte", pet.Porte);
                cmd.Parameters.AddWithValue("@nome", pet.Nome);
                cmd.Parameters.AddWithValue("@idade", pet.Idade);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
