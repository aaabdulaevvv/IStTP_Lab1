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
    public class GamesController : Controller
    {
        private readonly DBLibraryContext _context;

        public GamesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var dBLibraryContext = _context.Games.Include(g => g.GStadiumNavigation).Include(g => g.GTeamANavigation).Include(g => g.GTeamHNavigation);
            return View(await dBLibraryContext.ToListAsync());
        }

        public async Task<IActionResult> CreateQuery2()
        {
            return View();
        }
        public async Task<IActionResult> IndexQuery2(int cnt)
        {
            //var dBLibraryContext = _context.Games.FromSqlRaw("SELECT * FROM Game WHERE Game.G_Attendance > {0}", cnt - 1);
            var dBLibraryContext = _context.Games.FromSqlRaw("SELECT * FROM Game WHERE Game.G_ID in (SELECT Game.G_ID FROM (Game inner join Team on Game.G_TeamA=Team.T_ID) inner join City on Team.T_City=City.CI_ID WHERE City.CI_Population > {0})", cnt-1);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GStadiumNavigation)
                .Include(g => g.GTeamANavigation)
                .Include(g => g.GTeamHNavigation)
                .FirstOrDefaultAsync(m => m.GId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public async Task<IActionResult> DetailsT(string? id)
        {
            return RedirectToAction("Details", "Teams", new { id = id });
        }

        public async Task<IActionResult> DetailsSt(string? id)
        {
            return RedirectToAction("Details", "Stadia", new { id = id });
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["GStadium"] = new SelectList(_context.Stadia, "StId", "StName");
            ViewData["GTeamA"] = new SelectList(_context.Teams, "TId", "TName");
            ViewData["GTeamH"] = new SelectList(_context.Teams, "TId", "TName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GId,GTeamH,GTeamA,GStadium,GAttendance")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GStadium"] = new SelectList(_context.Stadia, "StId", "StId", game.GStadium);
            ViewData["GTeamA"] = new SelectList(_context.Teams, "TId", "TId", game.GTeamA);
            ViewData["GTeamH"] = new SelectList(_context.Teams, "TId", "TId", game.GTeamH);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["GStadium"] = new SelectList(_context.Stadia, "StId", "StName", game.GStadium);
            ViewData["GTeamA"] = new SelectList(_context.Teams, "TId", "TName", game.GTeamA);
            ViewData["GTeamH"] = new SelectList(_context.Teams, "TId", "TName", game.GTeamH);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GId,GTeamH,GTeamA,GStadium,GAttendance")] Game game)
        {
            if (id != game.GId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GId))
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
            ViewData["GStadium"] = new SelectList(_context.Stadia, "StId", "StId", game.GStadium);
            ViewData["GTeamA"] = new SelectList(_context.Teams, "TId", "TId", game.GTeamA);
            ViewData["GTeamH"] = new SelectList(_context.Teams, "TId", "TId", game.GTeamH);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.GStadiumNavigation)
                .Include(g => g.GTeamANavigation)
                .Include(g => g.GTeamHNavigation)
                .FirstOrDefaultAsync(m => m.GId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GId == id);
        }
    }
}
