using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Machine_Test___Clavax.Models
{
    public class employeeRegistration_Model
    {
        [Display(Name = "Employee ID")]
        public int Employee_ID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Employee_Name { get; set; }
        [Required]
        [Display(Name = "DOB")]
        public DateTime Employee_DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "State")]
        public int State_fid { get; set; }
        
        public string Hobbies { get; set; }

        public List<hobbyModel> hobbiesc { get; set; }

    }

    public class hobbyModel
    {
        public int hobbyID { get; set; }
        
        public string hobbyName { get; set; }

        public bool IsChecked { get; set; }
    }

    }