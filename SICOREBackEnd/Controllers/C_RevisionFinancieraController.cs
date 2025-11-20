using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_RevisionFinancieraController : ControllerBase
    {
        RevisionFinanciera clsRevisionFinanciera = new RevisionFinanciera();

        [HttpPost("RegistrarUnaFormalizacionVenta")]
        public async Task<IActionResult> RegistrarUnaFormalizacionVenta([FromBody] iFormalizacionParaSalvar pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.registrarUnaFormalizacionVenta(pFormalizacion);

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

        [HttpGet("ObtenerListadoFormalizacion")]
        public async Task<IActionResult> ObtenerListadoFormalizacion()
        {
            IEnumerable<iFormalizacion> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerListadoFormalizacion();

            return Ok(resultado);
        }

        [HttpGet("ObtenerFormalizacionPorId/{idFormalizacion}")]
        public async Task<IActionResult> ObtenerFormalizacionPorId(int idFormalizacion)
        {
            IEnumerable<iFormalizacion> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerFormalizacionPorId(idFormalizacion);

            return Ok(resultado);
        }

        [HttpGet("ObtenerFormalizacionParaVistaPorId/{idFormalizacion}")]
        public async Task<IActionResult> ObtenerFormalizacionParaVistaPorId(string idFormalizacion)
        {
            IEnumerable<iVerUnaFormalizacion> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerFormalizacionParaVistaPorId(idFormalizacion);

            return Ok(resultado);
        }

        [HttpPut("ActualizaUnaFormalizacion")]
        public async Task<IActionResult> ActualizaUnaFormalizacion([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.ActualizaUnaFormalizacion(pFormalizacion);

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

        [HttpPut("ActualizaUnaFormalizacionSinArchivos")]
        public async Task<IActionResult> ActualizaUnaFormalizacionSinArchivos([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.ActualizaUnaFormalizacionSinArchivos(pFormalizacion);

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

        [HttpPut("ActualizaUnaFormalizacionCredito")]
        public async Task<IActionResult> ActualizaUnaFormalizacionCredito([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.ActualizaUnaFormalizacionCredito(pFormalizacion);

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

        [HttpGet("ObtenerRutaFacturaPorId/{idFormalizacion}")]
        public async Task<IActionResult> ObtenerRutaFacturaPorId(string idFormalizacion)
        {
            IEnumerable<iRutaFacturaFormalizacion> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerRutaFacturaPorId(idFormalizacion);

            return Ok(resultado);
        }

        [HttpGet("ObtenerComprobantes")]
        public async Task<IActionResult> ObtenerComprobantes()
        {
            IEnumerable<iFacturasYComprobantes> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerComprobantes();

            return Ok(resultado);
        }

        [HttpGet("ObtenerFacturas")]
        public async Task<IActionResult> ObtenerFacturas()
        {
            IEnumerable<iFacturasYComprobantes> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerFacturas();

            return Ok(resultado);
        }

        [HttpGet("ObtenerNumeroComprobantes")]
        public async Task<IActionResult> ObtenerNumeroComprobantes()
        {
            IEnumerable<iFacturasYComprobantes> resultado = null;

            resultado = await clsRevisionFinanciera.ObtenerNumeroComprobantes();

            return Ok(resultado);
        }

        [HttpPut("CierraUnaFormalizacion")]
        public async Task<IActionResult> CierraUnaFormalizacion([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.CierraUnaFormalizacion(pFormalizacion);

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

        [HttpPost("PeticionActivarRevisionDeFormalizacion")]
        public async Task<IActionResult> PeticionActivarRevisionDeFormalizacion([FromBody] iPeticionActivarFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();

            string respuesta = await clsRevisionFinanciera.peticionActivarRevisionDeFormalizacion(pFormalizacion);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = "ERROR: " + respuesta;
            }

            return Ok(resultado);
        }

        [HttpPost("ActivaRevisionDeFormalizacion")]
        public async Task<IActionResult> ActivaRevisionDeFormalizacion([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();

            string respuesta = await clsRevisionFinanciera.activaRevisionDeFormalizacion(pFormalizacion.idFormalizacion);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = "ERROR: " + respuesta;
            }

            return Ok(resultado);
        }

        [HttpPost("RechazaRevisionDeFormalizacion")]
        public async Task<IActionResult> RechazaRevisionDeFormalizacion([FromBody] iActualizaFormalizacion pFormalizacion)
        {
            Resultado resultado = new Resultado();

            string respuesta = await clsRevisionFinanciera.rechazaRevisionDeFormalizacion(pFormalizacion);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = "ERROR: " + respuesta;
            }

            return Ok(resultado);
        }

        [HttpPost("RegistraUnaFormalizacionAgrupada")]
        public async Task<IActionResult> RegistraUnaFormalizacionAgrupada([FromBody] iFormalizacionParaSalvar pFormalizacion)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsRevisionFinanciera.registraUnaFormalizacionAgrupada(pFormalizacion);

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
