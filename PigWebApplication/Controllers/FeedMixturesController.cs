using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PigWebApplication.Models;

namespace PigWebApplication.Controllers
{
    [Authorize(Roles = "admin, worker")]
    public class FeedMixturesController : Controller
    {
        private readonly DbpigsContext _context;

        public FeedMixturesController(DbpigsContext context)
        {
            _context = context;
        }

        // GET: FeedMixtures
        public async Task<IActionResult> Index()
        {
              return View(await _context.FeedMixtures.ToListAsync());
        }

        // GET: FeedMixtures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FeedMixtures == null)
            {
                return NotFound();
            }

            var feedMixture = await _context.FeedMixtures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedMixture == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "FeedTypes", new { id = feedMixture.Id, name = feedMixture.Name});
        }

        // GET: FeedMixtures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedMixtures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Wheet,Barley,Corn,Pea,Oilcake")] FeedMixture feedMixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedMixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedMixture);
        }

        // GET: FeedMixtures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FeedMixtures == null)
            {
                return NotFound();
            }

            var feedMixture = await _context.FeedMixtures.FindAsync(id);
            if (feedMixture == null)
            {
                return NotFound();
            }
            return View(feedMixture);
        }

        // POST: FeedMixtures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Wheet,Barley,Corn,Pea,Oilcake")] FeedMixture feedMixture)
        {
            if (id != feedMixture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedMixture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedMixtureExists(feedMixture.Id))
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
            return View(feedMixture);
        }

        // GET: FeedMixtures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FeedMixtures == null)
            {
                return NotFound();
            }

            var feedMixture = await _context.FeedMixtures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedMixture == null)
            {
                return NotFound();
            }

            return View(feedMixture);
        }

        // POST: FeedMixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FeedMixtures == null)
            {
                return Problem("Entity set 'DbpigsContext.FeedMixtures'  is null.");
            }
            var feedMixture = await _context.FeedMixtures.FindAsync(id);
            if (feedMixture != null)
            {
                _context.FeedMixtures.Remove(feedMixture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedMixtureExists(int id)
        {
          return _context.FeedMixtures.Any(e => e.Id == id);
        }
    }
}
