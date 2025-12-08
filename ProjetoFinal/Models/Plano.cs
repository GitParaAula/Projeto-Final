namespace ProjetoFinal.Models
{
    public class Plano
    {
        public int Codigo_Plano { get; set; }
        public string? Nome { get; set; }
        public decimal Valor { get; set; }
        public DateTime Duracao { get; set; }
        public string? Requisitos { get; set; }
    }
}