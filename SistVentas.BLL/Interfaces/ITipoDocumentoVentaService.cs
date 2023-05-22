using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistVentas.Entity;

namespace SistVentas.BLL.Interfaces
{
    public interface ITipoDocumentoVentaService
    {
        Task<List<TipoDocumentoVenta>> Lista();

    }
}
