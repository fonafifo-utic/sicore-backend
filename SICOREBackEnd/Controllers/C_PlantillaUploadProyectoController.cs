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
    public class C_PlantillaUploadProyectoController : ControllerBase
    {
        PlantillaUploadProyecto _cargarExpedienteProyecto = new PlantillaUploadProyecto();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public C_PlantillaUploadProyectoController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpPost("CargarArchivoProyecto")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> CargarArchivoProyecto([FromForm] iArchivoDeProyecto pArchivoDeExpediente)
        {

            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;

            if (pArchivoDeExpediente.archivo.Length > 0)
            {
                try
                {
                    string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;
                    
                    string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_PROYECTO).Value;
                    string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA_PROYECTO).Value;
                    string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                    string nombreArchivoConFormato = pArchivoDeExpediente.proyecto + "_" + hoy.ToString("yyyyMMdd HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + pArchivoDeExpediente.extension;
                    string nombreArchivo = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\" + nombreArchivoConFormato;

                    string rutafrom = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\";
                    string rutaCompletaToDesc = rdescargas.Replace("/", "\\");

                    string rutaToDescargarProyecto = linkDescarga.Replace("/", "\\"); ;
                    string rutaDelExpedienteConFormato = rutaDelExpediente.Replace("/", "\\");

                    using (FileStream fileStream = System.IO.File.Create(nombreArchivo))
                    {
                        pArchivoDeExpediente.archivo.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Dispose();
                    }

                    iExpediente expediente = new iExpediente
                    {
                        fechaGeneracion = "",
                        idCertificado = 0,
                        idCotizacion = 0,
                        idExpediente = 0,
                        idFormalizacion = "",
                        idFuncionario = pArchivoDeExpediente.idFuncionario,
                        idProyecto = pArchivoDeExpediente.idProyecto,
                        nombreArchivo = nombreArchivoConFormato,
                        rutaFisicaPDF = rutaCompletaToDesc
                    };

                    string resultado = await _cargarExpedienteProyecto.guardaExpediente(expediente, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, rutaToDescargarProyecto, rutaDelExpedienteConFormato);

                    if (resultado == "1")
                    {
                        respuesta.valor = "1";
                        respuesta.descripcion = "El documento se incluyó exitosamente.";
                    }
                    else
                    {
                        respuesta.valor = "-1";
                        respuesta.descripcion = resultado;
                    }
                }
                catch (Exception ex)
                {
                    respuesta.valor = "-3";
                    respuesta.descripcion = ex.Message;
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
