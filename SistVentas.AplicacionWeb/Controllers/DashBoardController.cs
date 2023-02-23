using Microsoft.AspNetCore.Mvc;

namespace SistVentas.AplicacionWeb.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
