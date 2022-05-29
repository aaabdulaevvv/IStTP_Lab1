#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApplication1;
using Microsoft.Data.SqlClient;

namespace LibraryWebApplication1.Controllers
{
    public class CitiesController : Controller
    {
        private readonly DBLibraryContext _context;

        public CitiesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index(string? id, string? name, int population)
        {
            if (name == null)
            {
                var dBLibraryContext = _context.Cities.Include(g => g.CiCountryNavigation);
                return View(await dBLibraryContext.ToListAsync());
            }
            if (id == null) return RedirectToAction("Countries", "Index");
            ViewBag.CountryId = id;
            ViewBag.CountryName = name;
            ViewBag.CountryPopulation = population;
            var citiesByCountries = _context.Cities.Where(b=>b.CiCountry == id).Include(b=>b.CiCountryNavigation);
            return View(await citiesByCountries.ToListAsync());
        }

        public async Task<IActionResult> Index1()
        {
            var dBLibraryContext = _context.Cities.Include(c => c.CiCountryNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery1()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery1(int pop)
        {
            //var dBLibraryContext = _context.Cities.FromSqlRaw("SELECT * FROM City WHERE City.CI_Population > {0}", pop);
            var dBLibraryContext = _context.Cities.FromSqlRaw("SELECT * FROM City WHERE (SELECT COUNT (Stadium.ST_ID) FROM Stadium WHERE Stadium.ST_City=City.CI_ID) > {0}", pop-1);
            return View(await dBLibraryContext.ToListAsync());
        }
        public async Task<IActionResult> CreateQuery4()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery4(int cnt1, int cnt2)
        {
            var dBLibraryContext = _context.Cities.FromSqlRaw("SELECT * FROM City WHERE (SELECT COUNT (Team.T_ID) FROM Team WHERE (SELECT COUNT (Game.G_ID) FROM Game WHERE Game.G_TeamH=Team.T_ID OR Game.G_TeamA=Team.T_ID) >= {1} AND Team.T_City=City.CI_ID) >= {0}", cnt1, cnt2);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> IndexCo()
        {
            return RedirectToAction("Index", "Countries");
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.CiCountryNavigation)
                .FirstOrDefaultAsync(m => m.CiId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        public async Task<IActionResult> DetailsCo(string? id)
        {
            return RedirectToAction("Details", "Countries", new { id = id });
        }

        public async Task<IActionResult> ExportCo()
        {
            return RedirectToAction("Export", "Countries");
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewData["CiCountry"] = new SelectList(_context.Countries, "CoId", "CoName");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CiName,CiCountry,CiPopulation,CiId")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CiCountry"] = new SelectList(_context.Countries, "CoId", "CoId", city.CiCountry);
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["CiCountry"] = new SelectList(_context.Countries, "CoId", "CoName", city.CiCountry);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CiName,CiCountry,CiPopulation,CiId")] City city)
        {
            if (id != city.CiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CiId))
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
            ViewData["CiCountry"] = new SelectList(_context.Countries, "CoId", "CoId", city.CiCountry);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.CiCountryNavigation)
                .FirstOrDefaultAsync(m => m.CiId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index1));
        }

        private bool CityExists(string id)
        {
            return _context.Cities.Any(e => e.CiId == id);
        }
    }
}
