namespace ProjetoFinal.Models
{
    public class HistoricoCadastro
    {
        public int Codigo_HistoricoCadastro { get; set; }
        public int Codigo_Funcionario { get; set; }
        public string TipoCadastro { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
