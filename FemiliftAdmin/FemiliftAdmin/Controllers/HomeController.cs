using System.Web.Mvc;

namespace FemiliftAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return File(Server.MapPath("~/index.html"), "text/html");
        }
    }
}
