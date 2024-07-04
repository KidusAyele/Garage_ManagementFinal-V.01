

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Garage_ManagementFinal.Models
{
    public class login
    {
        [Required(ErrorMessage = "Email is required")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string role { get; set; }
        public string status { get; set; }
        public string Firstname { get; set; }
    }
}

