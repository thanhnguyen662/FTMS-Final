using FTMS.Models;
using FTMS.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FTMS.Controllers
{
    public class StaffsController : Controller
    {
        private ApplicationDbContext context;
        public StaffsController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Admin
        [Authorize(Roles = "Training Staff")]
        public ActionResult Index()
        {
            var userInfor = (from user in context.Users
                             select new
                             {
                                 UserId = user.Id,
                                 Username = user.UserName,
                                 EmailAddress = user.Email,
                                 RoleName = (from userRole in user.Roles
                                             join role in context.Roles
                                             on userRole.RoleId
                                             equals role.Id
                                             select role.Name)
                             }
                       ).ToList().Select(p => new UserRoleViewModel()
                       {
                           UserId = p.UserId,
                           Username = p.Username,
                           Email = p.EmailAddress,
                           Role = string.Join(",", p.RoleName)
                       }
                       );
            return View(userInfor);
        }
        //DELETE ACCOUNT
        [HttpGet]
        [Authorize(Roles = "Training Staff")]
        public ActionResult Delete(string id)
        {
            var AccountInDB = context.Users.SingleOrDefault(p => p.Id == id);

            if (AccountInDB == null)
            {
                return HttpNotFound();
            }

            context.Users.Remove(AccountInDB);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Edit 
        [HttpGet]
        [Authorize(Roles = "Training Staff")]
        public ActionResult Edit(string id)
        {
            var AccountInDB = context.Users.SingleOrDefault(p => p.Id == id);
            if (AccountInDB == null)
            {
                return HttpNotFound();
            }
            return View(AccountInDB);
        }

        //EDIT
        [HttpPost]
        [Authorize(Roles = "Training Staff")]
        public ActionResult Edit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var AccountInDB = context.Users.SingleOrDefault(p => p.Id == user.Id);

            if (AccountInDB == null)
            {
                return HttpNotFound();
            }

            AccountInDB.UserName = user.UserName;
            context.SaveChanges();

            return RedirectToAction("Index");
        }


        //RESET PASSWORD
        [HttpGet]
        [Authorize(Roles = "Training Staff")]
        public ActionResult Reset(string id)
        {
            var AccountInDB = context.Users.SingleOrDefault(p => p.Id == id);

            if (AccountInDB == null)
            {
                return HttpNotFound();
            }

            /*context.Users.Remove(AccountInDB);*/
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            userId = AccountInDB.Id;
            if (userId != null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                String newPassword = "1234567";
                userManager.AddPassword(userId, newPassword);
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}