using MySql.Data.MySqlClient;

namespace ProjetoFinal.Repositorio
{
    public class AtribuirPlanoRepositorio
    {
        private readonly string _conexao;

        public AtribuirPlanoRepositorio(IConfiguration config)
        {
            _conexao = config.GetConnectionString("conexaoMySQL");
        }

        public string AtribuirPlano(int codigoPet, int codigoPlano)
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
                    return "O código do pet está incorreto.";

                // Verificar plano
                string sqlPlano = "SELECT COUNT(*) FROM tbPlano WHERE Codigo_Plano = @plano";
                MySqlCommand cmdPlano = new MySqlCommand(sqlPlano, conn);
                cmdPlano.Parameters.AddWithValue("@plano", codigoPlano);

                long planoExiste = (long)cmdPlano.ExecuteScalar();

                if (planoExiste == 0)
                    return "O código do plano está incorreto.";

                // Atualizar pet com o plano
                string sqlUpdate =
                    "UPDATE tbPet SET Codigo_Plano = @plano WHERE Codigo_Pet = @pet";

                MySqlCommand cmdUpdate = new MySqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@plano", codigoPlano);
                cmdUpdate.Parameters.AddWithValue("@pet", codigoPet);

                cmdUpdate.ExecuteNonQuery();

                return "Plano atribuído ao pet com sucesso!";
            }
        }
    }
}