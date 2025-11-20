using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class C_PersonalizacionController : ControllerBase
    {
        Personalizacion clsPersonalizacion = new Personalizacion();

        [HttpGet("ListarPersonalizacion")]
        public async Task<IActionResult> ListarPersonalizacion()
        {
            IEnumerable<iPersonalizacion> resultado = null;

            resultado = await clsPersonalizacion.ObtenerListadoDePersonalizacion();

            return Ok(resultado);
        }

        [HttpPut("ActualizaPersonalizacion")]
        public async Task<IActionResult> ActualizaPersonalizacion([FromBody] iPersonalizacion pPersonalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPersonalizacion.ActualizaPersonalizacion(pPersonalizacion);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoDeDirectores")]
        public async Task<IActionResult> ObtenerListadoDeDirectores()
        {
            IEnumerable<iDirectorEjecutivo> resultado = null;

            resultado = await clsPersonalizacion.ObtenerListadoDeDirectores();

            return Ok(resultado);
        }

        [HttpGet("ObtenerParametrosReporteEncuesta")]
        public async Task<IActionResult> ObtenerParametrosReporteEncuesta()
        {
            IEnumerable<iParametrosReporteEncuesta> resultado = null;

            resultado = await clsPersonalizacion.ObtenerParametrosReporteEncuesta();

            return Ok(resultado);
        }

        [HttpPut("ActualizaParametrosReporte")]
        public async Task<IActionResult> ActualizaParametrosReporte([FromBody] iParametrosReporteEncuesta pParametros)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPersonalizacion.ActualizaParametrosReporte(pParametros);

            if (respuesta == "1")
            {
                resultado.valor = respuesta;
                resultado.descripcion = string.Empty;
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }
    }
}
