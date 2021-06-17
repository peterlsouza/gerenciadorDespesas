using GerenciadorDespesas.Models;
using GerenciadorDespesas.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDespesas.ViewComponents
{
    public class EstatisticasViewComponent : ViewComponent
    {
        private readonly MyDbContext _context;

        public EstatisticasViewComponent(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            EstatisticasViewModel estatisticas = new EstatisticasViewModel();

            estatisticas.MenorDespesa = _context.Despesas.Min(x => x.Valor);
            estatisticas.MaiorDespesa = _context.Despesas.Max(x => x.Valor);
            estatisticas.QuantidadeDespesas = _context.Despesas.Count();

            return View(estatisticas);
        }
    }
}