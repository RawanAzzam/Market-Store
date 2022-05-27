using Market_Store___First_Project.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Market_Store___First_Project.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private static int codeVerfiy;
        private static bool isVerfiy = false;
        public LoginAndRegisterController(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Email,Location,ImageFile")] Systemuser systemuser
            ,String password)
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
                    RoleId = 1,
                    UserName = systemuser.Email,
                    Passwordd = password,
                    UserId = systemuser.Id
                };
                _context.Add(userLogin);
                SendVerfiyCodeEmail(userLogin.UserName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(VerfiyEmail),new { userId = userLogin.Id});

            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName,Passwordd")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var auth = _context.UserLogin.
                    Where(x => x.UserName == userLogin.UserName &&
                    x.Passwordd == userLogin.Passwordd).SingleOrDefault();
               if (auth != null)
                {
                    var user = _context.Systemuser.Where(
                               x => x.Id == auth.UserId).SingleOrDefault();
                    if( (bool)auth.IsVerfiy)
                    {
                        switch (auth.RoleId)
                        {
                            case 1:
                                HttpContext.Session.SetInt32("UserId", (int)user.Id);
                                return RedirectToAction("Index", "Home");
                            case 2:
                                HttpContext.Session.SetInt32("AdminId", (int)user.Id);
                                return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                       
                        int userId = (int) auth.Id;
                        return RedirectToAction("VerfiyEmail", new { userId = userId });
                    }

                }
            }
            return View();
        }

        public IActionResult VerfiyEmail(int userId)
        {
            var userLogin = _context.UserLogin.Where(ul => ul.Id == userId).SingleOrDefault();
            SendVerfiyCodeEmail(userLogin.UserName);
           
            return View(userLogin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerfiyEmail(string code,int userId)
        {
            if (ModelState.IsValid)
            {
                if(codeVerfiy == int.Parse(code))
                {
                    var auth = _context.UserLogin.
                   Where(x => x.Id == userId).SingleOrDefault();
                    auth.IsVerfiy = true;
                    _context.Update(auth);
                    _context.SaveChanges();
                    isVerfiy = true;
                }
                else
                {
                    isVerfiy = false;
                }
            }
           

            if(isVerfiy)
            return RedirectToAction(nameof(Login));
            else 
            return RedirectToAction(nameof(Register));
        }

        private void SendVerfiyCodeEmail(String email)
        {
            Random rand = new Random();

            codeVerfiy = rand.Next(100000, 999999);
            string to = email; //To address    
            string from = "rawanazzam68@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = "this is code to verfiy Eamil \n"+codeVerfiy;
            message.Subject = "Market Store Verfiy Email";
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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session = null;
            return RedirectToAction(nameof(Login));
        }


    }
}
