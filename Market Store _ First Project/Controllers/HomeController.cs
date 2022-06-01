using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Market_Store___First_Project.Controllers
{
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly AdminReport adminReport = new AdminReport();
        private static int id;

        public HomeController(ILogger<HomeController> logger , ModelContext modelContext, IWebHostEnvironment webHostEnviroment)
        {
            _logger = logger;
            _context = modelContext;
            _webHostEnviroment = webHostEnviroment;
        }

        private void CheckSession()
        {
            ViewBag.isLogin = false;
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                ViewBag.isLogin = true;
                id = (int) HttpContext.Session.GetInt32("UserId");
            }
        }
        public IActionResult Index()
        {
            CheckSession();
            var testimonials = _context.Testimonial.Where(t => (bool)t.Isverfiy).Include(t => t.User).ToList();
            var category = _context.Category.ToList();
            var home = _context.Home.Where(h => h.Id == 1).SingleOrDefault();
            var contact = _context.Contactus.Where(h => h.Id == 1).SingleOrDefault();
            var about = _context.Aboutus.Where(h => h.Id == 1).SingleOrDefault();

            ViewBag.Users = adminReport.GetRegisteredUsers();
            ViewBag.Strors = adminReport.GetTotalStore();
            ViewBag.Products = adminReport.GetTotalProduct();

            return View(Tuple.Create<IEnumerable<Category>,IEnumerable<Testimonial>,Home,Contactus,Aboutus>
                (category,testimonials,home,contact,about));
        }

        //////////////////////////////// View Store For category //////////////////////////////////
        #region Store
        public IActionResult Store(int? categoryId)
        {
            CheckSession();
            var stores = _context.Store.ToList();

            if(categoryId != null)
            {
                stores = stores.Where(s => s.Categoryid == categoryId).ToList();
            }
            return View(stores);
        }

        //////////////////////////////// Report Store  //////////////////////////////////
        public IActionResult ReportStore(int storeId)
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var store = _context.Store.Where(s => s.Id == storeId).SingleOrDefault();
                return View(store);
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
         
        }

        [HttpPost]
        public IActionResult ReportStore(int storeId, string message)
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var reportIsFound = _context.Report.Where(t => t.Userid == id).SingleOrDefault();

                if (reportIsFound == null)
                {
                    Report report = new Report
                    {
                        Mesaage = message,
                        Storeid = storeId,
                        Userid = id,
                    };
                    _context.Add(report);
                    _context.SaveChanges();
                }
                else
                {
                    reportIsFound.Mesaage = message;
                    _context.Update(reportIsFound);
                    _context.SaveChanges();
                }

                return RedirectToAction("Store");
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
           
        }

        #endregion


        //////////////////////////////// View Product For Store  //////////////////////////////////
        public IActionResult ProductStore(int? storeId)
        {
            CheckSession();
            var productsStore = _context.ProductStore.Include(ps => ps.Store).Include(ps => ps.Product).ToList();
         
            if(storeId != null)
            {
                productsStore = productsStore.Where(ps => ps.Storeid == storeId).ToList();
            }
            return View(productsStore);
        }

        //////////////////////////////// Cart  //////////////////////////////////
        #region Cart 
        //////////////////////////////// Add Product  //////////////////////////////////
        public IActionResult AddProductOrder(int productId , int? quntity)
        {
            CheckSession();

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                string message = null;
                var product = _context.ProductStore.Where(ps => ps.Id == productId).SingleOrDefault();
                if (product.Count >= (quntity == null ? 1 : quntity))
                {
                    var lastOrder = _context.Userorder.Where(uo => uo.Userid == id).OrderBy(uo => uo.Dateoforder)
                      .LastOrDefault();

                    if (lastOrder == null)
                    {
                        Userorder userorder = new Userorder
                        {
                            Userid = id,
                            Dateoforder = System.DateTime.Now
                        };

                        _context.Add(userorder);
                        _context.SaveChanges();

                        var productOrder = new Productorder
                        {
                            Orderid = userorder.Id,
                            Productid = productId,
                            Quntity = quntity == null ? 1 : quntity,
                        };
                        _context.Add(productOrder);
                        _context.SaveChanges();

                        ViewBag.countProduct = "*";
                    }
                    else
                    {
                        if ((bool)lastOrder.IsCheckout)
                        {
                            Userorder userorder = new Userorder
                            {
                                Userid = id,
                                Dateoforder = System.DateTime.Now
                            };

                            _context.Add(userorder);
                            _context.SaveChanges();

                            var productOrder = new Productorder
                            {
                                Orderid = userorder.Id,
                                Productid = productId,
                                Quntity = quntity == null ? 1 : quntity,
                            };

                            _context.Add(productOrder);
                            _context.SaveChanges();
                            ViewBag.countProduct = "*";
                        }
                        else
                        {
                            var productOrderIsFound = _context.Productorder.Where(
                                po => po.Orderid == lastOrder.Id && po.Productid == productId).SingleOrDefault();

                            if (productOrderIsFound == null)
                            {
                                var productOrder = new Productorder
                                {
                                    Orderid = lastOrder.Id,
                                    Productid = productId,
                                    Quntity = quntity == null ? 1 : quntity,
                                };

                                _context.Add(productOrder);
                                _context.SaveChanges();
                                ViewBag.countProduct = "*";
                            }
                            else
                            {
                                if (quntity == null)
                                {
                                    productOrderIsFound.Quntity += 1;
                                }
                                else
                                {
                                    productOrderIsFound.Quntity = quntity;
                                }

                                _context.Update(productOrderIsFound);
                                _context.SaveChanges();
                                ViewBag.countProduct = "*";
                            }

                        }
                    }
                }
                else
                {
                    message = $"Quntity ({quntity}) is more than Available Count ({product.Count}) of product";
                }

                return RedirectToAction(nameof(Cart), new { msg = message });
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            

        }

        //////////////////////////////// View Cart  //////////////////////////////////
        public IActionResult Cart(string msg)
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var lastOrder = _context.Userorder.Where(uo => uo.Userid == id && uo.IsCheckout == false)
                 .OrderBy(uo => uo.Dateoforder)
                 .LastOrDefault();

                if (lastOrder != null)
                {
                    int totalCost = 0;

                    var productOrders = _context.Productorder.Where(pr => pr.Orderid == lastOrder.Id).ToList();
                    var productStore = _context.ProductStore.ToList();
                    var product = _context.Product.ToList();

                    var q = from po in productOrders
                            join ps in productStore
                            on po.Productid equals ps.Id
                            join p in product
                            on ps.Productid equals p.Id
                            select new MultiTables
                            {
                                product = p,
                                productorder = po,
                                productStore = ps
                            };

                    foreach (var item in q)
                    {
                        totalCost += (int)(item.product.Sale * item.productorder.Quntity);
                    }
                    lastOrder.Cost = totalCost;
                    _context.Update(lastOrder);
                    _context.SaveChanges();
                    ViewBag.Message = msg;
                    return View(Tuple.Create<IEnumerable<MultiTables>, Userorder>(q, lastOrder));
                }


                return View(Tuple.Create<IEnumerable<MultiTables>, Userorder>(null, null));
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

            
        }

        //////////////////////////////// Delete Product From Cart //////////////////////////
        public IActionResult DeletePrdouctFromOrder(int productId)
        {
            CheckSession();

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var lastOrder = _context.Userorder.Where(uo => uo.Userid == id && uo.IsCheckout == false)
                 .OrderBy(uo => uo.Dateoforder)
                 .LastOrDefault();

                if (lastOrder == null)
                {
                    return NotFound();
                }
                else
                {
                    var productOrderIsFound = _context.Productorder.Where(
                        po => po.Orderid == lastOrder.Id && po.Productid == productId)
                      .SingleOrDefault();

                    if (productOrderIsFound == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        _context.Productorder.Remove(productOrderIsFound);
                        _context.SaveChanges();
                    }

                }


                return RedirectToAction(nameof(Cart));
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }

           
        }

        //////////////////////////////// Check Out  //////////////////////////////////
        public IActionResult CheckOut(int orderId)
        {
            CheckSession();
            var order = _context.Userorder.Where(uo => uo.Id == orderId).SingleOrDefault();

            return View(order);
        }

        [HttpPost]
        public IActionResult CheckOut(int userOrderId , int cardNumber , int mm , int yy)
        {
            CheckSession();
            int id = 7;
            var user = _context.Systemuser.Where(u => u.Id == id).SingleOrDefault();
            var order = _context.Userorder.Where(uo => uo.Id == userOrderId).SingleOrDefault();
            var card = _context.Card.Where(c => c.Tcb == 
            cardNumber.ToString() && c.Expiredate.Value.Month == mm
            && c.Expiredate.Value.Year == yy).SingleOrDefault();

            if(order == null || card == null || user == null)
            {
                return NotFound();
            }
            else
            {
               
                if(order.Cost <= card.Balance)
                {
                    string body = $"Welcome  {user.Username} , Thank you to buy from us ,total Cost is { order.Cost}";
                    string totalProduct = $"Total Cost is : {order.Cost} \nDate Of Order :{order.Dateoforder}\nProducts are : \n";
                    var productsOrder = _context.Productorder.Where(pr => pr.Orderid == order.Id).ToList();
                    var productStore = _context.ProductStore.ToList();
                    var product = _context.Product.ToList();
                    var q = from po in productsOrder
                            join ps in productStore
                        on po.Productid equals ps.Id
                        join p in product
                        on ps.Productid equals p.Id
                        select new MultiTables
                        {
                            product = p,
                            productorder = po,
                            productStore = ps
                        };
                    int count = 1;
                  
                foreach (var item in q)
                {
                        totalProduct += $"{count++}\t\tProduct Name : {item.product.Namee}\t\tPrice : {item.product.Sale}\t\tQuntity : {item.productorder.Quntity}\t\tTotal : {item.product.Sale*item.productorder.Quntity}\n";
                }
                    string FileName = @"C:\Users\Lenovo\Desktop\"+user.Username+"-invoice.txt";
                    StreamWriter writer = new StreamWriter(FileName);
                    writer.Write(totalProduct);
                    writer.Close();
                    SendInvoiceByEmail(user.Email, body,FileName, "Payment");
                    card.Balance -= order.Cost;
                      foreach(var item in productsOrder)
                    {
                        var product1 = _context.ProductStore.Where(ps => ps.Id == item.Productid).SingleOrDefault();
                        product1.Count -= item.Quntity;
                        _context.Update(product1);
                        _context.SaveChanges();
                    }
                    order.IsCheckout = true;
                    _context.Update(card);
                    _context.Update(order);
                    _context.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Cart));

        }

        public void SendInvoiceByEmail(string to , string body , string fileName, string subject)
        {
            
            string from = "rawanazzam68@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = body;
            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Attachments.Add(new Attachment(fileName));

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

        //////////////////////////////// View Product  //////////////////////////////////
        #region View Product
        public IActionResult ViewProduct(int productId)
        {
            CheckSession();
            var productStore = _context.ProductStore.Where(ps => (int)ps.Id == productId).SingleOrDefault();
            var product = _context.Product.Where(p => p.Id == productStore.Productid)
                .Include(p => p.ProductCategory).SingleOrDefault();
            var store = _context.Store.Where(s => s.Id == productStore.Storeid).SingleOrDefault();

            double rate = getRate(product.Id);

            var rates = _context.Rate.Where(r => r.ProductId == product.Id).Include(r => r.User).ToList();

            return View(Tuple.Create<ProductStore,Product,Store,double,
               IEnumerable<Rate>>(productStore,product,store,rate,rates));
        }

        public IActionResult AddRate(string feedback,int rate , int productId , int productStoreId)
        {
            CheckSession();

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var ratingIsFound = _context.Rate.Where(r => r.UserId == id &&
                                                     r.ProductId == productId).SingleOrDefault();

                if (ratingIsFound == null)
                {
                    Rate rateing = new Rate();
                    rateing.ProductId = productId;
                    rateing.UserId = id;
                    rateing.RateNum = rate;
                    rateing.Feedback = feedback;

                    _context.Add(rateing);
                    _context.SaveChanges();
                }
                else
                {
                    ratingIsFound.RateNum = rate;
                    ratingIsFound.Feedback = feedback;
                    _context.Update(ratingIsFound);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(ViewProduct), new { productId = productStoreId });
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
 
        }

        private double getRate(decimal productId)
        {
            if (_context.Rate.Where(r => r.ProductId == productId).Count() == 0)
                return 0.0;

            return _context.Rate.Where(r => r.ProductId == productId).Average(rate => (int)rate.RateNum);
        }

        #endregion


        public IActionResult AboutUs()
        {
            CheckSession();
            var about = _context.Aboutus.Where(a => a.Id == 1).SingleOrDefault();
            return View(about);
        }

        public IActionResult AddTestimonial()
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
           
        }
       
        [HttpPost]
        public IActionResult AddTestimonial( string message , int rateNum)
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var testimonialIsFound = _context.Testimonial.Where(t => t.Userid == id).SingleOrDefault();

                if (testimonialIsFound == null)
                {
                    Testimonial testimonial = new Testimonial
                    {
                        Content = message,
                        Rate = rateNum,
                        Userid = id,
                    };
                    _context.Add(testimonial);
                    _context.SaveChanges();
                }
                else
                {
                    testimonialIsFound.Content = message;
                    testimonialIsFound.Rate = rateNum;
                    _context.Update(testimonialIsFound);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
           
        }

        public IActionResult ContactUs()
        {
            CheckSession();
            var contact = _context.Contactus.Where(c => c.Id == 1).SingleOrDefault();
            return View(contact);
        }

        [HttpPost]
        public IActionResult ContactUs([Bind("Message", "Username", "Email", "Phonenumber")] Contactususer contactus)
        {
            CheckSession();
            _context.Add(contactus);
            _context.SaveChanges();

            var contact = _context.Contactus.Where(c => c.Id == 1).SingleOrDefault();

            return View(contact);
        }

        public IActionResult ViewProfile()
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var user = _context.Systemuser.Where(u => u.Id == id).SingleOrDefault();
                var userLogin = _context.UserLogin.Where(ul => ul.UserId == id).SingleOrDefault();
                var orders = _context.Userorder.Where(o => o.Userid == id && o.IsCheckout == true).ToList();

                return View(Tuple.Create<Systemuser, UserLogin, IEnumerable<Userorder>>(user, userLogin, orders));
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
         
        }

        public IActionResult EditProfile([Bind("Username,Email,Id,Location,ImagePath,ImageFile")] Systemuser systemuser,
          string Passwordd)
        {
            CheckSession();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userLogin = _context.UserLogin.Where(u => u.UserId == systemuser.Id).SingleOrDefault();
                userLogin.Passwordd = Passwordd;

                if (systemuser.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" +
                    systemuser.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        systemuser.ImageFile.CopyTo(fileStream);
                    }
                    systemuser.ImagePath = fileName;
                }
                _context.Update(systemuser);
                _context.Update(userLogin);
                _context.SaveChanges();

                return RedirectToAction(nameof(ViewProfile));
            }
            else
            {
                return RedirectToAction("Login", "LoginAndRegister");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            CheckSession();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
           
            HttpContext.Session.Clear();
            HttpContext.Session = null;
            return RedirectToAction("Login", "LoginAndRegister");
        }
    }
}
