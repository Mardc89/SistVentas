using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistVentas.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

using SistVentas.DAL.Interfaces;
using SistVentas.DAL.Implementacion;
using SistVentas.BLL.Interfaces;
using SistVentas.BLL.Implementacion;

namespace SistVentas.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services,IConfiguration Configuration){

            services.AddDbContext<DBVENTASContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GeneryRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IFireBaseService,FireBaseService>();
            services.AddScoped<IUtilidadesService, UtilidadesService>();
            services.AddScoped<IRolService,RolService>();
            services.AddScoped<IUsuarioService,UsuarioService>();
            services.AddScoped<INegocioService,NegocioService>();
            services.AddScoped<ICategoriaService,CategoriaService>();
            services.AddScoped<IProductoService,ProductoService>();
            services.AddScoped<ITipoDocumentoVentaService,TipoDocumentoVentaService>();
            services.AddScoped<IVentaService,VentaService>();
            services.AddScoped<IDashBoardService,DashBoardService>();
            services.AddScoped<IMenuService,MenuService>();

        }
    }
}
