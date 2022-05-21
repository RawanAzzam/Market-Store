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
    public class SystemusersController : Controller
    {
        private readonly ModelContext _context;

        public SystemusersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Systemusers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Systemuser.ToListAsync());
        }

        // GET: Systemusers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemuser = await _context.Systemuser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemuser == null)
            {
                return NotFound();
            }

            return View(systemuser);
        }

        // GET: Systemusers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Systemusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Email,Id,Location,ImagePath")] Systemuser systemuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(systemuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemuser);
        }

        // GET: Systemusers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemuser = await _context.Systemuser.FindAsync(id);
            if (systemuser == null)
            {
                return NotFound();
            }
            return View(systemuser);
        }

        // POST: Systemusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Username,Email,Id,Location,ImagePath")] Systemuser systemuser)
        {
            if (id != systemuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemuserExists(systemuser.Id))
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
            return View(systemuser);
        }

        // GET: Systemusers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemuser = await _context.Systemuser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemuser == null)
            {
                return NotFound();
            }

            return View(systemuser);
        }

        // POST: Systemusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var systemuser = await _context.Systemuser.FindAsync(id);
            _context.Systemuser.Remove(systemuser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemuserExists(decimal id)
        {
            return _context.Systemuser.Any(e => e.Id == id);
        }
    }
}
