using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace SistVentas.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvoKeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            string nombreUsuario = "";
            string urlfotoUsuario = "";

            if (claimUser.Identity.IsAuthenticated){

                nombreUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

                urlfotoUsuario = ((ClaimsIdentity)claimUser.Identity).FindFirst("UrlFoto").Value;

            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["urlFotoUsuario"] =urlfotoUsuario;


            return View();

        }
    }
}
