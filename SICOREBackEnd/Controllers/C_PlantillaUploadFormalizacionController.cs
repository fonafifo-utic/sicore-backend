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
    public class C_PlantillaUploadFormalizacionController : ControllerBase
    {
        CargarFacturasFormalizacion _cargarFacturasFormalizacion = new CargarFacturasFormalizacion();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        
        public C_PlantillaUploadFormalizacionController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpPost("CargarArchivosFormalizacion")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> CargaArchivosFormalizacion([FromForm] iArchivoDeFormalizacion pArchivoDeExpediente)
        {

            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;
            int indice = 0;

            if (pArchivoDeExpediente.archivo.Count > 0)
            {
                try
                {
                    foreach(var archivo in pArchivoDeExpediente.archivo)
                    {
                        indice++;
                        string extension = Path.GetExtension(archivo.FileName);

                        string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;
                        
                        string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_FORMALIZACION).Value;
                        string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA_FORMALIZACION).Value;
                        string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                        string rutafrom = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\";
                        string nombreArchivoConFormato = pArchivoDeExpediente.cotizacion + "_" + _poneNombreArchivo(extension, indice);

                        string nombreArchivo = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\" + nombreArchivoConFormato;
                        string rutaCompletaToDesc = rdescargas.Replace("/", "\\");
                        string rutaDelExpedienteConFormato = rutaDelExpediente.Replace("/", "\\");

                        using (FileStream fileStream = System.IO.File.Create(nombreArchivo))
                        {
                            archivo.CopyTo(fileStream);
                            fileStream.Flush();
                            fileStream.Dispose();
                        }

                        iExpediente expediente = new iExpediente
                        {
                            fechaGeneracion = "",
                            idCertificado = 0,
                            idCotizacion = 0,
                            idExpediente = 0,
                            idFormalizacion = pArchivoDeExpediente.idFormalizacion,
                            idFuncionario = pArchivoDeExpediente.idFuncionario,
                            idProyecto = 0,
                            nombreArchivo = nombreArchivoConFormato,
                            rutaFisicaPDF = rutaCompletaToDesc
                        };

                        string resultado = await _cargarFacturasFormalizacion.guardaExpediente(expediente, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, rutaDelExpedienteConFormato);

                        if(resultado == "1")
                        {
                            respuesta.valor = "1";
                            respuesta.descripcion = "El documento se incluyó exitosamente.";
                        } else
                        {
                            respuesta.valor = "-1";
                            respuesta.descripcion = resultado;
                        }
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


        [HttpPost("ActualizaArchivosFormalizacion")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> ActualizaArchivosFormalizacion([FromForm] iArchivoDeFormalizacion pArchivoDeExpediente)
        {

            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;
            int indice = 0;

            if (pArchivoDeExpediente.archivo.Count > 0)
            {
                try
                {
                    foreach (var archivo in pArchivoDeExpediente.archivo)
                    {
                        string extension = Path.GetExtension(archivo.FileName);
                        indice++;

                        string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;

                        string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_FORMALIZACION).Value;
                        string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA_FORMALIZACION).Value;
                        string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                        string nombreArchivoConFormato = pArchivoDeExpediente.cotizacion + "_" + _poneNombreArchivo(extension, indice);
                        string nombreArchivo = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\" + nombreArchivoConFormato;

                        string rutafrom = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\";
                        string rutaCompletaToDesc = rdescargas.Replace("/", "\\");
                        string rutaDelExpedienteConFormato = rutaDelExpediente.Replace("/", "\\");

                        using (FileStream fileStream = System.IO.File.Create(nombreArchivo))
                        {
                            archivo.CopyTo(fileStream);
                            fileStream.Flush();
                            fileStream.Dispose();
                        }

                        iExpediente expediente = new iExpediente
                        {
                            fechaGeneracion = "",
                            idCertificado = 0,
                            idCotizacion = 0,
                            idExpediente = 0,
                            idFormalizacion = pArchivoDeExpediente.idFormalizacion,
                            idFuncionario = pArchivoDeExpediente.idFuncionario,
                            idProyecto = 0,
                            nombreArchivo = nombreArchivoConFormato,
                            rutaFisicaPDF = rutaCompletaToDesc
                        };

                        string resultado = await _cargarFacturasFormalizacion.actualizaExpediente(expediente, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, rutaDelExpedienteConFormato);

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

        private string _poneNombreArchivo(string extension, int indice)
        {
            string nombreArchivo = string.Empty;
            if(extension == ".pdf")
            {
                nombreArchivo = "FACTURA_01" + extension;
            }

            if(extension == ".xml")
            {
                nombreArchivo = "FACTURA_0" + indice.ToString() + extension;
            }

            return nombreArchivo;
        }
    }
}
