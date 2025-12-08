using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Models
{
    public class Pet
    {
        public int Codigo_Pet { get; set; }
        public string? Raca { get; set; }
        public string? Tipo { get; set; }
        public string? Porte { get; set; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public int? Codigo_Plano { get; set; }
        public int? Codigo_Usuario { get; set; }
    }
}
