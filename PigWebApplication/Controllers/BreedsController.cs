using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PigWebApplication.Models;
using ClosedXML.Excel;

namespace PigWebApplication.Controllers
{
    [Authorize(Roles = "admin, worker")]
    public class BreedsController : Controller
    {
        private readonly DbpigsContext _context;

        public BreedsController(DbpigsContext context)
        {
            _context = context;
        }

        // GET: Breeds
        public async Task<IActionResult> Index()
        {
              return View(await _context.Breeds.ToListAsync());
        }

        // GET: Breeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Breeds == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (breed == null)
            {
                return NotFound();
            }

            //return View(breed);
            return RedirectToAction("Index", "Pigs", new {id = breed.Id, name = breed.Name});
        }

        // GET: Breeds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Breeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Direction")] Breed breed)
        {
            if (ModelState.IsValid)
            {
                _context.Add(breed);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(breed);
        }

        // GET: Breeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Breeds == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds.FindAsync(id);
            if (breed == null)
            {
                return NotFound();
            }
            return View(breed);
        }

        // POST: Breeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Direction")] Breed breed)
        {
            if (id != breed.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(breed);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BreedExists(breed.Id))
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
            return View(breed);
        }

        // GET: Breeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Breeds == null)
            {
                return NotFound();
            }

            var breed = await _context.Breeds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (breed == null)
            {
                return NotFound();
            }

            return View(breed);
        }

        // POST: Breeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Breeds == null)
            {
                return Problem("Entity set 'DbpigsContext.Breeds'  is null.");
            }
            var breed = await _context.Breeds.FindAsync(id);
            if (breed != null)
            {
                _context.Breeds.Remove(breed);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BreedExists(int id)
        {
          return _context.Breeds.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Breed newbreed;
                                var breeds = (from breed in _context.Breeds
                                         where breed.Name.Contains(worksheet.Name)
                                         select breed).ToList();
                                if (breeds.Count > 0)
                                {
                                    newbreed = breeds[0];
                                }
                                else
                                {
                                    newbreed = new Breed();
                                    newbreed.Name = worksheet.Name;
                                    newbreed.Direction = "from EXCEL";
                                    //додати в контекст
                                    _context.Breeds.Add(newbreed);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Pig pig = new Pig();
                                        pig.Gender = (short)row.Cell(1).Value;
                                        pig.BirthDate = row.Cell(2).Value;
                                        pig.Note = row.Cell(3).Value.ToString();
                                        pig.Breed = newbreed;
                                        _context.Pigs.Add(pig);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var breeds = _context.Breeds.Include("Pigs").ToList();

                foreach (var b in breeds)
                {
                    var worksheet = workbook.Worksheets.Add(b.Name);
                    worksheet.Cell("A1").Value = "Number";
                    worksheet.Cell("B1").Value = "Gender";
                    worksheet.Cell("C1").Value = "Birthdate";
                    worksheet.Cell("D1").Value = "Note";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var pigs = b.Pigs.ToList();

                    for (int i = 0; i < pigs.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = pigs[i].Id;
                        worksheet.Cell(i + 2, 2).Value = pigs[i].Gender;
                        worksheet.Cell(i + 2, 3).Value = pigs[i].BirthDate;
                        worksheet.Cell(i + 2, 4).Value = pigs[i].Note;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"pigs_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };

                }
            }
        }

    }
}
