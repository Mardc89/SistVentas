﻿using Microsoft.AspNetCore.Mvc;

using SistVentas.AplicacionWeb.Models.ViewModels;
using SistVentas.BLL.Interfaces;
using SistVentas.Entity;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;



namespace SistVentas.AplicacionWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        public AccesoController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult RestablecerClave()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            Usuario usuario_encontrado = await _usuarioServicio.ObtenerPorCredenciales(modelo.Correo,modelo.Clave);

            if (usuario_encontrado == null){
                ViewData["Mensaje"]= "No se encontraron coincidencias";
                return View();
            }

            ViewData["Mensaje"] = null;

            List<Claim> claims = new List<Claim>(){
                new Claim(ClaimTypes.Name,usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier,usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role,usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto",usuario_encontrado.UrlFoto),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = modelo.MantenerSesion
            };

            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity), properties
            );



            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        public async Task <IActionResult> RestablecerClave(VMUsuarioLogin modelo)
        {

            try
            {
                string urlPlantinaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/RestablecerClave?clave=clave";

                bool resultado = await _usuarioServicio.RestablecerClave(modelo.Correo,urlPlantinaCorreo);

                if (resultado)
                {
                    ViewData["Mensaje"] = "Listo su contraseña fue Restablecida. Recibice su correo";
                    ViewData["MensajeError"] = null;

                }

                else
                {
                    ViewData["MensajeError"] = "Tenemos Problemas.Por intentelo de nuevo más tarde";
                    ViewData["Mensaje"] = null;

                }


            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

    }
}
