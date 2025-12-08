using System.Numerics;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Models
{
    public class Funcionario
    {
        public int Codigo_funcionario { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Rg { get; set; }

    }
}
