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
    public class C_CotizacionController : ControllerBase
    {
        Cotizacion clsCotizacion = new Cotizacion();

        [HttpGet("ListarCotizacion")]
        public async Task<IActionResult> ListarCotizacion()
        {
            IEnumerable<iCotizacion> resultado = null;

            resultado = await clsCotizacion.ObtenerListadoCotizacion();

            return Ok(resultado);
        }

        [HttpGet("ListaCotizacionPorId/{id}")]
        public async Task<IActionResult> ListaCotizacionPorId(int id)
        {
            IEnumerable<iCotizacion> resultado = null;

            resultado = await clsCotizacion.ObtenerCotizacionPorId(id);

            return Ok(resultado);
        }

        [HttpPost("IngresaCotizacion")]
        public async Task<IActionResult> IngresaCotizacion([FromBody] iCotizacionParaSalvar pCotizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCotizacion.IngresaCotizacion(pCotizacion);

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

        [HttpPut("ActualizaCotizacion")]
        public async Task<IActionResult> ActualizaCotizacion([FromBody] iCotizacionParaSalvar pCotizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCotizacion.ActualizaCotizacion(pCotizacion);

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

        [HttpPost("AnulaCotizacion")]
        public async Task<IActionResult> AnulaCotizacion([FromBody] iAnulaCotizacion pCotizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCotizacion.AnulaCotizacion(pCotizacion);

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

        [HttpPost("ValidaUnaCotizacion")]
        public async Task<IActionResult> ValidaUnaCotizacion([FromBody] iValidaCotizacion pCotizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCotizacion.ValidaUnaCotizacion(pCotizacion);

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

        [HttpPost("RechazaUnaCotizacion")]
        public async Task<IActionResult> RechazaUnaCotizacion([FromBody] iValidaCotizacion pCotizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCotizacion.RechazaUnaCotizacion(pCotizacion);

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
