using System.Linq;
using System.Web.Mvc;
using FemiliftAdmin.Filters;
using FemiliftAdmin.Models;
using FemiliftAdmin.Models.ViewModels;

namespace FemiliftAdmin.Controllers.Admin
{
    [RoutePrefix("Admin/Account")]
    public class AccountController : Controller
    {
        private readonly FemiliftContext db = new FemiliftContext();

        [Route("Login")]
        public ActionResult Login()
        {
            if (Session["AdminUserId"] != null)
                return RedirectToAction("Index", "Branches", new { area = "" });

            return View("~/Views/Admin/Account/Login.cshtml");
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Account/Login.cshtml", model);

            var user = db.AdminUsers.FirstOrDefault(u => u.Username == model.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                Session["AdminUserId"] = user.Id;
                Session["AdminUsername"] = user.Username;
                return RedirectToAction("Index", "Branches");
            }

            ModelState.AddModelError("", "帳號或密碼錯誤");
            return View("~/Views/Admin/Account/Login.cshtml", model);
        }

        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }

        [AdminAuth]
        [Route("ChangePassword")]
        public ActionResult ChangePassword()
        {
            return View("~/Views/Admin/Account/ChangePassword.cshtml");
        }

        [HttpPost]
        [AdminAuth]
        [Route("ChangePassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Account/ChangePassword.cshtml", model);

            var userId = (int)Session["AdminUserId"];
            var user = db.AdminUsers.Find(userId);

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError("CurrentPassword", "目前密碼不正確");
                return View("~/Views/Admin/Account/ChangePassword.cshtml", model);
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            db.SaveChanges();

            TempData["Success"] = "密碼已成功更新";
            return RedirectToAction("ChangePassword");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
