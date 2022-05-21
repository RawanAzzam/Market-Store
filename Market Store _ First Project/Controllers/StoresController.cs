using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Market_Store___First_Project.Models;

namespace Market_Store___First_Project.Controllers
{
    public class StoresController : Controller
    {
        private readonly ModelContext _context;

        public StoresController(ModelContext context)
        {
            _context = context;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Store.Include(s => s.Category);
            return View(await modelContext.ToListAsync());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "Id");
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Storename,Storelocation,Ownername,Categoryid,StoreLogo,Id")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "Id", store.Categoryid);
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "Id", store.Categoryid);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Storename,Storelocation,Ownername,Categoryid,StoreLogo,Id")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
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
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "Id", store.Categoryid);
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(decimal id)
        {
            return _context.Store.Any(e => e.Id == id);
        }
    }
}
