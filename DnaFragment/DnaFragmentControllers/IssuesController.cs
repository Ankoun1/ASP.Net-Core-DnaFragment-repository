namespace DnaFragment.DnaFragmentControllers
{
    using System.Linq;
    using DnaFragment.Data;
    using DnaFragment.Data.Models;
    using DnaFragment.Models.Cars;
    using DnaFragment.Models.Issues;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class IssuesController : Controller
    {
       
      /*  private readonly IUserService userService;
        private readonly DnaFragmentDbContext data;

        public IssuesController(IUserService userService, DnaFragmentDbContext data)
        {
            this.userService = userService;
            this.data = data;          
        }

        [Authorize]
        public IActionResult LrProductIssues(string carId)
        {
            if (!UserAcsesCar(User.id, carId))
            {
                return Unauthorized();
            }

            var carWithIssues = this.data
                .LrProducts
                .Where(c => c.Id == carId)
                .Select(c => new LrProductIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Name,
                    Year = c.Year,
                    UserIsMechanic = this.userService.IsMechanic(this.User.Id),
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        Description = i.Discription,
                        IsFixed = i.IsFixed
                    })
                })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with ID '{carId}' does not exist.");
            }

            return View(carWithIssues);
        }

        [Authorize]
        public IActionResult Add(string carId)
        {
            var car = data.LrProducts.Where(x => x.Id == carId).Select(x => new ValidateCarIssueModel { Id = x.Id }).FirstOrDefault();
            if(car == null)
            {
                return BadRequest();
            }
            return View(car);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(AddIssueModel model)
        {

            if (!UserAcsesCar(User.Id, model.CarId))
            {
                return Error("You do not have access to this car.");
            }          

         

            if (errors.Any())
            {
                return this.Error(errors);
            }

            var issue = new Issue
            {
                //Id = model.Id,
                Discription = model.Description,
                IsFixed = false,
                LrProductId = model.LrProductId
            };

            this.data.Issues.Add(issue);
            this.data.SaveChanges();
            
            return this.Redirect($"CarIssues?CarId={issue.LrProductId}");
        }

        [Authorize]        
        public IActionResult Fix(string issueId, string lrProductId)
        {
            var isMechanic = this.userService.IsMechanic(this.User.Id);

            if (!isMechanic)
            {
                return Unauthorized();
            }

            var issue = this.data.Issues.Find(issueId);

            if (issue == null)
            {
                return this.BadRequest();
            }

            issue.IsFixed = true;
            this.data.SaveChanges();

            return this.Redirect($"CarIssues?CarId={lrProductId}");
        }

        [Authorize]
        public IActionResult Delete(string issueId, string lrProductId)
        {
            if (!UserAcsesCar(User.Id, lrProductId))
            {
                return Unauthorized();
            }
            var issue = data.Issues.Find(issueId);

            this.data.Issues.Remove(issue);
            this.data.SaveChanges();

            return this.Redirect($"CarIssues?CarId={lrProductId}");
        }

        private bool UserAcsesCar(string userId,string lrProductId)
        {
            bool IsMechanic = this.userService.IsMechanic(userId);
            if (!IsMechanic)
            {
                var userOwnsCar = this.userService.userOwnsCar(lrProductId, userId);

                if (!userOwnsCar)
                {
                    return false;
                }
            }
            return true;
        }*/
    }
}
