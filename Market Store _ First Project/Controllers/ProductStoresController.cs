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
    public class ProductStoresController : Controller
    {
        private readonly ModelContext _context;

        public ProductStoresController(ModelContext context)
        {
            _context = context;
        }

        // GET: ProductStores
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.ProductStore.Include(p => p.Product).Include(p => p.Store);
            return View(await modelContext.ToListAsync());
        }

        // GET: ProductStores/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productStore = await _context.ProductStore
                .Include(p => p.Product)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productStore == null)
            {
                return NotFound();
            }

            return View(productStore);
        }

        // GET: ProductStores/Create
        public IActionResult Create()
        {
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Id");
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Id");
            return View();
        }

        // POST: ProductStores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Storeid,Productid,Count")] ProductStore productStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Id", productStore.Productid);
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Id", productStore.Storeid);
            return View(productStore);
        }

        // GET: ProductStores/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productStore = await _context.ProductStore.FindAsync(id);
            if (productStore == null)
            {
                return NotFound();
            }
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Id", productStore.Productid);
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Id", productStore.Storeid);
            return View(productStore);
        }

        // POST: ProductStores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Storeid,Productid,Count")] ProductStore productStore)
        {
            if (id != productStore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductStoreExists(productStore.Id))
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
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Id", productStore.Productid);
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Id", productStore.Storeid);
            return View(productStore);
        }

        // GET: ProductStores/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productStore = await _context.ProductStore
                .Include(p => p.Product)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productStore == null)
            {
                return NotFound();
            }

            return View(productStore);
        }

        // POST: ProductStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var productStore = await _context.ProductStore.FindAsync(id);
            _context.ProductStore.Remove(productStore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductStoreExists(decimal id)
        {
            return _context.ProductStore.Any(e => e.Id == id);
        }
    }
}
