using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class NewsBoardsController : Controller
    {
        private readonly NewsDbContext _context;

        public NewsBoardsController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: NewsBoards
        public async Task<IActionResult> Index(string? Id)
        {
            var viewModel = new ClientNewsBoardViewModel
            {
                NewsBoards = await _context.NewsBoards
                .Include(i => i.Subscriptions)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync()
            };

            if (Id != null)
            {
                ViewData["NewsBoardId"] = Id;
                viewModel.Subscriptions = await _context.Subscriptions
                .Include(i => i.Client)
                .Include(i => i.NewsBoard)
                .Where(i => i.NewsBoardId == Id)
                .AsNoTracking()
                .ToListAsync();
                /*viewModel.Clients = await _context.Clients
                .Include(i => i.Subscriptions)
                .Where(i => i.Subscriptions.Contains == Id)
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync();*/
            }

            return View(viewModel);
        }

        // GET: NewsBoards/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NewsBoards == null)
            {
                return NotFound();
            }

            var NewsBoard = await _context.NewsBoards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (NewsBoard == null)
            {
                return NotFound();
            }

            return View(NewsBoard);
        }

        // GET: NewsBoards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Fee")] NewsBoard NewsBoard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(NewsBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(NewsBoard);
        }

        // GET: NewsBoards/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NewsBoards == null)
            {
                return NotFound();
            }

            var NewsBoard = await _context.NewsBoards.FindAsync(id);
            if (NewsBoard == null)
            {
                return NotFound();
            }
            return View(NewsBoard);
        }

        // POST: NewsBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Fee")] NewsBoard NewsBoard)
        {
            if (id != NewsBoard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(NewsBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsBoardExists(NewsBoard.Id))
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
            return View(NewsBoard);
        }

        // GET: NewsBoards/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NewsBoards == null)
            {
                return NotFound();
            }

            var NewsBoard = await _context.NewsBoards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (NewsBoard == null)
            {
                return NotFound();
            }

            return View(NewsBoard);
        }

        // POST: NewsBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NewsBoards == null)
            {
                return Problem("Entity set 'NewsDbContext.NewsBoards'  is null.");
            }
            var NewsBoard = await _context.NewsBoards.FindAsync(id);
            if (NewsBoard != null)
            {
                _context.NewsBoards.Remove(NewsBoard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsBoardExists(string id)
        {
            return _context.NewsBoards.Any(e => e.Id == id);
        }
    }
}
