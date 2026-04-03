using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FemiliftAdmin.Filters;
using FemiliftAdmin.Models;
using FemiliftAdmin.Models.ViewModels;

namespace FemiliftAdmin.Controllers.Admin
{
    [AdminAuth]
    [RoutePrefix("Admin/Branches")]
    public class BranchesController : Controller
    {
        private readonly FemiliftContext db = new FemiliftContext();
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };
        private const int MaxImageSize = 2 * 1024 * 1024; // 2MB

        [Route("")]
        public ActionResult Index()
        {
            var branches = db.Branches.OrderBy(b => b.SortOrder).ToList();
            return View("~/Views/Admin/Branches/Index.cshtml", branches);
        }

        [Route("Create")]
        public ActionResult Create()
        {
            return View("~/Views/Admin/Branches/Create.cshtml");
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Branches/Create.cshtml", model);

            var branch = new Branch
            {
                Name = model.Name,
                Phone = model.Phone,
                Hours = model.Hours,
                Address = model.Address,
                MapUrl = model.MapUrl,
                IsVisible = true,
                SortOrder = db.Branches.Any() ? db.Branches.Max(b => b.SortOrder) + 1 : 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            if (model.ImageFile != null)
            {
                var imagePath = SaveImage(model.ImageFile);
                if (imagePath == null)
                {
                    ModelState.AddModelError("ImageFile", "僅支援 jpg、png、webp 格式，大小不超過 2MB");
                    return View("~/Views/Admin/Branches/Create.cshtml", model);
                }
                branch.ImagePath = imagePath;
            }

            db.Branches.Add(branch);
            db.SaveChanges();

            TempData["Success"] = "院所已新增";
            return RedirectToAction("Index");
        }

        [Route("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            var branch = db.Branches.Find(id);
            if (branch == null) return HttpNotFound();

            var model = new BranchEditViewModel
            {
                Id = branch.Id,
                Name = branch.Name,
                Phone = branch.Phone,
                Hours = branch.Hours,
                Address = branch.Address,
                MapUrl = branch.MapUrl,
                ExistingImagePath = branch.ImagePath,
                IsVisible = branch.IsVisible
            };
            return View("~/Views/Admin/Branches/Edit.cshtml", model);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BranchEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/Branches/Edit.cshtml", model);

            var branch = db.Branches.Find(id);
            if (branch == null) return HttpNotFound();

            branch.Name = model.Name;
            branch.Phone = model.Phone;
            branch.Hours = model.Hours;
            branch.Address = model.Address;
            branch.MapUrl = model.MapUrl;
            branch.IsVisible = model.IsVisible;
            branch.UpdatedAt = DateTime.Now;

            if (model.RemoveImage)
            {
                DeleteImage(branch.ImagePath);
                branch.ImagePath = null;
            }

            if (model.ImageFile != null)
            {
                var imagePath = SaveImage(model.ImageFile);
                if (imagePath == null)
                {
                    ModelState.AddModelError("ImageFile", "僅支援 jpg、png、webp 格式，大小不超過 2MB");
                    model.ExistingImagePath = branch.ImagePath;
                    return View("~/Views/Admin/Branches/Edit.cshtml", model);
                }
                DeleteImage(branch.ImagePath);
                branch.ImagePath = imagePath;
            }

            db.SaveChanges();

            TempData["Success"] = "院所已更新";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var branch = db.Branches.Find(id);
            if (branch == null) return HttpNotFound();

            DeleteImage(branch.ImagePath);
            db.Branches.Remove(branch);
            db.SaveChanges();

            TempData["Success"] = "院所已刪除";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Toggle/{id}")]
        public ActionResult Toggle(int id)
        {
            var branch = db.Branches.Find(id);
            if (branch == null) return HttpNotFound();

            branch.IsVisible = !branch.IsVisible;
            branch.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            return Json(new { success = true, isVisible = branch.IsVisible });
        }

        [HttpPost]
        [Route("Reorder")]
        public ActionResult Reorder(List<int> ids)
        {
            if (ids == null) return new HttpStatusCodeResult(400);

            for (int i = 0; i < ids.Count; i++)
            {
                var branch = db.Branches.Find(ids[i]);
                if (branch != null)
                {
                    branch.SortOrder = i;
                }
            }
            db.SaveChanges();

            return Json(new { success = true });
        }

        private string SaveImage(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0) return null;
            if (file.ContentLength > MaxImageSize) return null;

            var ext = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(ext)) return null;

            var fileName = Guid.NewGuid() + ext;
            var relativePath = "uploads/branches/" + fileName;
            var absolutePath = Path.Combine(Server.MapPath("~/uploads/branches"), fileName);

            var dir = Path.GetDirectoryName(absolutePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            file.SaveAs(absolutePath);
            return relativePath;
        }

        private void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var absolutePath = Server.MapPath("~/" + imagePath);
            if (System.IO.File.Exists(absolutePath))
            {
                System.IO.File.Delete(absolutePath);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
