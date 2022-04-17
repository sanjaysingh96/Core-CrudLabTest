using CoreLabTest.Models;
using CoreLabTest.MyDatabase;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreLabTest.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {

            HttpContext.Session.SetString("Name", "Sanjay");


            return View();
        }

        [HttpGet]
        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(RegModel robj)
        {

            foodiesContext db = new foodiesContext();
            var userlogin = db.Registrations.Where(a => a.Email == robj.Email).FirstOrDefault();
            if (userlogin == null)
            {
                TempData["invalid"] = "Invalid Email !!";
            }
            else
            {
                if(userlogin.Email==robj.Email && userlogin.Password == robj.Password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, userlogin.Name),
                                        new Claim(ClaimTypes.Email, userlogin.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    HttpContext.Session.SetString("Name", userlogin.Name);
                    HttpContext.Session.GetString("Name");

                    return RedirectToAction("IndexDashBoard", "Home");
                }
                else
                {
                    TempData["wrong"] = "Wrong Password !!";
                    return View();
                }
            }
            return View();
        }


        [HttpGet]
        
        public IActionResult UserReg()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult UserReg(RegModel robj)
        {
            foodiesContext db = new foodiesContext();
            Registration tbl = new Registration();
            tbl.Id = robj.Id;
            tbl.Name = robj.Name;
            tbl.Email = robj.Email;
            tbl.Mobile = robj.Mobile;
            tbl.Password = robj.Password;

            db.Registrations.Add(tbl);
            db.SaveChanges();

            return RedirectToAction("Login","Home");
        }


        [Authorize]
        public IActionResult IndexDashboard()
        {
            return View();
        }

        
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index","Home");
        }


        public IActionResult Privacy()
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
