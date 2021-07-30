namespace DnaFragment.DnaFragmentControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using DnaFragment.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using static Data.DataConstants.LrUserConst;

    public class LrUsersController : Controller
    {      
       
        private readonly DnaFragmentDbContext data;

        public LrUsersController( DnaFragmentDbContext data)
        {         
            this.data = data;           
        }
        
        public IActionResult Register() => View();

       
        [HttpPost]
        public IActionResult Register(RegisterUser lrUser)
        {
            var userId = this.User.GetId();

            var userIdAlreadyRegister = this.data
                .LrUsers
                .Any(d => d.Id == userId);

            if (userIdAlreadyRegister)
            {
                return BadRequest();
            }

            if (this.data.LrUsers.Any(u => u.Username == lrUser.Username))
            {
                this.ModelState.AddModelError(nameof(lrUser.Username), $"User with '{lrUser.Username}' username already exists.");
            }

            if (this.data.LrUsers.Any(u => u.Email == lrUser.Email))
            {
                this.ModelState.AddModelError(nameof(lrUser.Email), $"User with '{lrUser.Email}' e-mail already exists.");
            }

            if (!ModelState.IsValid)
            {              

                return View(lrUser);
            }         

            var user = new LrUser
            {
                Id = userId,
                Username = lrUser.Username,
                Password = lrUser.Password,
                Email = lrUser.Email,
                PhoneNumber = lrUser.PhoneNumber,
                IsMechanic = lrUser.UserType == TypeMechanic,
               
            };

            if (((lrUser.Username == "Anko" && lrUser.Password == "1234567")
                || (lrUser.Username == "Niki" && lrUser.Password == "12345678")) && lrUser.UserPreoritet == TypeAdministrator)
            {
                user.IsAdministrator = true;         

            }
          
            data.LrUsers.Add(user);

            data.SaveChanges();

            return Redirect("/LrUsers/Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginUserFormModel model)
        {
            var userId = this.data
               .LrUsers
               .Where(u => u.Username == model.Username && u.Password == model.Password)
               .Select(u => u.Id)
               .FirstOrDefault();


            if (userId == null)
            {
                this.ModelState.AddModelError(nameof(userId), "Username and password combination is not valid.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

           // this.SignIn(userId);

           
          /*  var questionsOld = data.Questions.Where(x =>(DateTime.Now - x.CreatedOn).TotalDays > 30);
            var answersOld = data.Answers.Where(x => (DateTime.Now - x.CreatedOn).TotalDays > 30);
           
            data.Questions.RemoveRange(questionsOld);
            data.Answers.RemoveRange(answersOld);
            data.SaveChanges();*/

            return Redirect("/Categories/All");
        }

        [Authorize]
        public IActionResult All()
        {
           
            var users = data.LrUsers.Where(x => !x.IsAdministrator).Select(x => new UserListingViewModel
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                IsAdministrator = x.IsAdministrator
            }).ToList();

            return View(users);
        }

        public IActionResult Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
