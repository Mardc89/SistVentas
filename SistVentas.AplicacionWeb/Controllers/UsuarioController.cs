using Microsoft.AspNetCore.Mvc;

namespace SistVentas.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
