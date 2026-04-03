using System.Linq;
using System.Web.Mvc;
using FemiliftAdmin.Models;

namespace FemiliftAdmin.Controllers
{
    public class BranchController : Controller
    {
        private readonly FemiliftContext db = new FemiliftContext();

        public ActionResult Index()
        {
            var branches = db.Branches
                .Where(b => b.IsVisible)
                .OrderBy(b => b.SortOrder)
                .ToList();
            return View(branches);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
