using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
     
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }
     
        [HttpGet("")]
        public IActionResult Register() => View();

        [HttpPost("create")]
        public IActionResult Create(UserRegister user) 
        {
            if (ModelState.IsValid)
            {
                // If a User exists with provided email
                if(_context.Users.Any(u => u.RegEmail == user.RegEmail))
                {
                    // error if email already exist
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    // You may consider returning to the View at this point
                    return View("Register");
                }

                PasswordHasher<UserRegister> hasher = new PasswordHasher<UserRegister>();
                user.RegPassword = hasher.HashPassword(user, user.RegPassword);

                var newUser = _context.Users.Add(user).Entity;
                _context.SaveChanges();
                // HttpContext.Session.SetString("FirstName", newUser.FirstName);
                // ViewBag.CurrentUserName = newUser.FirstName;
                // TempData["UserName"] = newUser.FirstName;
                HttpContext.Session.SetInt32("userId", newUser.UserId);
                return RedirectToAction("Success");
            }
            return View("Register");
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public IActionResult Login(UserLogin user)
        {
            if(ModelState.IsValid)
            {
                UserRegister UserLog = _context.Users.FirstOrDefault(usr => usr.RegEmail == user.LogEmail);
                if(UserLog == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Login");
                }
                PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin>();
                var result = hasher.VerifyHashedPassword(user, UserLog.RegPassword, user.LogPassword);
                if(result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Login");
                }

                // ViewBag.CurrentUserName = user.
                HttpContext.Session.SetInt32("userId", UserLog.UserId);
                return RedirectToAction("Success");
            }
            return View("Login");
        }

        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }
        
        [HttpGet("success")]
        public IActionResult Success() 
        {
            if(HttpContext.Session.GetInt32("userId") == null)
                return RedirectToAction("Register");
            return View();
        }

    }
 }