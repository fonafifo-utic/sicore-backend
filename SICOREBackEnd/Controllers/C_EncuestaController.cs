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
    public class C_EncuestaController : ControllerBase
    {
        Encuesta clsPregunta = new Encuesta();

        [HttpPost("IngresaUnaPregunta")]
        public async Task<IActionResult> IngresaUnaPregunta([FromBody] iPregunta pPregunta)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPregunta.IngresaUnaPregunta(pPregunta);

            if (respuesta == "1" || respuesta == "2")
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

        [HttpGet("ListarPreguntas")]
        public async Task<IActionResult> ListarPreguntas()
        {
            IEnumerable<iPreguntas> resultado = null;

            resultado = await clsPregunta.ObtenerListadoPreguntas();

            return Ok(resultado);
        }

        [HttpGet("ListarPreguntasPorId/{idPregunta}")]
        public async Task<IActionResult> ListarPreguntas(int idPregunta)
        {
            IEnumerable<iPreguntas> resultado = null;

            resultado = await clsPregunta.ObtenerPreguntasPorId(idPregunta);

            return Ok(resultado);
        }

        [HttpGet("ListaRespuestasPorId/{idPregunta}")]
        public async Task<IActionResult> ListaRespuestasPorId(int idPregunta)
        {
            IEnumerable<iRespuestas> resultado = null;

            resultado = await clsPregunta.ObtenerRespuestasPorId(idPregunta);

            return Ok(resultado);
        }

        [HttpPost("IngresaUnaEncuesta")]
        public async Task<IActionResult> IngresaUnaEncuesta([FromBody] iEncuesta [] pEncuesta)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPregunta.IngresaUnaEncuesta(pEncuesta);

            if (respuesta == "1" || respuesta == "2")
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

        [HttpPut("ActualizaPregunta")]
        public async Task<IActionResult> ActualizaPregunta([FromBody] iPregunta pPregunta)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPregunta.ActualizaPregunta(pPregunta);

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

        [HttpGet("ObtenerEncuesta")]
        public async Task<IActionResult> ObtenerEncuesta()
        {
            IEnumerable<iVistaEncuesta> resultado = null;

            resultado = await clsPregunta.ObtenerEncuesta();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListaEncuesta")]
        public async Task<IActionResult> ObtenerListaEncuesta()
        {
            IEnumerable<iListaEncuesta> resultado = null;

            resultado = await clsPregunta.ObtenerListaEncuesta();

            return Ok(resultado);
        }

        [HttpGet("ObtieneEncuestaPorIdCliente/{idCliente}")]
        public async Task<IActionResult> ObtieneEncuestaPorIdCliente(int idCliente)
        {
            IEnumerable<iVistaEncuesta> resultado = null;

            resultado = await clsPregunta.ObtieneEncuestaPorIdCliente(idCliente);

            return Ok(resultado);
        }

        [HttpPost("IngresEncuestaHechaPorCliente")]
        public async Task<IActionResult> IngresEncuestaHechaPorCliente([FromBody] iRespuestasEncuesta[] pEncuesta)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsPregunta.IngresEncuestaHechaPorCliente(pEncuesta);

            if (respuesta == "1" || respuesta == "2")
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

        [HttpGet("ObtenerListadoRespuestasRating")]
        public async Task<IActionResult> ObtenerListadoRespuestasRating()
        {
            IEnumerable<iRespuestasTipoRating> resultado = null;

            resultado = await clsPregunta.ObtenerListadoRespuestasRating();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoRespuestasSeleccion")]
        public async Task<IActionResult> ObtenerListadoRespuestasSeleccion()
        {
            IEnumerable<iRespuestasTipoSeleccion> resultado = null;

            resultado = await clsPregunta.ObtenerListadoRespuestasSeleccion();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoRespuestasEnviadasMes")]
        public async Task<IActionResult> ObtenerListadoRespuestasEnviadasMes()
        {
            IEnumerable<iRespuestasListadoMes> resultado = null;

            resultado = await clsPregunta.ObtenerListadoRespuestasEnviadasMes();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoEnviadas")]
        public async Task<IActionResult> ObtenerListadoEnviadas()
        {
            IEnumerable<iEncuestaEnviada> resultado = null;

            resultado = await clsPregunta.ObtenerListadoEnviadas();

            return Ok(resultado);
        }

        [HttpGet("ObtenerListadoPendientes")]
        public async Task<IActionResult> ObtenerListadoPendientes()
        {
            IEnumerable<iEncuestaPendiente> resultado = null;

            resultado = await clsPregunta.ObtenerListadoPendientes();

            return Ok(resultado);
        }

        [HttpGet("ObtieneRespuestaEncuestaPorIdCliente/{idCliente}")]
        public async Task<IActionResult> ObtieneRespuestaEncuestaPorIdCliente(int idCliente)
        {
            IEnumerable<iRespuestaEncuestaEnviada> resultado = null;

            resultado = await clsPregunta.ObtieneRespuestaEncuestaPorIdCliente(idCliente);

            return Ok(resultado);
        }

        [HttpPost("ReEnviaEncuesta")]
        public async Task<IActionResult> ReEnviaEncuesta([FromBody] iOpcionesParaEnviarCertificado pCertificado)
        {
            Resultado resultado = new Resultado();

            var certificado = new iOpcionesParaEnviarCertificado()
            {
                asunto = "Encuesta de Satisfacción",
                destinatario = pCertificado.destinatario,
                enlace = pCertificado.enlaceEncuesta,
                idFuncionario = pCertificado.idFuncionario,
                idCotizacion = pCertificado.idCotizacion,
                numeroCertificado = pCertificado.numeroCertificado
            };

            string respuesta = await clsPregunta.ReEnviaEncuesta(certificado);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "La encuesta se envío adecuadamente.";
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
