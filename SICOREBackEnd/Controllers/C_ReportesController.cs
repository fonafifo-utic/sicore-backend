using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_ReportesController : ControllerBase
    {
        Reportes reporte = new Reportes();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public C_ReportesController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpPost("TraeListadoMensualCertificados")]
        public async Task<IActionResult> TraeListadoMensualCertificados([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoCertificadoMensual> resultado = null;

            resultado = await reporte.TraeListadoMensualCertificados(rangoFechas);

            return Ok(resultado);
        }

        [HttpPost("TraeListadoMensualCotizaciones")]
        public async Task<IActionResult> TraeListadoMensualCotizaciones([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoCotizacionesMensual> resultado = null;

            resultado = await reporte.TraeListadoMensualCotizaciones(rangoFechas);

            return Ok(resultado);
        }

        [HttpPost("TraeListadoMensualFormalizaciones")]
        public async Task<IActionResult> TraeListadoMensualFormalizaciones([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoFormalizacionMensual> resultado = null;

            resultado = await reporte.TraeListadoMensualFormalizaciones(rangoFechas);

            return Ok(resultado);
        }

        [HttpPost("TraeListadoVentas")]
        public async Task<IActionResult> TraeListadoVentas([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoVentas> resultado = null;

            resultado = await reporte.TraeListadoVentas(rangoFechas);

            return Ok(resultado);
        }

        [HttpGet("TraeTodosLosSectoresActivos")]
        public async Task<IActionResult> TraeTodosLosSectoresActivos ()
        {
            IEnumerable<iSectoresComerciales> sectores = await reporte.TraeTodosLosSectoresActivos();

            return Ok(sectores);
        }

        [HttpPost("TraeSectoresPorRangoFecha")]
        public async Task<IActionResult> TraeSectoresPorRangoFecha([FromBody] iRangoFechaBusqueda rangoFechas)
        {

            IEnumerable<iSectoresComerciales> resultado = await reporte.traeSectoresPorRangoFecha(rangoFechas);

            return Ok(resultado);
        }
    }
}
