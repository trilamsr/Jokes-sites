using System;
using System.ComponentModel.DataAnnotations;

namespace Belt_Exam.ViewModels 
{
    public class LogAndReg
    {
        public Register Register {set;get;}
        public Login Login {set;get;}
    }
    
    public class Register 
    {
        [Required]
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage="Name should only contain letters and spaces")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage="Alias should only contain letters and numbers")]
        public string Alias { get; set; }


        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage="Invalid email")]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password field must be at least 8 character")]
        [DataType(DataType.Password)]

        public string Password { get; set;}

        [Required]
        [Compare("Password", ErrorMessage="Confirm password does not match password")]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

    }
    public class Login 
    {
        [Required]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage="Invalid email")]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password {get;set;}
    }
}