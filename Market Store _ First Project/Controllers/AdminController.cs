using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        // GET: AnminController
        public ActionResult Index()
        {
            return View();
        }

        #region User

        public IActionResult Users(string? userName, int? userId, string? location, string? roleName,
            string? email)
        {
            var systemUsers = _context.Systemuser.ToList();
            var usresLogin = _context.UserLogin.ToList();
            var roles = _context.Role.ToList();

            if(userName != null)
            {
                systemUsers = systemUsers.Where(user => user.Username == userName).ToList();
            }
            if(userId != null)
            {
                systemUsers = systemUsers.Where(user => user.Id == userId).ToList();
            }
            if(email != null)
            {
                systemUsers = systemUsers.Where(user => user.Email == email).ToList();
            }
            if(location != null)
            {
                systemUsers = systemUsers.Where(user => user.Location == location).ToList();
            }
            if(roleName != null)
            {
                roles = roles.Where(role => role.Rolename == roleName).ToList();
            }

            var multiTable = from su in systemUsers
                             join us in usresLogin on su.Id equals us.UserId
                             join r in roles on us.RoleId equals r.Id
                             select new MultiTables {
                                 systemuser = su , userLogin = us , role = r };

            return View(multiTable);
        }

        ///////////////////****************************** Create User
        #region Create User
        public IActionResult CreateUser()
        {
            var roles = _context.Role.ToList();
            return View(roles);
        }

        // POST: CreateUser
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Username,Email,Location,ImageFile")] Systemuser systemuser,
           String password, int roleId)
        {
            if (ModelState.IsValid)
            {
                if (systemuser.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    systemuser.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await systemuser.ImageFile.CopyToAsync(fileStream);
                    }
                    systemuser.ImagePath = fileName;
                }
                _context.Add(systemuser);
                await _context.SaveChangesAsync();
                UserLogin userLogin = new UserLogin
                {
                    RoleId = roleId,
                    UserName = systemuser.Email,
                    Passwordd = password,
                    UserId = systemuser.Id
                };
                _context.Add(userLogin);
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));

            }
            if (ModelState.IsValid)
            {
                _context.Add(systemuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(systemuser);
        }
        #endregion

        //////////////////////*************************** Edit User
        #region editUser
        public async Task<IActionResult> EditUser(decimal? id)
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(decimal id, [Bind("Username,Email,Id,Location,ImagePath,ImageFile")] Systemuser systemuser,
             String password)
        {
            if (id != systemuser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (systemuser.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        systemuser.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await systemuser.ImageFile.CopyToAsync(fileStream);
                        }
                        systemuser.ImagePath = fileName;
                    }
                    var userLogin = await _context.UserLogin.Where(u => u.UserId == systemuser.Id)
                        .SingleOrDefaultAsync();
                    userLogin.UserName = systemuser.Email;
                    userLogin.Passwordd = password;

                    _context.Update(systemuser);
                    _context.Update(userLogin);
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
                return RedirectToAction(nameof(Users));
            }
            return View(systemuser);
        }

        private bool SystemuserExists(decimal id)
        {
            return _context.Systemuser.Any(e => e.Id == id);
        }

        #endregion

        /////////////////////***************************** Delete User 
        #region Delete User
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemuser = await _context.Systemuser
                .SingleOrDefaultAsync(m => m.Id == id);
            if (systemuser == null)
            {
                return NotFound();
            }
            else
            {
                var userLogin = await _context.UserLogin.
                    SingleOrDefaultAsync(u => u.UserId == id);
                _context.UserLogin.Remove(userLogin);
                _context.Systemuser.Remove(systemuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }

            
        }
        #endregion

        ///////////////////******************************* Search User
        #region Search User
        public IActionResult SearchUser(string? userName , int? userId ,string? location , string? roleName,
            string? email)
        {
            
            var usresLogin = _context.UserLogin.ToList();
            var roles = _context.Role.ToList();

            var systemUsers = _context.Systemuser.Where(user => user.Username == userName).ToList();
            
            var multiTable = from su in systemUsers
                             join us in usresLogin on su.Id equals us.UserId
                             join r in roles on us.RoleId equals r.Id
                             select new MultiTables
                             {
                                 systemuser = su,
                                 userLogin = us,
                                 role = r
                             };

            return RedirectToAction(nameof(Users));
        }
        #endregion
        #endregion

        #region Category
        public async Task<IActionResult> Categories()
        {
            return View(await _context.Category.ToListAsync());
        }

        /////////////////////*********************** Create Category
        #region Create Category
        public IActionResult CreateCategory()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("CategoryName,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    category.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(fileStream);
                    }
                    category.ImagePath = fileName;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }
        #endregion

        ////////////////////************************ Edit Category 
        #region Edit Category
        public async Task<IActionResult> EditCategory(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(decimal id, [Bind("Id,CategoryName,ImageFile,ImagePath")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        category.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await category.ImageFile.CopyToAsync(fileStream);
                        }
                        category.ImagePath = fileName;
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }


        #endregion

        #region Delete Category
        public async Task<IActionResult> DeleteCategory(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
              
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }

        }

       
        #endregion

        ////////////////////***************** If Category Exsit
        private bool CategoryExists(decimal id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
        #endregion


        #region Store
        public ActionResult Stores(string? categoryName , string? location , int? categoryId)
        {
           

            var stores = _context.Store.ToList();
            var categories = _context.Category.ToList();

            if(categoryName != null)
            {
                stores = stores.Where(s => s.Category.CategoryName == categoryName).ToList();
            }
            if(categoryId != null){
                stores = stores.Where(s => s.Category.Id == categoryId).ToList();
            }
            var multiTables = from s in stores
                              join c in categories on s.Categoryid equals c.Id
                              select new MultiTables
                              {
                                  category = c, store = s
                              };

            return View(multiTables);
        }

        //////////////////////********************** Create Store
        #region Create Store
        public IActionResult CreateStore()
        {
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStore([Bind("Storename,Storelocation,Ownername,Categoryid,LogoFile")] Store store)
        {
            if (ModelState.IsValid)
            {
                if (store.LogoFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    store.LogoFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await store.LogoFile.CopyToAsync(fileStream);
                    }
                   store.StoreLogo = fileName;
                }
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stores));
            }
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "CategoryName", store.Categoryid);
            return View(store);
        }
        #endregion

        ///////////////**************** Edit Store
        #region Edit Store
        public async Task<IActionResult> EditStore(decimal? id)
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
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "CategoryName", store.Categoryid);
            return View(store);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStore(decimal id, [Bind("Storename,Storelocation,Ownername,Categoryid,StoreLogo,Id,LogoFile")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (store.LogoFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        store.LogoFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await store.LogoFile.CopyToAsync(fileStream);
                        }
                        store.StoreLogo = fileName;
                    }
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
                return RedirectToAction(nameof(Stores));
            }
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "CategoryName", store.Categoryid);
            return View(store);
        }

        #endregion

        ///////////////////******************** Delete Store 
        #region Delete Store
        public async Task<IActionResult> DeleteStore(decimal? id)
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
            else
            {
                 _context.Store.Remove(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stores));
            }

           
        }

    
        #endregion

        ///////////*************** check if store exsit or not
        private bool StoreExists(decimal id)
        {
            return _context.Store.Any(e => e.Id == id);
        }
        #endregion


        #region Product Store
        public ActionResult ProductStore(int? storeId)
        {


            var productStore = _context.ProductStore.ToList();
            var stores = _context.Store.ToList();
            var products = _context.Product.ToList();

            if (storeId != null)
            {
                productStore = productStore.Where(s => s.Storeid == storeId).ToList();
            }
           

            var multiTable = from ps in productStore
                             join s in stores on ps.Storeid equals s.Id
                             join p in products on ps.Productid equals p.Id
                             select new MultiTables
                             { 
                                 product = p,
                                 store = s,
                                 productStore = ps
                             };

            var multiTableRate = new MultiTables();
            foreach(var item in products)
            {
                multiTableRate.AddRate((int)item.Id, (int)getRate(item.Id));
            }
           
            return View(Tuple.Create<IEnumerable<MultiTables>,MultiTables>(multiTable,multiTableRate));
        }


        private double getRate(decimal productId)
        {
            return _context.Rate.Where(r => r.ProductId == productId).Average(rate => (int)rate.RateNum);
        }

        #region Create Product Store
        public IActionResult CreateProductStore()
        {
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Namee");
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Storename");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductStore([Bind("Storeid,Productid,Count")] ProductStore productStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProductStore));
            }
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Namee", productStore.Productid);
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Storename", productStore.Storeid);
            return View(productStore);
        }
        #endregion
        #endregion
    }

}
