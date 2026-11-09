using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DateExample.DataModel;

namespace DotNet7MVCDateExample.Controllers
{
    public class LocalDatesController : Controller
    {
        private readonly DateContext _context;

        public LocalDatesController(DateContext context)
        {
            _context = context;
        }

        // GET: LocalDates
        public async Task<IActionResult> Index()
        {
              return _context.Dates != null ? 
                          View(await _context.Dates.ToListAsync()) :
                          Problem("Entity set 'DateContext.Dates'  is null.");
        }

        // GET: LocalDates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dates == null)
            {
                return NotFound();
            }

            var localDate = await _context.Dates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (localDate == null)
            {
                return NotFound();
            }

            return View(localDate);
        }

        // GET: LocalDates/Create
        public IActionResult Create()
        {
            // Need to populate an entity otherwise view does not 
            LocalDate d = new LocalDate
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,

            };
            return View(d);
        }

        // POST: LocalDates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StartDate,EndDate")] LocalDate localDate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localDate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localDate);
        }

        // GET: LocalDates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dates == null)
            {
                return NotFound();
            }

            var localDate = await _context.Dates.FindAsync(id);
            if (localDate == null)
            {
                return NotFound();
            }
            return View(localDate);
        }

        // POST: LocalDates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StartDate,EndDate")] LocalDate localDate)
        {
            if (id != localDate.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localDate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalDateExists(localDate.ID))
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
            return View(localDate);
        }

        // GET: LocalDates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dates == null)
            {
                return NotFound();
            }

            var localDate = await _context.Dates
                .FirstOrDefaultAsync(m => m.ID == id);
            if (localDate == null)
            {
                return NotFound();
            }

            return View(localDate);
        }

        // POST: LocalDates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dates == null)
            {
                return Problem("Entity set 'DateContext.Dates'  is null.");
            }
            var localDate = await _context.Dates.FindAsync(id);
            if (localDate != null)
            {
                _context.Dates.Remove(localDate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalDateExists(int id)
        {
          return (_context.Dates?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
