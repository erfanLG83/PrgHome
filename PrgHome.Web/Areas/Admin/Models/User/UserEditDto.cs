﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrgHome.DataLayer.IdentityClasses;

namespace PrgHome.Web.Areas.Admin.Models.User
{
    public class UserEditDto
    {
        public UserEditDto()
        {

        }
        public UserEditDto(AppUser user,IEnumerable<string> roles)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.UserName;
            PhoneNumber = user.PhoneNumber;
            LastImage = user.Image;
            IsActive = user.IsActive;
            IsLockout = user.LockoutEnabled;
            EmailConfirmed = user.EmailConfirmed;
            if (LastImage==null)
            {
                LastImageDeleted = true;
            }
            else
            {
                LastImageDeleted = false;
            }
            UserRoles = "";
            foreach (var item in roles)
            {
                UserRoles += item + ",";
            }
            if (UserRoles.Length > 0)
            {
                UserRoles = UserRoles.Remove(UserRoles.Length - 1, 1);
            }
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "شیوه ورودی ایمیل صحیح نمی باشد !")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [StringLength(20, ErrorMessage = "{0} باید کمتر از {2} کارکتر باشد و حداکثر دارای {1} کارکتر باشد", MinimumLength = 3)]
        public string Username { get; set; }
        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }
        public string UserRoles { get; set; }
        public string LastImage { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockout { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LastImageDeleted { get; set; }
    }
}
