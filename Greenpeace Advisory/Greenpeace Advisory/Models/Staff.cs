using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Greenpeace_Advisory.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        [Required, Display(Name = "Last Name"), StringLength(25, ErrorMessage = "Last Name cannot be longer than 25 characters.")]
        public String LastName { get; set; }
        [Required, Display(Name = "First Name"), StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public String FirstName { get; set; }
        [Display(Name = "Middle Name"), StringLength(25, ErrorMessage = "Last Name cannot be longer than 25 characters.")]
        public String MiddleName { get; set; }
    }
}