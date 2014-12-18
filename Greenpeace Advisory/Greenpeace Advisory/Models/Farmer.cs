using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Greenpeace_Advisory.Models
{
    public class Farmer
    {
        public int FarmerID { get; set; }
        [Required, Display(Name = "Last Name"), StringLength(25, ErrorMessage = "Last Name cannot be longer than 25 characters.")]
        public String LastName { get; set; }
        [Required, Display(Name = "First Name"), StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public String FirstName { get; set; }
        [Display(Name = "Middle Name"), StringLength(25, ErrorMessage = "Last Name cannot be longer than 25 characters.")]
        public String MiddleName { get; set; }

        public virtual ICollection<ContactDetail> MobileNumbers { get; set; }
       
        //[Required]
        //public int RegionId { get; set; }

        //[ForeignKey("RegionId")]
        //public virtual Region Region { get; set; }
    }

    //public class Region
    //{
    //    public int RegionId { get; set; }
    //    [Required]
    //    public string Name { get; set; }
    //}

    public class ContactDetail
    {
        public int ContactDetailId { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter proper contact details.")]
        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        [Required]
        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public virtual Farmer Farmer { get; set; }
    }
}