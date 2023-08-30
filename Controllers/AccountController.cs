using Portfolio_Eastelson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace Portfolio_Eastelson.Controllers
{
    public class AccountController : Controller
    {
        
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Employees.FirstOrDefault(e => e.Username == model.Username);

                if (user != null && IsValidUser(user.Username, model.Password))
                {
                    Session["AuthenticatedUser"] = user;
                    if (user.IsAdmin)
                    {
                        
                        return RedirectToAction("ListeDesSalaries", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Contact", "Home");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect.";
                    return View();
                }
            }
            return View(model);
        }

        private bool IsValidUser(string username, string password)
        {
            var passwordHash = _db.Employees
          .Where(e => e.Username == username)
                .Select(e => e.PasswordHash)
                .FirstOrDefault();
            return  password == passwordHash;
        }

       
    }
}






