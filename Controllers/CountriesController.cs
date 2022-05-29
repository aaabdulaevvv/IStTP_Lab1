#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApplication1;
using ClosedXML.Excel;

namespace LibraryWebApplication1.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DBLibraryContext _context;

        public CountriesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CoId == id);
            if (country == null)
            {
                return NotFound();
            }

            // return View(country);
            return RedirectToAction("Index", "Cities", new { id = country.CoId, name = country.CoName, population = country.CoPopulation });
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoName,CoPopulation,CoId")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CoName,CoPopulation,CoId")] Country country)
        {
            if (id != country.CoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CoId))
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
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.CoId == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(string id)
        {
            return _context.Countries.Any(e => e.CoId == id);
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
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Country newcou;
                                var c = (from cou in _context.Countries
                                         where cou.CoId.Contains(worksheet.Name)
                                         select cou).ToList();
                                if (c.Count > 0)
                                {
                                    newcou = c[0];
                                }
                                else
                                {
                                    newcou = new Country();
                                    newcou.CoId = worksheet.Name;
                                    newcou.CoName = "from EXCEL";
                                    newcou.CoPopulation = 0;
                                    //додати в контекст
                                    _context.Countries.Add(newcou);
                                }
                                //перегляд усіх рядків
                                foreach (IXLRow row in worksheet.RowsUsed())
                                {
                                    try
                                    {
                                        City city = new City();
                                        city.CiId = row.Cell(1).Value.ToString();
                                        city.CiName = row.Cell(2).Value.ToString();
                                        city.CiPopulation = Int32.Parse(row.Cell(3).Value.ToString());

                                        city.CiCountryNavigation = newcou;
                                        _context.Cities.Add(city);
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
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
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var countries = _context.Countries.Include("Cities").ToList();
                foreach (var c in countries)
                {
                    var worksheet = workbook.Worksheets.Add(c.CoId);
                    worksheet.Cell("A1").Value = "Код міста";
                    worksheet.Cell("B1").Value = "Назва міста" ;
                    worksheet.Cell("C1").Value = "Населення" ;
                    worksheet.Cell("D1").Value = "Країна" ;
                    worksheet.Row(1).Style.Font.Bold = true;
                    var cities = c.Cities.ToList();
                    for (int i = 0; i < cities.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = cities[i].CiId;
                        worksheet.Cell(i + 2, 2).Value = cities[i].CiName;
                        worksheet.Cell(i + 2, 3).Value = cities[i].CiPopulation;
                        worksheet.Cell(i + 2, 4).Value = cities[i].CiCountryNavigation.CoName;
                    }
                }
                using (var stream = new MemoryStream())

                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"countries_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
