using MySql.Data.MySqlClient;
using ProjetoFinal.Models;

namespace ProjetoFinal.Repositorio
{
    public class ConsultarPetRepositorio
    {
        private readonly string _conexao;

        public ConsultarPetRepositorio(IConfiguration configuration)
        {
            _conexao = configuration.GetConnectionString("conexaoMySQL");
        }

        public List<PetConsultaViewModel> ListarPets(string nome)
        {
            var lista = new List<PetConsultaViewModel>();

            using (MySqlConnection con = new MySqlConnection(_conexao))
            {
                con.Open();

                string sql = @"
                    SELECT 
                        p.Raca,
                        p.Tipo,
                        p.Nome AS NomePet,
                        p.Idade,
                        u.Nome AS NomeDono,
                        pl.Nome AS NomePlano
                    FROM tbPet p
                    LEFT JOIN tbUsuario u ON p.Codigo_Usuario = u.Codigo_Usuario
                    LEFT JOIN tbPlano pl ON p.Codigo_Plano = pl.Codigo_Plano
                    WHERE p.Nome LIKE @nome
                ";

                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PetConsultaViewModel
                        {
                            Raca = dr.GetString("Raca"),
                            Tipo = dr.GetString("Tipo"),
                            NomePet = dr.GetString("NomePet"),
                            Idade = dr.GetInt32("Idade"),
                            NomeDono = dr.IsDBNull(dr.GetOrdinal("NomeDono")) ? "-" : dr.GetString("NomeDono"),
                            NomePlano = dr.IsDBNull(dr.GetOrdinal("NomePlano")) ? "-" : dr.GetString("NomePlano")
                        });
                    }
                }
            }

            return lista;
        }
    }

    // ViewModel para a consulta
    public class PetConsultaViewModel
    {
        public string? Raca { get; set; }
        public string? Tipo { get; set; }
        public string? NomePet { get; set; }
        public int Idade { get; set; }
        public string? NomeDono { get; set; }
        public string? NomePlano { get; set; }
    }
}