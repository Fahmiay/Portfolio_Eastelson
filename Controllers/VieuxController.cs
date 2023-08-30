using Portfolio_Eastelson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portfolio_Eastelson.Controllers
{
    public class VieuxController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult TestConnection()
        {
            bool isConnected = false;

            try
            {
                var employee = _db.Employees.FirstOrDefault(); // Essaye de récupérer le premier employé
                if (employee != null)
                {
                    isConnected = true;
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs si nécessaire
            }

            ViewBag.Connected = isConnected;

            return View();
        }
        public ActionResult Index()
        {
            var employees = _db.Employees.Select(e => e.Username).ToList(); // Récupère les éléments de la première colonne
            return View(employees);
        }

    }
}