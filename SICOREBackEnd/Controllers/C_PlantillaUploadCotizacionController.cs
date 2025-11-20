using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_PlantillaUploadCotizacionController : ControllerBase
    {
        EnviarCotizacion _enviarCotizacion = new EnviarCotizacion();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public C_PlantillaUploadCotizacionController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpPost("CargarAutoCotizacion")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> CargarAutoCotizacion([FromForm] iArchivoCotizacion cotizacionToEnviar)
        {
            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;

            if (cotizacionToEnviar.archivo.Length > 0)
            {
                try
                {
                    string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;

                    string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_COTIZACION).Value;
                    string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA).Value;
                    string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                    string nombreArchivoConFormato = cotizacionToEnviar.consecutivo + "_" + hoy.ToString("yyyyMMdd HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    string nombreArchivo = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\" + nombreArchivoConFormato;

                    string rutafrom = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\";
                    string rutaCompletaToDesc = rdescargas.Replace("/", "\\");
                    string rutaDelExpedienteConFormato = rutaDelExpediente.Replace("/", "\\");

                    using (FileStream fileStream = System.IO.File.Create(nombreArchivo))
                    {
                        cotizacionToEnviar.archivo.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Dispose();
                    }

                    var cotizacion = new iCotizacionToEnviar()
                    {
                        asunto = "Cotización número: " + cotizacionToEnviar.consecutivo,
                        destinatario = cotizacionToEnviar.destinatario,
                        enlace = linkDescarga,
                        idCliente = cotizacionToEnviar.idCliente,
                        idFuncionario = cotizacionToEnviar.idFuncionario,
                        numeroCotizacion = cotizacionToEnviar.consecutivo,
                        idCotizacion = cotizacionToEnviar.idCotizacion
                    };
                    
                    await _enviarCotizacion.guardarCotizacionPDF(cotizacion, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, rutaDelExpedienteConFormato);

                    respuesta.valor = "1";
                    respuesta.descripcion = "El documento seleccionado se adjuntó exitosamente para ser enviado.";
                }
                catch (Exception e)
                {
                    respuesta.valor = "-1";
                    respuesta.descripcion = e.Message;
                }
            }
            else
            {
                respuesta.valor = "-2";
                respuesta.descripcion = "ATENCIÓN: No se seleccionó ningún documento para adjuntar.";
            }

            return respuesta;
        }

    }
}
