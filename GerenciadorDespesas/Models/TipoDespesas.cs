using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDespesas.Models
{
    public class TipoDespesas
    {
        public int TipoDespesaId { get; set; }
        
        [Required(ErrorMessage = "Campo {0} Obrigatório!")]
        [StringLength(50, ErrorMessage = "O campo de ter entre {2} e {1} caracteres", MinimumLength = 3)]
        [Remote("TipoDespesaExiste", "TipoDespesas")]
        public string Nome { get; set; }

        public ICollection<Despesas> Despesas { get; set; }
    }
}
