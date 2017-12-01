using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevOpsPortal.Models
{
    public class CustomerDetails
    {
        [Display(Name = "CustomerId")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "ContactNumber is required.")]
        public string ContactNumber { get; set; }

        public string AlternateContactNumber { get; set; }

        [Required(ErrorMessage = "Specialty is required.")]
        public string Specialty { get; set; } 

        public string QualificationSummary { get; set; } 
    }
}