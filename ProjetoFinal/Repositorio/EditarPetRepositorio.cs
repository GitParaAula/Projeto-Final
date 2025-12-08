using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class EditarPetRepositorio
    {
        private readonly string _conexao;

        public EditarPetRepositorio(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("conexaoMySQL");
        }

        public List<Pet> ListarPets(string nome)
        {
            List<Pet> lista = new List<Pet>();

            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"SELECT Codigo_Pet, Nome, Tipo, Raca, Porte, Idade
                               FROM tbPet
                               WHERE Nome LIKE @nome";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Pet
                        {
                            Codigo_Pet = dr.GetInt32("Codigo_Pet"),
                            Nome = dr.GetString("Nome"),
                            Tipo = dr.GetString("Tipo"),
                            Raca = dr.GetString("Raca"),
                            Porte = dr.GetString("Porte"),
                            Idade = dr.GetInt32("Idade")
                        });
                    }
                }
            }

            return lista;
        }

        public Pet BuscarPorId(int id)
        {
            Pet pet = new Pet();

            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"SELECT Codigo_Pet, Nome, Tipo, Raca, Porte, Idade
                               FROM tbPet
                               WHERE Codigo_Pet = @id";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pet.Codigo_Pet = dr.GetInt32("Codigo_Pet");
                        pet.Nome = dr.GetString("Nome");
                        pet.Tipo = dr.GetString("Tipo");
                        pet.Raca = dr.GetString("Raca");
                        pet.Porte = dr.GetString("Porte");
                        pet.Idade = dr.GetInt32("Idade");
                    }
                }
            }

            return pet;
        }

        public void AtualizarPet(Pet pet)
        {
            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"UPDATE tbPet
                               SET Nome = @nome,
                                   Tipo = @tipo,
                                   Raca = @raca,
                                   Porte = @porte,
                                   Idade = @idade
                               WHERE Codigo_Pet = @id";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@nome", pet.Nome);
                cmd.Parameters.AddWithValue("@tipo", pet.Tipo);
                cmd.Parameters.AddWithValue("@raca", pet.Raca);
                cmd.Parameters.AddWithValue("@porte", pet.Porte);
                cmd.Parameters.AddWithValue("@idade", pet.Idade);
                cmd.Parameters.AddWithValue("@id", pet.Codigo_Pet);

                cmd.ExecuteNonQuery();
            }
        }
        public List<PetComPlanoViewModel> ListarPetsComPlanoPorUsuario(int codigoUsuario)
        {
            List<PetComPlanoViewModel> lista = new List<PetComPlanoViewModel>();

            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
            SELECT 
                p.Codigo_Pet, p.Nome AS NomePet, p.Tipo, p.Raca, p.Idade, p.Porte,
                pl.Codigo_Plano, pl.Nome AS NomePlano, pl.Valor, pl.Duracao
            FROM tbPet p
            INNER JOIN tbPlano pl ON p.Codigo_Plano = pl.Codigo_Plano
            WHERE p.Codigo_Usuario = @codigoUsuario";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@codigoUsuario", codigoUsuario);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PetComPlanoViewModel
                        {
                            Codigo_Pet = dr.GetInt32("Codigo_Pet"),
                            NomePet = dr.GetString("NomePet"),
                            Tipo = dr.GetString("Tipo"),
                            Raca = dr.GetString("Raca"),
                            Idade = dr.GetInt32("Idade"),
                            Porte = dr.GetString("Porte"),
                            Codigo_Plano = dr.GetInt32("Codigo_Plano"),
                            NomePlano = dr.GetString("NomePlano"),
                            Valor = dr.GetDecimal("Valor"),
                            Duracao = dr.GetDateTime("Duracao")
                        });
                    }
                }
            }

            return lista;
        }
    }
}