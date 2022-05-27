using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public HomeController(ILogger<HomeController> logger , ModelContext modelContext)
        {
            _logger = logger;
            _context = modelContext;
        }

        public IActionResult Index()
        {
            ViewBag.isLogin = false;
           if (HttpContext.Session.GetString("UserName") !=  null)
            {

                ViewBag.isLogin = true;
                ViewBag.name = HttpContext.Session.GetString("UserName");
            }

            var testimonials = _context.Testimonial.Where(t => (bool)t.Isverfiy).Include(t => t.User).ToList();
           var category = _context.Category.ToList();
         
            return View(Tuple.Create<IEnumerable<Category>,IEnumerable<Testimonial>>(category,testimonials));
        }

        public IActionResult Store(int? categoryId)
        {
            var stores = _context.Store.ToList();

            return View(stores);
        }

        public IActionResult ProductStore(int? storeId)
        {
            var productsStore = _context.ProductStore.Include(ps => ps.Store).Include(ps => ps.Product);
         
            return View(productsStore);
        }

        public IActionResult AddProductOrder(int productId)
        {
            int id = 6;
            var lastOrder = _context.Userorder.Where(uo => uo.Userid == 6).OrderBy(uo => uo.Dateoforder)
                .LastOrDefault();

            if(lastOrder == null)
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
                };
                _context.Add(productOrder);
                _context.SaveChanges();

                ViewBag.countProduct = "*";
            }
            else
            {
                if((bool)lastOrder.IsCheckout)
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
                        Quntity = 1,
                    };

                    _context.Add(productOrder);
                    _context.SaveChanges();
                    ViewBag.countProduct = "*";
                }
                else
                {
                    var productOrderIsFound = _context.Productorder.Where(
                        po => po.Orderid == lastOrder.Id && po.Productid == productId).SingleOrDefault();

                    if(productOrderIsFound == null)
                    {
                        var productOrder = new Productorder
                        {
                            Orderid = lastOrder.Id,
                            Productid = productId,
                            Quntity = 1,
                        };

                        _context.Add(productOrder);
                        _context.SaveChanges();
                        ViewBag.countProduct = "*";
                    }
                    else
                    {
                        productOrderIsFound.Quntity += 1;
                        _context.Update(productOrderIsFound);
                        _context.SaveChanges();
                        ViewBag.countProduct = "*";
                    }
                   
                }
            }
            var productsStore = _context.ProductStore.Include(ps => ps.Store).Include(ps => ps.Product);

            return RedirectToAction("ProductStore",productsStore);

        }

        public IActionResult Cart()
        {
            int id = 6;
            var lastOrder = _context.Userorder.Where(uo => uo.Userid == 6 && uo.IsCheckout == false)
                .OrderBy(uo => uo.Dateoforder)
                .LastOrDefault();

            if(lastOrder != null)
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

                foreach(var item in q)
                {
                    totalCost += (int) (item.product.Sale * item.productorder.Quntity);
                }
                lastOrder.Cost = totalCost;
                _context.Update(lastOrder);
                _context.SaveChanges();
                return View(Tuple.Create<IEnumerable<MultiTables>,Userorder>(q,lastOrder));
            }

            //foreach (var productOrder in productOrders)
            //{
            //    int qunitiy = (int)productOrder.Quntity;

            //    var productStore = _context.ProductStore.Where(ps => ps.Id == productOrder.Productid).SingleOrDefault();
            //    var product = _context.Product.Where(p => p.Id == productStore.Productid).SingleOrDefault();
            //    totalCost += (int)product.Sale * qunitiy;
            //}

            return View(Tuple.Create<IEnumerable<MultiTables>, Userorder>(null, null));
        }

        public IActionResult DeletePrdouctFromOrder(int productId)
        {
            int id = 6;
            var lastOrder = _context.Userorder.Where(uo => uo.Userid == 6 && uo.IsCheckout == false)
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

        public IActionResult CheckOut(int orderId)
        {
            var order = _context.Userorder.Where(uo => uo.Id == orderId).SingleOrDefault();

            return View(order);
        }

        [HttpPost]
        public IActionResult CheckOut(int userOrderId , int cardNumber , int mm , int yy)
        {
            int id = 6;
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
                    SendEmail(user.Email, "hi rawan <br> ur order cost is " + order.Cost, "Payment");
                    card.Balance -= order.Cost;
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

        public void SendEmail(string to , string body , string subject)
        {
            
            string from = "rawanazzam68@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = body;
            message.Subject = subject;
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

        public IActionResult ViewProduct(int productId)
        {
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
            int id = 44;

            var ratingIsFound = _context.Rate.Where(r => r.UserId == id && 
                                                    r.ProductId == productId).SingleOrDefault();

            if(ratingIsFound == null)
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
            
            return RedirectToAction(nameof(ViewProduct),new { productId = productStoreId });
        }

        private double getRate(decimal productId)
        {
            if (_context.Rate.Where(r => r.ProductId == productId).Count() == 0)
                return 0.0;

            return _context.Rate.Where(r => r.ProductId == productId).Average(rate => (int)rate.RateNum);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddTestimonial()
        {
            return View();
        }
        public IActionResult ReportStore(int storeId)
        {
            var store = _context.Store.Where(s => s.Id == storeId).SingleOrDefault();
            return View(store);
        }

        [HttpPost]
        public IActionResult ReportStore(int storeId , string message)
        {
            int id = 44;
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
        [HttpPost]
        public IActionResult AddTestimonial( string message , int rateNum)
        {
            int id = 7;
            var testimonialIsFound = _context.Testimonial.Where(t => t.Userid == id).SingleOrDefault();

            if(testimonialIsFound == null)
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

        public IActionResult Contact_us()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
