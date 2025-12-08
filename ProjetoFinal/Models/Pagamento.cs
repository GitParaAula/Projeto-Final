using Microsoft.AspNetCore.Mvc;

namespace ProjetoFinal.Models
{
    public class Pagamento
    {
        public int Codigo_Pagamento { get; set; }
        public string? Titular { get; set; }
        public decimal Valor { get; set; }
        public string? MetodoPagamento { get; set; }
        public int Codigo_Usuario { get; set; }
    }
}