using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistVentas.AplicacionWeb.Models.ViewModels;
using SistVentas.AplicacionWeb.Utilidades.Response;
using SistVentas.BLL.Interfaces;
using SistVentas.Entity;

using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SistVentas.AplicacionWeb.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly ITipoDocumentoVentaService _tipoDocumentoVentaServicio;
        private readonly IVentaService _ventaServicio;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;

        public VentaController(ITipoDocumentoVentaService tipoDocumentoVentaServicio, IVentaService ventaServicio, IMapper mapper, IConverter converter)
        {
            _tipoDocumentoVentaServicio = tipoDocumentoVentaServicio;
            _ventaServicio = ventaServicio;
            _mapper = mapper;
            _converter = converter;

        }


        public IActionResult HistorialVenta()
        {
            return View();
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<VMTipoDocumentoVenta> vmListaTiposDocumentos = _mapper.Map<List<VMTipoDocumentoVenta>>(await _tipoDocumentoVentaServicio.Lista());

            return StatusCode(StatusCodes.Status200OK,vmListaTiposDocumentos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListaProductos = _mapper.Map<List<VMProducto>>(await _ventaServicio.ObtenerProductos(busqueda));

            return StatusCode(StatusCodes.Status200OK, vmListaProductos);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta> gResponse = new GenericResponse<VMVenta>();

            try
            {
                ClaimsPrincipal claimUser = HttpContext.User;

                string idUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault();

                modelo.IdUsuario =int.Parse(idUsuario);
                Venta venta_creada = await _ventaServicio.Registrar(_mapper.Map<Venta>(modelo));
                modelo = _mapper.Map<VMVenta>(venta_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta,string fechaInicio,string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaServicio.Historial(numeroVenta,fechaInicio,fechaFin));

            return StatusCode(StatusCodes.Status200OK,vmHistorialVenta);
        }


        public IActionResult MostrarPDFVenta(string numeroVenta)
        {

            string urlPlantillaVista = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/PDFVenta?numeroVenta={numeroVenta}";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings(){
                    PaperSize =PaperKind.A4,
                    Orientation=Orientation.Portrait,
                },

                Objects ={
                    new ObjectSettings(){
                     Page=urlPlantillaVista
                }

             }


            };

            var archivoPDF = _converter.Convert(pdf);
            return File(archivoPDF, "application/pdf");
        }

    }
}
