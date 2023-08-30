using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio_Eastelson.Models
{
    [Table("Missions")]
    public class Mission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MissionId { get; set; }

        [ForeignKey("Employee")]
        [Required]
        public int EmployeeId { get; set; }

        [StringLength(50)]
        public string TechnicalSkill { get; set; }

        [StringLength(50)]
        public string ClientName { get; set; }

        [StringLength(255)]
        public string MissionDetails { get; set; }

        public DateTime? ClientEntryDate { get; set; }

        [StringLength(50)]
        public string Role { get; set; }

        // Navigation property
        public virtual Employee Employee { get; set; }

       
    }
}