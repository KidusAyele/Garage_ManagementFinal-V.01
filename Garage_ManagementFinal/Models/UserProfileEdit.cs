using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


 namespace Garage_ManagementFinalWithDatabase.Models

{
    public class UserProfileEdit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email
        {
            get; set;
        }
        [Required]
        [Display(Name = "username")]
        public string username { get; set; }
        [Required]
        [Display(Name = "password")]
        public string password { get; set; }
    }
}