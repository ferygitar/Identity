using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage ="نام کاربری را وارد کنید")]
        [Remote("IsUserNameInUse", "Account")]
        public string UserName { get; set; }

        [Display(Name ="ایمیل")]
        [Required(ErrorMessage ="پست الکترونیکی خود را وارد کنید")]
        [EmailAddress]
        [Remote("IsEmailInUse", "Account")]
        public string Email { get; set; }

        [Display(Name ="رمز عبور")]
        [Required(ErrorMessage ="رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="تکرار رمز عبور")]
        [Required(ErrorMessage ="تکرار رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
