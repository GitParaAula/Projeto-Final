using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Models
{
    public class Endereco
    {
        public int Codigo_Endereco { get; set; }
        public string? Rua { get; set; }
        public int Numero { get; set; }
        public string? Complemento { get; set; }
    }
}
