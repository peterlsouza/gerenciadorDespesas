using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciadorDespesas.Models;

namespace GerenciadorDespesas.Controllers
{
    public class SalariosController : Controller
    {
        private readonly MyDbContext _context;

        public SalariosController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Salarios.Include(s => s.Meses).OrderBy(m => m.MesId);
            return View(await myDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string txtProcurar)
        {
            if(!String.IsNullOrEmpty(txtProcurar))
            {
                return View(await _context.Salarios.Include(
                    s => s.Meses).Where(m => m.Meses.Nome.ToUpper().Contains(txtProcurar.ToUpper())).ToListAsync());
                //include vai fazer tipo join... 
            }

            return View(await _context.Salarios.Include(s => s.Meses).ToListAsync());
        }


        // GET: Salarios/Create
        public IActionResult Create()
        {
            ViewData["MesId"] = new SelectList(_context.Meses.Where(s => s.MesId != s.Salarios.MesId), "MesId", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalarioId,MesId,Valor")] Salarios salarios)
        {
            if (ModelState.IsValid)
            {
                TempData["Confirmacao"] = "Salário cadastrado com sucesso";
                _context.Add(salarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MesId"] = new SelectList(_context.Meses.Where(s => s.MesId != s.Salarios.MesId), "MesId", "Nome", salarios.MesId);
            return View(salarios);
        }

        // GET: Salarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salarios = await _context.Salarios.FindAsync(id);
            if (salarios == null)
            {
                return NotFound();
            }
            ViewData["MesId"] = new SelectList(_context.Meses.Where(s => s.MesId == salarios.MesId), "MesId", "Nome", salarios.MesId);
            return View(salarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalarioId,MesId,Valor")] Salarios salarios)
        {
            if (id != salarios.SalarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salarios);
                    await _context.SaveChangesAsync();
                    TempData["Confirmacao"] = "Salário atualizado com sucesso";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalariosExists(salarios.SalarioId))
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
            ViewData["MesId"] = new SelectList(_context.Meses.Where(s => s.MesId == salarios.MesId), "MesId", "Nome", salarios.MesId);
            return View(salarios);
        }

        

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var salarios = await _context.Salarios.FindAsync(id);
            _context.Salarios.Remove(salarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalariosExists(int id)
        {
            return _context.Salarios.Any(e => e.SalarioId == id);
        }
    }
}
