using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PigWebApplication.Models;

namespace PigWebApplication.Controllers
{
    public class InjectionsController : Controller
    {
        private readonly DbpigsContext _context;

        public InjectionsController(DbpigsContext context)
        {
            _context = context;
        }

        // GET: Injections
        public async Task<IActionResult> Index(int? id, string? name)
        {

            if (id == null) return RedirectToAction("Index", "Medicines");
            ViewBag.MedicineId = id;
            ViewBag.MedicineName = name;
            var injectionsByMedicine = _context.Injections.Where(i => i.MedicineId == id).Include(i => i.Medicine).Include(i => i.Pig);

            return View(await injectionsByMedicine.ToListAsync());
        }

        // GET: Injections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Injections == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections
                .Include(i => i.Medicine)
                .Include(i => i.Pig)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (injection == null)
            {
                return NotFound();
            }

            return View(injection);
        }

        // GET: Injections/Create
        public IActionResult Create(int medicineId)
        {
            //ViewData["MedicineId"] = new SelectList(_context.Medicines, "Id", "Id");
            ViewBag.MedicineId = medicineId;
            ViewBag.MedicineName = _context.Medicines.Where(m => m.Id == medicineId).FirstOrDefault().Name;
            
            
            ViewData["PigId"] = new SelectList(_context.Pigs, "Id", "Id");
            return View();
        }

        // POST: Injections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int medicineId, [Bind("Id,Date,MedicineId,PigId,Note")] Injection injection)
        {
            injection.MedicineId = medicineId;
            if (ModelState.IsValid)
            {
                _context.Add(injection);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Medicines", new { id = medicineId, name = _context.Medicines.Where(m => m.Id == medicineId).FirstOrDefault().Name });
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["MedicineId"] = new SelectList(_context.Medicines, "Id", "Id", injection.MedicineId);
            ViewData["PigId"] = new SelectList(_context.Pigs, "Id", "Id", injection.PigId);
            return RedirectToAction("Index", "Medicines", new { id = medicineId, name = _context.Medicines.Where(m => m.Id == medicineId).FirstOrDefault().Name });
        }

        // GET: Injections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Injections == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections.FindAsync(id);
            if (injection == null)
            {
                return NotFound();
            }
            ViewData["MedicineId"] = new SelectList(_context.Medicines, "Id", "Id", injection.MedicineId);
            ViewData["PigId"] = new SelectList(_context.Pigs, "Id", "Id", injection.PigId);
            return View(injection);
        }

        // POST: Injections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,MedicineId,PigId,Note")] Injection injection)
        {
            if (id != injection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(injection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InjectionExists(injection.Id))
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
            ViewData["MedicineId"] = new SelectList(_context.Medicines, "Id", "Id", injection.MedicineId);
            ViewData["PigId"] = new SelectList(_context.Pigs, "Id", "Id", injection.PigId);
            return View(injection);
        }

        // GET: Injections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Injections == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections
                .Include(i => i.Medicine)
                .Include(i => i.Pig)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (injection == null)
            {
                return NotFound();
            }

            return View(injection);
        }

        // POST: Injections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Injections == null)
            {
                return Problem("Entity set 'DbpigsContext.Injections'  is null.");
            }
            var injection = await _context.Injections.FindAsync(id);
            if (injection != null)
            {
                _context.Injections.Remove(injection);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InjectionExists(int id)
        {
          return _context.Injections.Any(e => e.Id == id);
        }
    }
}
