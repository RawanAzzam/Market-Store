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
    public class UserLoginsController : Controller
    {
        private readonly ModelContext _context;

        public UserLoginsController(ModelContext context)
        {
            _context = context;
        }

        // GET: UserLogins
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.UserLogin.Include(u => u.Role).Include(u => u.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: UserLogins/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogin
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLogin == null)
            {
                return NotFound();
            }

            return View(userLogin);
        }

        // GET: UserLogins/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Rolename");
            ViewData["UserId"] = new SelectList(_context.Systemuser, "Id", "Id");
            return View();
        }

        // POST: UserLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Passwordd,RoleId,UserId,IsVerfiy")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Rolename", userLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Systemuser, "Id", "Id", userLogin.UserId);
            return View(userLogin);
        }

        // GET: UserLogins/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogin.FindAsync(id);
            if (userLogin == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Rolename", userLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Systemuser, "Id", "Id", userLogin.UserId);
            return View(userLogin);
        }

        // POST: UserLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,UserName,Passwordd,RoleId,UserId,IsVerfiy")] UserLogin userLogin)
        {
            if (id != userLogin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLoginExists(userLogin.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Rolename", userLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Systemuser, "Id", "Id", userLogin.UserId);
            return View(userLogin);
        }

        // GET: UserLogins/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogin
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userLogin == null)
            {
                return NotFound();
            }

            return View(userLogin);
        }

        // POST: UserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userLogin = await _context.UserLogin.FindAsync(id);
            _context.UserLogin.Remove(userLogin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLoginExists(decimal id)
        {
            return _context.UserLogin.Any(e => e.Id == id);
        }
    }
}
