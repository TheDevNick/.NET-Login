using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Login.Models
{
    public class UserRegister
{
    [Key]
    public int UserId {get;set;}
    [Required]
    [MinLength(2,ErrorMessage ="Name must be at least 2 characters")]
    public string FirstName {get;set;}
    [Required]
    [MinLength(2,ErrorMessage ="Name must be at least 2 characters")]
    public string LastName {get;set;}
    [Required]
    public string RegEmail {get;set;}

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8,ErrorMessage ="Password must be at least 8 characters")]
    public string RegPassword {get;set;}
    [Required]
    [DataType(DataType.Password)]
    [Compare("RegPassword",ErrorMessage ="Passwords do not match")]
    [MinLength(8)]
    [NotMapped]
    public string Confirm {get;set;}
}

    public class UserLogin
{
    [Required(ErrorMessage = "This field is required")]
    public string LogEmail {get;set;}
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8,ErrorMessage ="Password must be at least 8 characters")]
    public string LogPassword {get;set;}
}
}