using SistVentas.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistVentas.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista();
        Task<Usuario> Crear(Usuario entidad,Stream foto=null,string Nombrefoto="",string UrlPlantillaCorreo="");
        Task<Usuario> Editar(Usuario entidad, Stream foto = null, string Nombrefoto = "");
        Task<bool> Eliminar(int IdUsuario);
        Task<Usuario> ObtenerPorCredenciales(string correo,string clave);
        Task<Usuario> ObtenerPorId(int IdUsuario);
        Task<bool> GuardarPerfil(Usuario entidad);
        Task<bool> CambiarClave(int IdUsuario,string claveActual,string claveNueva);
        Task<bool> RestablecerClave(string correo, string UrlPlantillaCorreo);
    }
}
