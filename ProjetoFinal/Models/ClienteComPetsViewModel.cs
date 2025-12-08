using System;
using System.Collections.Generic;

namespace ProjetoFinal.Models
{
    public class ClienteComPetsViewModel
    {
        public Usuario Cliente { get; set; }
        public List<PetComPlanoViewModel> Pets { get; set; } = new List<PetComPlanoViewModel>();
    }

    public class PetComPlanoViewModel
    {
        public int Codigo_Pet { get; set; }
        public string NomePet { get; set; }
        public string Tipo { get; set; }
        public string Raca { get; set; }
        public int Idade { get; set; }
        public string Porte { get; set; }

        public int Codigo_Plano { get; set; }
        public string NomePlano { get; set; }
        public decimal Valor { get; set; }
        public DateTime Duracao { get; set; }
    }
}