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
    public class KarnetController : Controller
    {
        private readonly MvcUzytkownikContext _context;

        public KarnetController(MvcUzytkownikContext context)
        {
            _context = context;
        }

        // GET: Karnet
        public async Task<IActionResult> Index()
        {
            return _context.Karnet != null ? 
                        View(await _context.Karnet.ToListAsync()) :
                        Problem("Entity set 'MvcUzytkownikContext.Karnet'  is null.");
        }

        // GET: Karnet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Karnet == null)
            {
                return NotFound();
            }

            var karnet = await _context.Karnet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karnet == null)
            {
                return NotFound();
            }

            return View(karnet);
        }

        // GET: Karnet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Karnet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Cena,Opis,CzasTrwaniaWMiesiacach")] Karnet karnet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(karnet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(karnet);
        }

        // GET: Karnet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Karnet == null)
            {
                return NotFound();
            }

            var karnet = await _context.Karnet.FindAsync(id);
            if (karnet == null)
            {
                return NotFound();
            }
            return View(karnet);
        }

        // POST: Karnet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Cena,Opis,CzasTrwaniaWMiesiacach")] Karnet karnet)
        {
            if (id != karnet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(karnet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KarnetExists(karnet.Id))
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
            return View(karnet);
        }

        // GET: Karnet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Karnet == null)
            {
                return NotFound();
            }

            var karnet = await _context.Karnet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (karnet == null)
            {
                return NotFound();
            }

            return View(karnet);
        }

        // POST: Karnet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Karnet == null)
            {
                return Problem("Entity set 'MvcKarnetContext.Karnet'  is null.");
            }
            var karnet = await _context.Karnet.FindAsync(id);
            if (karnet != null)
            {
                _context.Karnet.Remove(karnet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KarnetExists(int id)
        {
          return (_context.Karnet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
