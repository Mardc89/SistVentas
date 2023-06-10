using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SistVentas.AplicacionWeb.Models.ViewModels;
using SistVentas.AplicacionWeb.Utilidades.Response;
using SistVentas.BLL.Interfaces;


namespace SistVentas.AplicacionWeb.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        private readonly IDashBoardService _dasboardServicio;

        public DashBoardController(IDashBoardService dasboardServicio)
        {
            _dasboardServicio = dasboardServicio;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task <IActionResult>ObtenerResumen()
        {
            GenericResponse<VMDashBoard> gResponse = new GenericResponse<VMDashBoard>();

            try
            {
                VMDashBoard vmDashboard = new VMDashBoard();
                vmDashboard.TotalVentas = await _dasboardServicio.TotalVentasUltimaSemana();
                vmDashboard.TotalIngresos = await _dasboardServicio.TotalIngresosUltimaSemana();
                vmDashboard.TotalProductos = await _dasboardServicio.TotalProductos();
                vmDashboard.TotalCategorias = await _dasboardServicio.TotalCategorias();

                List<VMVentasSemana> listaVentasSemana = new List<VMVentasSemana>();
                List<VMProductosSemana> listaProductosSemana = new List<VMProductosSemana>();

                foreach (KeyValuePair<string, int> item in await _dasboardServicio.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VMVentasSemana()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });

                }

                foreach (KeyValuePair<string, int> item in await _dasboardServicio.ProductosTopUltimaSemana())
                {
                    listaProductosSemana.Add(new VMProductosSemana()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });

                }

                vmDashboard.VentasUltimaSemana = listaVentasSemana;
                vmDashboard.ProductosTopUltimaSemana = listaProductosSemana;

                gResponse.Estado = true;
                gResponse.Objeto = vmDashboard;

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje =ex.Message;


            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
                 
        }
    }
}
