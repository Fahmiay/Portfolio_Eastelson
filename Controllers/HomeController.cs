using Portfolio_Eastelson.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using System.Data.Entity;
using PagedList;

namespace Portfolio_Eastelson.Controllers
{
    public class HomeController : Controller
    {

       
        private ApplicationDbContext _db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About(LoginModel model)
        {
            var user = Session["AuthenticatedUser"] as Employee;
            return View(user);
        }

        public ActionResult Contact(string username)
        {
            var model = new ContactViewModel();

            if (Session["AuthenticatedUser"] != null)
            {
                var user = (Employee)Session["AuthenticatedUser"];
                ViewBag.Age = CalculateAge(user.DateOfBirth);

                if (!string.IsNullOrEmpty(username))
                {
                    model.Employee = _db.Employees
                        .Include(e => e.Missions)
                        .FirstOrDefault(e => e.Username == username);
                }
                else
                {
                    // Si aucun nom d'utilisateur n'est spécifié, affichez les informations de l'utilisateur authentifié
                    model.Employee = _db.Employees
                        .Include(e => e.Missions)
                        .FirstOrDefault(e => e.Id == user.Id);
                }

                if (model.Employee?.Missions != null)
                {
                    model.Missions = model.Employee.Missions.ToList();
                }

                model.Missions = model.Employee?.Missions?.OrderByDescending(m => m.ClientEntryDate).ToList() ?? new List<Mission>();
            }

            return View(model);
        }


        public int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            // Vérifie si l'anniversaire est déjà passé cette année
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            return age;
        }
        [HttpPost]
        public JsonResult AjouterMission(Mission mission, int employeeId)
        {
            if (ModelState.IsValid)
            {
                mission.EmployeeId = employeeId;

                _db.Missions.Add(mission);

                try
                {
                    _db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Vous pouvez loguer l'exception ici pour un débogage ultérieur
                    return Json(new { success = false, message = "Une erreur s'est produite lors de l'ajout de la mission" });
                }
            }

            return Json(new { success = false, message = "Échec de la validation du modèle" });
        }

        [HttpPost]
        
        public JsonResult SupprimerMission(int id, int employeeId)
        {
            var mission = _db.Missions.Find(id);
            if (mission == null)
            {
                return Json(new { success = false, message = "Mission introuvable" });
            }

            Employee user = _db.Employees.Find(employeeId) as Employee;

            if (user != null && user.Id== employeeId)
            {
                _db.Missions.Remove(mission);
                try
                {
                    _db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    // Loguez l'exception ici pour un débogage ultérieur
                    return Json(new { success = false, message = "Une erreur s'est produite lors de la suppression de la mission" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Accès non autorisé ou ID de l'employé incorrect" });
            }
        }

        public ActionResult ListeDesSalaries(string searchString, int? page)
        {
            var user = Session["AuthenticatedUser"] as Employee;
            if (user != null && user.IsAdmin)
            {
                IQueryable<Employee> employees = _db.Employees;

                if (!String.IsNullOrEmpty(searchString))
                {
                    employees = employees.Where(s => s.FirstName.Contains(searchString)
                                                   || s.LastName.Contains(searchString));
                }

                // Define the number of records per page
                int pageSize = 8;

                // Set the page number
                int pageNumber = (page ?? 1);

                return View(employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult DeleteEmployee(string username)
        {
            // Rechercher l'employé dans la base de données
            var employee = _db.Employees.Include(e => e.Missions)  // Inclure les missions
                                         .FirstOrDefault(e => e.Username == username);

            if (employee == null)
            {
                return HttpNotFound("Employé non trouvé");
            }

            try
            {
                // Supprimer toutes les missions associées à cet employé
                if (employee.Missions != null)
                {
                    _db.Missions.RemoveRange(employee.Missions);
                }

                // Supprimer l'employé
                _db.Employees.Remove(employee);

                // Sauvegarder les changements
                _db.SaveChanges();

                return RedirectToAction("ListeDesSalaries");  // Rediriger vers la liste des employés
            }
            catch (Exception ex) // Vous pouvez également utiliser DbUpdateException
            {
                // Log ou gérer l'exception
                return new HttpStatusCodeResult(500, "Une erreur s'est produite pendant la suppression");
            }
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee model)
        {
            
                // Convertir le modèle de vue en entité
                var employee = new Employee
                {
                    Username = model.Username,
                    PasswordHash = model.PasswordHash,  // Note: Hash the password before saving
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Salary = model.Salary,
                    PhotoPath = model.PhotoPath,
                    IsAdmin = model.IsAdmin
                };

                _db.Employees.Add(employee);
                _db.SaveChanges();

                return RedirectToAction("ListeDesSalaries", "Home");
            }
        
        

        public ActionResult Logout()
        {
           
            Session.Clear();  // Supprime toutes les clés de session
            Session.Abandon();  // Annule la session actuelle
            return RedirectToAction("Login", "Account");            
           
        }

    }

}

  