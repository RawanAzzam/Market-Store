using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Market_Store___First_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly AdminReport report = new AdminReport();
        private  static int id;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        private void CheckSession()
        {
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                id = (int) HttpContext.Session.GetInt32("AdminId");
                var user = _context.Systemuser.Where(u => u.Id == id).SingleOrDefault();
                ViewBag.userName = user.Username;
                ViewBag.ImageFile = user.ImagePath;
            }
           
        }
      

        public void GetChart()
        {
            var stores = _context.Store.ToList();

            string[] storeNames = new string[stores.Count];
            int[] sale = new int[stores.Count];
            int count = 0;
            foreach (var item in stores)
            {
                storeNames[count] = item.Storename;
                sale[count] = (int)report.GetTotalSalesForStore((int)item.Id);
                count++;
                //dataPoints.Add(new Charts((int)item.Id,(int) report.GetTotalSalesForStore((int)item.Id)));
            }

            
            ViewBag.storeNames =  JsonConvert.SerializeObject(storeNames);
            ViewBag.sale = JsonConvert.SerializeObject(sale);
        }

        public void GetAnnualSale()
        {
            string[] months = new string[] { "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"};
            int year = DateTime.Now.Year;
            int[] saleMonth = new int[12];
            for (int i = 1; i <= 12; i++)
                saleMonth[i - 1] = (int) report.GetSaleForPeroidOfTime(new DateTime(year, i, 1),
                    new DateTime(year, i, 28));

            ViewBag.months = JsonConvert.SerializeObject(months);
            ViewBag.saleMonth = JsonConvert.SerializeObject(saleMonth);
        }
        // GET: AnminController
        public ActionResult Index()
        {
            CheckSession();
            GetChart();
            GetAnnualSale();

            //if (HttpContext.Session.GetInt32("AdminId") != null)
            //{
            // int id = (int) HttpContext.Session.GetInt32("AdminId");

            var user = _context.Systemuser.Where(u => u.Id == 6).SingleOrDefault();

                ViewBag.userName = user.Username;
                ViewBag.ImageFile = user.ImagePath;
                ViewBag.UserRegistered = report.GetRegisteredUsers();
                ViewBag.TodaySale = report.GetTodaySale();
                ViewBag.MontlySale = report.GetMontlySale();
                ViewBag.TotalStore = report.GetTotalStore();

                var contactuser = _context.Contactususer.ToList();
                Tuple<IEnumerable<MultiTables>, MultiTables, IEnumerable<Contactususer>> tuple
                    = Tuple.Create<IEnumerable<MultiTables>, MultiTables, IEnumerable<Contactususer>>
                    (report.GetOdresByPeroidOfTime(null, null).Item1, report.GetOdresByPeroidOfTime(null, null).Item2,
                    contactuser);
                return View(tuple);

            //}
            //return RedirectToAction("Login", "LoginAndRegister");
        }

        public ActionResult GetOrderBySpecficDate(DateTime? dateFrom, DateTime? dateTo,string? type)
        {
            CheckSession();
            if (type != null && type.Equals("annule"))
            {
                dateFrom = new DateTime(DateTime.Now.Year, 1, 1);
                dateTo = new DateTime(DateTime.Now.Year, 12,31);
                return View(nameof(Index), report.GetOdresByPeroidOfTime(dateFrom, dateTo));

            }
            else if (type != null &&  type.Equals("monthly"))
            {
                dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month,28);
                return View(nameof(Index), report.GetOdresByPeroidOfTime(dateFrom, dateTo));
            }
            return View(nameof(Index), report.GetOdresByPeroidOfTime(dateFrom, dateTo));
        }

        public IActionResult AdminProfile()
        {
            CheckSession();
            var profileInfo = _context.Systemuser.Where(user => user.Id == id).SingleOrDefault();
            var adminLogin = _context.UserLogin.Where(user => user.UserId == id).SingleOrDefault();

            return View(Tuple.Create<Systemuser, UserLogin>(profileInfo, adminLogin));
        }

        public async Task<IActionResult>  EditProfile([Bind("Username,Email,Id,Location,ImagePath,ImageFile")] Systemuser systemuser)
        {
            CheckSession();
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
                return RedirectToAction(nameof(AdminProfile));
            }
            return View();
        }
       
        #region User

        public IActionResult Users(string? userName, int? userId, string? location, string? roleName,
            string? email)
        {
            CheckSession();
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
            CheckSession();
            var roles = _context.Role.ToList();
            return View(roles);
        }

        // POST: CreateUser
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Username,Email,Location,ImageFile")] Systemuser systemuser,
           String password, int roleId)
        {
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
            CheckSession();
            return View(await _context.Category.ToListAsync());
        }

        /////////////////////*********************** Create Category
        #region Create Category
        public IActionResult CreateCategory()
        {
            CheckSession();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("CategoryName,ImageFile")] Category category)
        {
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
        public ActionResult Stores(string categoryName , string location , int? categoryId,string name 
            , string owner , int? id )
        {
            CheckSession();
            var stores = _context.Store.ToList();
            var categories = _context.Category.ToList();

            if(categoryName != null)
            {
                stores = stores.Where(s => s.Category.CategoryName == categoryName).ToList();
            }
            if(categoryId != null){
                stores = stores.Where(s => s.Category.Id == categoryId).ToList();
            }
            if(location != null)
            {
                stores = stores.Where(s => s.Storelocation == location).ToList();
            }
            if(name != null)
            {
                stores = stores.Where(s => s.Storename == name).ToList();
            }
            if(id != null)
            {
                stores = stores.Where(s => s.Id == id).ToList();
            }
            if(owner != null)
            {
                stores = stores.Where(s => s.Ownername == owner).ToList();
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
            CheckSession();
            ViewData["Categoryid"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStore([Bind("Storename,Storelocation,Ownername,Categoryid,LogoFile")] Store store)
        {
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
            CheckSession();
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
        public ActionResult ProductStore(int? storeId ,int? productStoreId)
        {
            CheckSession();
            var productStore = _context.ProductStore.ToList();
            var stores = _context.Store.ToList();
            var products = _context.Product.ToList();

            if (storeId != null)
            {
                productStore = productStore.Where(s => s.Storeid == storeId).ToList();
                ViewBag.todaySale = report.GetToadySaleForStore((int)storeId);
                ViewBag.monthlySale = report.GetMonthlySaleForStore((int) storeId);
                ViewBag.totalSale = report.GetTotalSalesForStore((int)storeId);
                ViewBag.totalProduct = report.GetTotalProductForStore((int)storeId);
            }
            if(productStoreId != null)
            {
                productStore = productStore.Where(s => s.Id == productStoreId).ToList();
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


        ///////////////////***************** Create Product Store
          #region Create Product Store
        public IActionResult CreateProductStore()
        {
            CheckSession();
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Namee");
            ViewData["Storeid"] = new SelectList(_context.Store, "Id", "Storename");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductStore([Bind("Storeid,Productid,Count")] ProductStore productStore)
        {
            CheckSession();
            if (ModelState.IsValid)
            {
              var isFound = _context.ProductStore.Where(ps => ps.Storeid == productStore.Storeid
                && ps.Productid == productStore.Productid).SingleOrDefault();
                int id = (int)productStore.Storeid;
                if (isFound == null)
                {
                    _context.Add(productStore);
                }
                else
                {
                    isFound.Count += productStore.Count;
                    _context.ProductStore.Update(isFound);
                }
              
                await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(ProductStore),new {storeId = id });
            }
            ViewData["Productid"] = new SelectList(_context.Product, "Id", "Namee", productStore.Productid);
            ViewData["Storeid"] = new SelectList(_context.Product, "Id", "Storename", productStore.Storeid);
            return View(productStore);
        }
        #endregion

        ////////////////////**************** Delete Product Store
        public async Task<IActionResult> DeleteProductStore(decimal? id)
        {
            CheckSession();
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
            else
            {
                _context.ProductStore.Remove(productStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ProductStore),new { storeId = productStore.Storeid});
            }

        }

       
        #endregion

        #region Product
        public IActionResult Products()
        {
            CheckSession();
            var products = _context.Product.ToList();
            var productCategory = _context.ProductCategory.ToList();

            var multiTable = from p in products
                             join pc in productCategory
                             on p.ProductCategoryId equals pc.Id
                             select new MultiTables
                             {
                                 product = p,
                                 productCategory = pc
                             };

            var multiTableRate = new MultiTables();
            foreach (var item in products)
            {
                multiTableRate.AddRate((int)item.Id, (int)getRate(item.Id));
            }

            return View(Tuple.Create<IEnumerable<MultiTables>, MultiTables>(multiTable, multiTableRate));

        }

        ////////////******************************* Add New Product
        #region Create Product
        public IActionResult CreateProduct()
        {
            CheckSession();
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("Namee,Sale,Price,ImageFile,ProductCategoryId")] Product product)
        {
            CheckSession();
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    product.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    product.ImagePath = fileName;
                }
                product.DateOfAdd = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);
            return View();
        }
        #endregion

        /////////////////////////*********************** Edit Product
        #region Edit Product
        public async Task<IActionResult> EditProduct(decimal? id)
        {
            CheckSession();
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(decimal id, [Bind("Id,Namee,Sale,Price,ImagePath,ImageFile,ProductCategoryId,DateOfAdd")] Product product)
        {
            CheckSession();
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnviroment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" +
                        product.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }
                        product.ImagePath = fileName;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Products));
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);
            return View(product);
        }

        #endregion

        ///////////////////////////********************* Delete Product
        #region Delete Product
        public async Task<IActionResult> DeleteProduct(decimal? id)
        {
            CheckSession();
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }

        }

       #endregion

        //////////////////******************** getRateOfProduct
        private double getRate(decimal productId)
        {
            if (_context.Rate.Where(r => r.ProductId == productId).Count() == 0)
                return 0.0;

            return _context.Rate.Where(r => r.ProductId == productId).Average(rate => (int)rate.RateNum);
        }

        ///////////////////////**************** check if product Exsit 
        private bool ProductExists(decimal id)
        {

            return _context.Product.Any(e => e.Id == id);
        }

        #endregion

        #region Testimonial
        public IActionResult ViewTestimonial()
        {
            CheckSession();
            var testimonials = _context.Testimonial.Include(t => t.User).ToList();

            return View(testimonials);
        }

        public IActionResult ManageTestimonial(int id , int verfiy)
        {
            CheckSession();
            var testimonial = _context.Testimonial.Where(t => t.Id == id).SingleOrDefault();

            switch (verfiy)
            {
                case 1: _context.Testimonial.Remove(testimonial);
                    _context.SaveChanges();
                    break;
                case 2: testimonial.Isverfiy = true;
                    _context.Update(testimonial);
                    _context.SaveChanges();
                    break;
            }

            var testimonials = _context.Testimonial.Include(t => t.User).ToList();

            return RedirectToAction("ViewTestimonial");
        }
        #endregion

        #region Report Store
        public IActionResult ViewReport()
        {
            CheckSession();
            var storeReports = _context.Report.Include(r => r.User).Include(r => r.Store).ToList();

            return View(storeReports);
        }
        #endregion

        #region Manage Home
        public IActionResult ManageHome()
        {
            CheckSession();
            var home = _context.Home.Where(h => h.Id == 1).SingleOrDefault();
            var contact = _context.Contactus.Where(c => c.Id == 1).SingleOrDefault();
            var about = _context.Aboutus.Where(a => a.Id == 1).SingleOrDefault();
            return View(Tuple.Create<Home,Contactus,Aboutus>(home,contact,about));
        }

      
        [HttpPost]
        public async Task<IActionResult> ManageHome([Bind("Id","Slide1","Slide2","Slide3","Websitename",
            "Logoimage","Slide1File","Slide2File","Slide3File")] Home home,
            [Bind("Email", "Address", "Phonenumber","Id")] Contactus contact,
            [Bind("ImageFile", "ImagePath","Info","Id", "OurFeatures1", "OurFeatures2", "OurFeatures3")] Aboutus aboutus)
        {
            CheckSession();
            #region home
            if (home.Slide1File != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" +
                home.Slide1File.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await home.Slide1File.CopyToAsync(fileStream);
                }
                home.Slide1 = fileName;
            }
            if (home.Slide2File != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" +
                home.Slide2File.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await home.Slide2File.CopyToAsync(fileStream);
                }
                home.Slide2 = fileName;
            }
            if (home.Slide3File != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" +
                home.Slide3File.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await home.Slide3File.CopyToAsync(fileStream);
                }
                home.Slide3 = fileName;
            }
            #endregion

            #region About us
            if (aboutus.ImageFile != null)
            {
                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" +
                aboutus.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await aboutus.ImageFile.CopyToAsync(fileStream);
                }
                aboutus.ImagePath = fileName;
            }
            #endregion

            
            _context.Update(home);
            _context.Update(contact);
            _context.Update(aboutus);
            _context.SaveChanges();

            return View(Tuple.Create<Home, Contactus, Aboutus>(home, contact, aboutus));
        }
        #endregion

        #region Contact Us User

        public IActionResult ReplyMessage(int contactUserId)
        {
            CheckSession();
            var contact = _context.Contactususer.Where(c => c.Id == contactUserId).SingleOrDefault();

            return View(contact);
        }

        [HttpPost]
        public IActionResult ReplyMessage(int id ,string email, string body)
        {
            CheckSession();
            SendEmail(email, body);

            var contactUs = _context.Contactususer.Where(c => c.Id == id).SingleOrDefault();
            _context.Contactususer.Remove(contactUs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SendEmail(string email,string body)
        {

            string to = email; //To address    
            string from = "rawanazzam68@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = body;
            message.Subject = "Reply Report Problem";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("rawanazzam68@gmail.com", "Rram1210.");
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Manage Code Sale

        public IActionResult CodesSale()
        {
            CheckSession();
            var codes = _context.CodeSale.ToList();

            return View(codes);
        }

        #region Add Code Sale
        public IActionResult AddCodeSale()
        {
            CheckSession();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCodeSale([Bind("code","sale")] CodeSale codeSale)
        {
            CheckSession();
            if(ModelState.IsValid)
            {
                var codeIsFound = _context.CodeSale.Where(c => c.code == codeSale.code).SingleOrDefault();
                if(codeIsFound != null)
                {
                    ViewBag.CodeIsFound = "This code is already used , try another code";
                }
                else {
                 _context.Add(codeSale);
                _context.SaveChanges();
                    return RedirectToAction("CodesSale");
                }

                
            }
            return View();

            
        }

        #endregion

        #region Edit Code Sale
        public async Task<IActionResult> EditCodeSale(decimal? id)
        {
            CheckSession();
            if (id == null)
            {
                return NotFound();
            }

            var code = await _context.CodeSale.FindAsync(id);
            if (code == null)
            {
                return NotFound();
            }
            return View(code);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCodeSale(decimal id, [Bind("Id","code", "sale")] CodeSale codeSale)
        {
            CheckSession();
            if (id != codeSale.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Update(codeSale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(codeSale.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CodesSale));
            }
            return View(codeSale);
        }
        #endregion

        #region Delete Code Sale
        public async Task<IActionResult> DeleteCodeSale(decimal? id)
        {
            CheckSession();
            if (id == null)
            {
                return NotFound();
            }

            var code = await _context.CodeSale
                  .FirstOrDefaultAsync(m => m.Id == id);
            if (code == null)
            {
                return NotFound();
            }
            else
            {
                _context.CodeSale.Remove(code);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CodesSale));
            }

        }
        #endregion
        #endregion

        #region 
        public IActionResult ProductCategory()
        {
            var productCategory = _context.ProductCategory.ToList();
            return View(productCategory);
        }


        #endregion
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session = null;
            return RedirectToAction("Login","LoginAndRegister");
        }
    }

}
