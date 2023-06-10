using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistVentas.AplicacionWeb.Models;
using System.Diagnostics;

using System.Security.Claims;

using AutoMapper;
using SistVentas.AplicacionWeb.Models.ViewModels;
using SistVentas.AplicacionWeb.Utilidades.Response;
using SistVentas.BLL.Interfaces;
using SistVentas.Entity;

namespace SistVentas.AplicacionWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       

        private readonly IUsuarioService _usuarioServicio;
        private readonly IMapper _mapper;

        public HomeController(IUsuarioService usuarioServicio, IMapper mapper)
        {
            _usuarioServicio = usuarioServicio;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPerfil([FromBody] VMUsuario modelo)
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                Usuario entidad = _mapper.Map<Usuario>(modelo);
                entidad.IdUsuario = int.Parse(idUsuario);

                bool resultado = await _usuarioServicio.GuardarPerfil(entidad);

                response.Estado = resultado;

            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;


            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarClave([FromBody] VMCambiarClave modelo)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                bool resultado = await _usuarioServicio.CambiarClave(
                    int.Parse(idUsuario),
                    modelo.ClaveActual,
                    modelo.ClaveNueva
                );


                response.Estado = resultado;

            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;


            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet]
        public async Task <IActionResult> ObtenerUsuario()
        {
            GenericResponse<VMUsuario> response = new GenericResponse<VMUsuario>();
            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                VMUsuario usuario = _mapper.Map<VMUsuario>(await _usuarioServicio.ObtenerPorId(int.Parse(idUsuario)));

                response.Estado = true;
                response.Objeto = usuario;


            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = ex.Message;


            }

            return StatusCode(StatusCodes.Status200OK,response);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task <IActionResult>Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Acceso");
        }
    }
}