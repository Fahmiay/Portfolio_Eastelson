using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Portfolio_Eastelson.Models
{
    public class Employee
    {


        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Required]
        [StringLength(255)]
        public string PhotoPath { get; set; }
        public bool IsAdmin { get; set; } = false;

        public virtual ICollection<Mission> Missions { get; set; }

        public  int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            // Vérifie si l'anniversaire est déjà passé cette année
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
            {
                age--;
            }

            return age;
        }
    }
    
}