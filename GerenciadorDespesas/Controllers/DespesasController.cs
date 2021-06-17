using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorDespesas.Models;
using X.PagedList;
using GerenciadorDespesas.ViewModels;

namespace GerenciadorDespesas.Controllers
{
    public class DespesasController : Controller
    {
        private readonly MyDbContext _context;

        public DespesasController(MyDbContext context)
        {
            _context = context;
        }

     
        public async Task<IActionResult> Index(int? pagina)
        {
            const int itensPagina = 8;
            int numeroPagina = (pagina ?? 1); //se tiver algum valor, será o numero da pagina, senão será 1

            ViewData["Meses"] = new SelectList(_context.Meses.Where(x => x.MesId == x.Salarios.MesId), "MesId", "Nome");
            
            var myDbContext = _context.Despesas.Include(d => d.Meses).Include(d => d.TipoDespesas).OrderBy(d => d.MesId);
            return View(await myDbContext.ToPagedListAsync(numeroPagina, itensPagina));
        }

       

        // GET: Despesas/Create
        public IActionResult Create()
        {
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome");
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DespesaId,MesId,TipoDespesaId,Valor")] Despesas despesas)
        {
            if (ModelState.IsValid)
            {
                TempData["Confirmacao"] = "Despesa cadastrada com sucesso!";
                _context.Add(despesas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }

        // GET: Despesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var despesas = await _context.Despesas.FindAsync(id);
            if (despesas == null)
            {
                return NotFound();
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DespesaId,MesId,TipoDespesaId,Valor")] Despesas despesas)
        {
            if (id != despesas.DespesaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(despesas);
                    await _context.SaveChangesAsync();
                    TempData["Confirmacao"] = "Despesa atualizada com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DespesasExists(despesas.DespesaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(_context.Meses, "MesId", "Nome", despesas.MesId);
            ViewData["TipoDespesaId"] = new SelectList(_context.TipoDespesas, "TipoDespesaId", "Nome", despesas.TipoDespesaId);
            return View(despesas);
        }

        
        // POST: Despesas/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var despesas = await _context.Despesas.FindAsync(id);
            _context.Despesas.Remove(despesas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DespesasExists(int id)
        {
            return _context.Despesas.Any(e => e.DespesaId == id);
        }

        public JsonResult GastosTotaisMes(int mesId)
        {
            GastosTotaisMesViewModel gastos = new GastosTotaisMesViewModel();

            gastos.ValorTotalGasto = _context.Despesas.Where(d => d.Meses.MesId == mesId).Sum(d => d.Valor);
            gastos.Salario = _context.Salarios.Where(s => s.Meses.MesId == mesId).Select(s => s.Valor).FirstOrDefault();

            return Json(gastos);
        }

        public JsonResult GastoMes(int mesId)
        {
            var query = from despesas in _context.Despesas
                        where despesas.Meses.MesId == mesId
                        group despesas by despesas.TipoDespesas.Nome into g
                        select new
                        {
                            TiposDespesas = g.Key,
                            Valores = g.Sum(d => d.Valor)
                        };
            return Json(query);
        }

        public JsonResult GastosTotais()
        {
            var query = _context.Despesas
                .OrderBy(m => m.Meses.MesId)
                .GroupBy(m => m.Meses.Nome)
                .Select(d => new { NomeMeses = d.Key, Valores = d.Sum(x => x.Valor) });

            return Json(query);
        }
    }
}
