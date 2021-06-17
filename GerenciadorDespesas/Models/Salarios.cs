using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDespesas.Models
{
    public class Salarios
    {
        public int SalarioId { get; set; }
        public int MesId { get; set; }
        public Meses Meses { get; set; }

        [Required(ErrorMessage ="Campo {0} obrigatório!")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor Inválido!")]
        public double Valor { get; set; }
    }
}
