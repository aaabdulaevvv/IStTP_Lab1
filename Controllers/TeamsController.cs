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
    public class TeamsController : Controller
    {
        private readonly DBLibraryContext _context;

        public TeamsController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            var dBLibraryContext = _context.Teams.Include(t => t.TCityNavigation).Include(t => t.TStadiumNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery3()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery3(int cnt)
        {
            var dBLibraryContext = _context.Teams.FromSqlRaw("SELECT * FROM Team WHERE EXISTS (SELECT Game.G_ID FROM Game WHERE Game.G_Attendance > {0} AND (Game.G_TeamH = Team.T_ID or Game.G_TeamA = Team.T_ID))", cnt);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery6()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery6(string id)
        {
            //Знайти команди, які грали у всіх тих містах, де грала команда Х
            var dBLibraryContext = _context.Teams.FromSqlRaw("SELECT * FROM Team WHERE NOT EXISTS (SELECT City.CI_ID FROM City WHERE EXISTS (SELECT Game.G_ID FROM Game INNER JOIN Stadium ON Game.G_Stadium = Stadium.ST_ID WHERE(Game.G_TeamH = {0} OR Game.G_TeamA = {0}) AND Stadium.ST_City = City.CI_ID) AND NOT EXISTS (SELECT Game.G_ID FROM Game INNER JOIN Stadium ON Game.G_Stadium = Stadium.ST_ID WHERE(Game.G_TeamH = Team.T_ID OR Game.G_TeamA = Team.T_ID) AND Stadium.ST_City = City.CI_ID)) AND Team.T_ID <> {0}", id);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery8()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery8(string id)
        {
            //Знайти команди, які не грали проти команд з країни Х
            var dBLibraryContext = _context.Teams.FromSqlRaw("SELECT * FROM Team WHERE NOT EXISTS(SELECT Game.G_ID FROM Game WHERE (Game.G_TeamH = Team.T_ID AND Game.G_TeamA IN (SELECT T.T_ID FROM Team T INNER JOIN City ON T.T_City=City.CI_ID WHERE City.CI_Country = {0})) OR (Game.G_TeamA = Team.T_ID AND Game.G_TeamH IN (SELECT T.T_ID FROM Team T INNER JOIN City ON T.T_City=City.CI_ID WHERE City.CI_Country = {0})) )", id);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.TCityNavigation)
                .Include(t => t.TStadiumNavigation)
                .FirstOrDefaultAsync(m => m.TId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        public async Task<IActionResult> DetailsSt(string? id)
        {
            return RedirectToAction("Details", "Stadia", new { id = id });
        }

        public async Task<IActionResult> DetailsCi(string? id)
        {
            return RedirectToAction("Details", "Cities", new { id = id });
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["TCity"] = new SelectList(_context.Cities, "CiId", "CiName");
            ViewData["TStadium"] = new SelectList(_context.Stadia, "StId", "StName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TName,TCity,TStadium,TManager,TId")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TCity"] = new SelectList(_context.Cities, "CiId", "CiId", team.TCity);
            ViewData["TStadium"] = new SelectList(_context.Stadia, "StId", "StId", team.TStadium);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["TCity"] = new SelectList(_context.Cities, "CiId", "CiName", team.TCity);
            ViewData["TStadium"] = new SelectList(_context.Stadia, "StId", "StName", team.TStadium);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TName,TCity,TStadium,TManager,TId")] Team team)
        {
            if (id != team.TId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TId))
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
            ViewData["TCity"] = new SelectList(_context.Cities, "CiId", "CiId", team.TCity);
            ViewData["TStadium"] = new SelectList(_context.Stadia, "StId", "StId", team.TStadium);
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.TCityNavigation)
                .Include(t => t.TStadiumNavigation)
                .FirstOrDefaultAsync(m => m.TId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(string id)
        {
            return _context.Teams.Any(e => e.TId == id);
        }
    }
}
