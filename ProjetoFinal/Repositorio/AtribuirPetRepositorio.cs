using MySql.Data.MySqlClient;

namespace ProjetoFinal.Repositorio
{
    public class AtribuirPetRepositorio
    {
        private readonly string _conexao;

        public AtribuirPetRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        public string AtribuirPet(int codigoPet, int codigoUsuario)
        {
            using (MySqlConnection conn = new MySqlConnection(_conexao))
            {
                conn.Open();

                // Verificar pet
                string sqlPet = "SELECT COUNT(*) FROM tbPet WHERE Codigo_Pet = @pet";
                MySqlCommand cmdPet = new MySqlCommand(sqlPet, conn);
                cmdPet.Parameters.AddWithValue("@pet", codigoPet);

                long petExiste = (long)cmdPet.ExecuteScalar();

                if (petExiste == 0)
                    return "Pet não encontrado.";

                // Verificar dono
                string sqlDono = "SELECT COUNT(*) FROM tbUsuario WHERE Codigo_Usuario = @dono";
                MySqlCommand cmdDono = new MySqlCommand(sqlDono, conn);
                cmdDono.Parameters.AddWithValue("@dono", codigoUsuario);

                long donoExiste = (long)cmdDono.ExecuteScalar();

                if (donoExiste == 0)
                    return "Dono não encontrado.";

                // Atualizar pet
                string sqlUpdate =
                    "UPDATE tbPet SET Codigo_Usuario = @dono WHERE Codigo_Pet = @pet";

                MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@dono", codigoUsuario);
                cmdUpdate.Parameters.AddWithValue("@pet", codigoPet);

                cmdUpdate.ExecuteNonQuery();

                return "Pet atribuído ao dono com sucesso!";
            }
        }
    }
}