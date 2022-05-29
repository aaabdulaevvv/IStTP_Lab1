#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApplication1;

namespace LibraryWebApplication1.Controllers
{
    public class StadiaController : Controller
    {
        private readonly DBLibraryContext _context;

        public StadiaController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Stadia
        public async Task<IActionResult> Index()
        {
            var dBLibraryContext = _context.Stadia.Include(s => s.StCityNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery5()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery5(int cnt)
        {
            var dBLibraryContext = _context.Stadia.FromSqlRaw("SELECT * FROM Stadium WHERE (SELECT COUNT (Team.T_ID) FROM Team WHERE EXISTS (SELECT Game.G_ID FROM Game WHERE Game.G_Stadium=Stadium.ST_ID AND (Game.G_TeamH=Team.T_ID OR Game.G_TeamA=Team.T_ID))) = {0}", cnt);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery7()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery7(string id)
        {
            //Знайти стадіони, на яких грали всі команди міста Х і обов'язково ще якісь
            var dBLibraryContext = _context.Stadia.FromSqlRaw("SELECT * FROM Stadium WHERE NOT EXISTS (SELECT Team.T_ID FROM Team WHERE Team.T_City = {0} AND NOT EXISTS (SELECT Game.G_ID FROM Game WHERE Game.G_Stadium = Stadium.ST_ID AND ((Team.T_ID = Game.G_TeamH) OR (Team.T_ID = Game.G_TeamA)))) AND EXISTS (SELECT Team.T_ID FROM Team WHERE Team.T_ID <> {0} AND EXISTS (SELECT Game.G_ID FROM Game WHERE Game.G_Stadium = Stadium.ST_ID AND ((Game.G_TeamH = Team.T_ID) OR (Game.G_TeamA = Team.T_ID))))", id);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: Stadia/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia
                .Include(s => s.StCityNavigation)
                .FirstOrDefaultAsync(m => m.StId == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        public async Task<IActionResult> DetailsCi(string? id)
        {
            return RedirectToAction("Details", "Cities", new { id = id });
        }

        // GET: Stadia/Create
        public IActionResult Create()
        {
            ViewData["StCity"] = new SelectList(_context.Cities, "CiId", "CiName");
            return View();
        }

        // POST: Stadia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StName,StCity,StCapacity,StId")] Stadium stadium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StCity"] = new SelectList(_context.Cities, "CiId", "CiId", stadium.StCity);
            return View(stadium);
        }

        // GET: Stadia/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }
            ViewData["StCity"] = new SelectList(_context.Cities, "CiId", "CiName", stadium.StCity);
            return View(stadium);
        }

        // POST: Stadia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StName,StCity,StCapacity,StId")] Stadium stadium)
        {
            if (id != stadium.StId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadiumExists(stadium.StId))
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
            ViewData["StCity"] = new SelectList(_context.Cities, "CiId", "CiId", stadium.StCity);
            return View(stadium);
        }

        // GET: Stadia/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadium = await _context.Stadia
                .Include(s => s.StCityNavigation)
                .FirstOrDefaultAsync(m => m.StId == id);
            if (stadium == null)
            {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var stadium = await _context.Stadia.FindAsync(id);
            _context.Stadia.Remove(stadium);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(string id)
        {
            return _context.Stadia.Any(e => e.StId == id);
        }
    }
}
