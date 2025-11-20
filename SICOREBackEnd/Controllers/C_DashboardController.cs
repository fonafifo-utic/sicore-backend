using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_DashboardController : ControllerBase
    {
        Dashboard clsProyecto = new Dashboard();

        [HttpGet("ListarProyectos")]
        public async Task<IActionResult> ListarProyectos()
        {
            IEnumerable<iProyectosDashboard> resultado = null;

            resultado = await clsProyecto.ObtenerProyectos();

            return Ok(resultado);
        }

        [HttpGet("ObtenerResumenVentas")]
        public async Task<IActionResult> ObtenerResumenVentas()
        {
            IEnumerable<iResumenVentasDashboard> resultado = null;

            resultado = await clsProyecto.ObtenerResumenVentas();

            return Ok(resultado);
        }

        [HttpGet("ObtenerResumenCotizaciones")]
        public async Task<IActionResult> ObtenerResumenCotizaciones()
        {
            IEnumerable<iResumenCotizaciones> resultado = null;

            resultado = await clsProyecto.ObtenerResumenCotizaciones();

            return Ok(resultado);
        }

        [HttpGet("ObtenerResumenVentasPorProyecto")]
        public async Task<IActionResult> ObtenerResumenVentasPorProyecto()
        {
            IEnumerable<iResumenVentas> resultado = null;

            resultado = await clsProyecto.ObtenerResumenVentasPorProyecto();

            return Ok(resultado);
        }

        [HttpGet("ObtenerResumenInventarioPorProyecto")]
        public async Task<IActionResult> ObtenerResumenInventarioPorProyecto()
        {
            IEnumerable<iResumenInventario> resultado = null;

            resultado = await clsProyecto.ObtenerResumenInventarioPorProyecto();

            return Ok(resultado);
        }

        [HttpGet("ObtenerVentasPorMesParaGrafico")]
        public async Task<IActionResult> ObtenerVentasPorMesParaGrafico()
        {
            IEnumerable<iResumenVentasPorMes> resultado = await clsProyecto.ObtenerVentasPorMesParaGrafico();

            return Ok(resultado);
        }
    }
}
