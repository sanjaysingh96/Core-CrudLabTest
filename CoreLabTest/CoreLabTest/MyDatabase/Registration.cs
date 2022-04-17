using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CoreLabTest.MyDatabase
{
    public partial class Registration
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Email id")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string Mobile { get; set; }
        [Required (ErrorMessage ="Create password")]
        public string Password { get; set; }
    }
}
