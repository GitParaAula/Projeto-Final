using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class PetRepositorio
    {
        private readonly string _conexao;

        public PetRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        // LISTAR TODOS
        public List<Pet> ListarPets()
        {
            List<Pet> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"SELECT Codigo_Pet, Raca, Tipo, Porte, Nome, Idade, Codigo_Plano, Codigo_Usuario
                               FROM tbPet";

                MySqlCommand cmd = new(sql, con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Pet
                    {
                        Codigo_Pet = Convert.ToInt32(reader["Codigo_Pet"]),
                        Raca = reader["Raca"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Porte = reader["Porte"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Idade = Convert.ToInt32(reader["Idade"]),
                        Codigo_Plano = reader["Codigo_Plano"] as int?,
                        Codigo_Usuario = reader["Codigo_Usuario"] as int?
                    });
                }
            }

            return lista;
        }

        // BUSCAR POR NOME
        public List<Pet> BuscarPorNome(string nome)
        {
            List<Pet> lista = new();

            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"SELECT Codigo_Pet, Raca, Tipo, Porte, Nome, Idade, Codigo_Plano, Codigo_Usuario
                               FROM tbPet
                               WHERE Nome LIKE @nome";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Pet
                    {
                        Codigo_Pet = Convert.ToInt32(reader["Codigo_Pet"]),
                        Raca = reader["Raca"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        Porte = reader["Porte"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Idade = Convert.ToInt32(reader["Idade"]),
                        Codigo_Plano = reader["Codigo_Plano"] as int?,
                        Codigo_Usuario = reader["Codigo_Usuario"] as int?
                    });
                }
            }

            return lista;
        }

        // EXCLUIR PET
        public void ExcluirPet(int id)
        {
            using (var con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"DELETE FROM tbPet WHERE Codigo_Pet = @id";

                MySqlCommand cmd = new(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}