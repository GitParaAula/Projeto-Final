using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Models
{
    public class Usuario
    {
        public int Codigo_Usuario { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public int Idade { get; set; }
        public string? Email { get; set; }
        public Endereco Endereco { get; set; }
    }
}
