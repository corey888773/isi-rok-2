using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcUzytkownik.Data;
using Silownia.Models;

namespace Silownia.Controllers
{
    public class WejscieController : Controller
    {
        private readonly MvcUzytkownikContext _context;

        public WejscieController(MvcUzytkownikContext context)
        {
            _context = context;
        }

        // GET: Wejscie
        public async Task<IActionResult> Index()
        {
            return _context.Wejscie != null ? 
                        View(await _context.Wejscie.ToListAsync()) :
                        Problem("Entity set 'MvcUzytkownikContext.Wejscie'  is null.");
        }

        // GET: Wejscie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wejscie == null)
            {
                return NotFound();
            }

            var wejscie = await _context.Wejscie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wejscie == null)
            {
                return NotFound();
            }

            return View(wejscie);
        }

        // GET: Wejscie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wejscie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,CzasTrwania")] Wejscie wejscie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wejscie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wejscie);
        }

        // GET: Wejscie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wejscie == null)
            {
                return NotFound();
            }

            var wejscie = await _context.Wejscie.FindAsync(id);
            if (wejscie == null)
            {
                return NotFound();
            }
            return View(wejscie);
        }

        // POST: Wejscie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,CzasTrwania")] Wejscie wejscie)
        {
            if (id != wejscie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wejscie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WejscieExists(wejscie.Id))
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
            return View(wejscie);
        }

        // GET: Wejscie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wejscie == null)
            {
                return NotFound();
            }

            var wejscie = await _context.Wejscie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wejscie == null)
            {
                return NotFound();
            }

            return View(wejscie);
        }

        // POST: Wejscie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wejscie == null)
            {
                return Problem("Entity set 'MvcWejscieContext.Wejscie'  is null.");
            }
            var wejscie = await _context.Wejscie.FindAsync(id);
            if (wejscie != null)
            {
                _context.Wejscie.Remove(wejscie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WejscieExists(int id)
        {
          return (_context.Wejscie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
