

using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using SICOREBackEnd.Utils;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_CertificadoController : ControllerBase
    {
        Certificado clsCertificado = new Certificado();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public C_CertificadoController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpGet("ListarCertificado")]
        public async Task<IActionResult> ListarCertificado()
        {
            IEnumerable<iCertificado> resultado = null;

            resultado = await clsCertificado.ObtenerListadoCertificados();

            return Ok(resultado);
        }

        [HttpGet("ListarCertificadosAprobados")]
        public async Task<IActionResult> ListarCertificadosAprobados()
        {
            IEnumerable<iCertificado> resultado = null;

            resultado = await clsCertificado.listarCertificadosAprobados();

            return Ok(resultado);
        }

        [HttpGet("ListaCertificadoPorId/{idCertificado}")]
        public async Task<IActionResult> ListaCertificadoPorId(string idCertificado)
        {
            IEnumerable<iVistaCertificado> resultado = null;

            resultado = await clsCertificado.ObtenerCertificadoPorId(idCertificado);

            return Ok(resultado);
        }

        [HttpPost("IngresaCertificado")]
        public async Task<IActionResult> IngresaCertificado([FromBody] iCertificado pCertificado)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCertificado.IngresaCertificado(pCertificado);

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

        [HttpPut("ActualizaCertificado")]
        public async Task<IActionResult> ActualizaCertificado([FromBody] iCertificado pCertificado)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCertificado.ActualizaCertificado(pCertificado);

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

        [HttpGet("TraeRutaCertificadoPorId/{idCertificado}")]
        public async Task<IActionResult> TraeRutaCertificadoPorId(int idCertificado)
        {
            IEnumerable<iRutaCertificado> resultado = null;

            resultado = await clsCertificado.ObtenerRutaCertificadoPorId(idCertificado);

            return Ok(resultado);
        }

        [HttpPost("EnviaUnCertificado")]
        public async Task<IActionResult> EnviaUnCertificado([FromBody] iOpcionesParaEnviarCertificado pCertificado)
        {
            Resultado resultado = new Resultado();

            string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_CERTIFICADO).Value;
            string rutaCompletaToDesc = rdescargas.Replace("/", "\\");

            var certificado = new iOpcionesParaEnviarCertificado()
            {
                asunto = "Certificado número: " + pCertificado.numeroCertificado,
                destinatario = pCertificado.destinatario,
                enlace = pCertificado.enlaceEncuesta,
                idFuncionario = pCertificado.idFuncionario,
                idCotizacion = pCertificado.idCotizacion,
                numeroCertificado = pCertificado.numeroCertificado,
                enviaEncuesta = pCertificado.enviaEncuesta
            };

            string respuesta = await clsCertificado.EnviaCertificado(certificado);

            if (respuesta == "1")
            {
                resultado.valor = "1";
                resultado.descripcion = "El certificado seleccionado se adjuntó exitosamente para ser enviado.";
            }
            else
            {
                resultado.valor = "-1";
                resultado.descripcion = respuesta;
            }

            return Ok(resultado);
        }

        [HttpGet("ObtieneRutaElementosExpediente")]
        public async Task<IActionResult> ObtieneRutaElementosExpediente()
        {
            IEnumerable<iRutaCertificado> resultado = null;

            resultado = await clsCertificado.ObtieneRutaElementosExpediente();

            return Ok(resultado);
        }

        [HttpPut("PoneObservacionesAlCertificado")]
        public async Task<IActionResult> PoneObservacionesAlCertificado([FromBody] iPoneObservacionesAlCertificado pCertificado)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCertificado.PoneObservacionesAlCertificado(pCertificado);

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

        [HttpPut("ApruebaCertificado")]
        public async Task<IActionResult> ApruebaCertificado([FromBody] iPoneObservacionesAlCertificado pCertificado)
        {
            Resultado resultado = new Resultado();
            string respuesta = await clsCertificado.apruebaCertificado(pCertificado);

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
