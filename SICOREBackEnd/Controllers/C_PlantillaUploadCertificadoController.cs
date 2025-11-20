using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_PlantillaUploadCertificadoController : ControllerBase
    {
        CargarCertificadoFirmado _cargarCertificadoFirmado = new CargarCertificadoFirmado();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;

        public C_PlantillaUploadCertificadoController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
        }

        [HttpPost("CargarCertificadoFirmado")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> CargarCertificadoFirmado([FromForm] iSubirCertificadoFirmado pCertificado)
        {
            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;

            if (pCertificado.archivo.Length > 0)
            {
                try
                {
                    string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;

                    string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_CERTIFICADO).Value;
                    string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA_CERTIFICADO).Value;
                    string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                    string nombreArchivoConFormato = pCertificado.cotizacion + "_" + hoy.ToString("yyyyMMdd HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + "." + pCertificado.extension;
                    string nombreArchivo = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\" + nombreArchivoConFormato;

                    string rutafrom = _environment.WebRootPath + "\\" + carpetaParaSubirArchivos + "\\";
                    string rutaCompletaToDesc = rdescargas.Replace("/", "\\");
                    string rutaDelExpedienteConFormato = rutaDelExpediente.Replace("/", "\\");

                    using (FileStream fileStream = System.IO.File.Create(nombreArchivo))
                    {
                        pCertificado.archivo.CopyTo(fileStream);
                        fileStream.Flush();
                        fileStream.Dispose();
                    }

                    bool resultadoValidacion = await RevisaSiElCertificadoEstaFirmado(nombreArchivo);

                    if (resultadoValidacion)
                    {
                        iExpedienteCertificado expediente = new iExpedienteCertificado
                        {
                            fechaGeneracion = "",
                            idCertificado = pCertificado.idCertificado,
                            idCotizacion = 0,
                            idExpediente = 0,
                            idFormalizacion = 0,
                            idFuncionario = pCertificado.idFuncionario,
                            idProyecto = 0,
                            nombreArchivo = nombreArchivoConFormato,
                            rutaFisicaPDF = rutaCompletaToDesc
                        };

                        respuesta.valor = await _cargarCertificadoFirmado.guardaExpediente(expediente, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, pCertificado.cotizacion, rutaDelExpedienteConFormato);
                        respuesta.descripcion = "El documento incluyó exitosamente.";

                    }
                    else
                    {
                        respuesta.valor = "-3";
                        respuesta.descripcion = "Certificado no firmado.";
                    }

                }
                catch (Exception ex)
                {
                    respuesta.valor = "-1";
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

        [HttpPost("SubeArchivosAlExpediente")]
        [Consumes("multipart/form-data")]
        public async Task<Resultado> SubeArchivosAlExpediente([FromForm] iSubirArchivoAlExpediente pArchivoDeExpediente)
        {
            int indice = 0;
            Resultado respuesta = new Resultado();
            DateTime hoy = DateTime.Now;

            if (pArchivoDeExpediente.archivo.Count > 0)
            {
                try
                {
                    foreach (var archivo in pArchivoDeExpediente.archivo)
                    {
                        indice++;

                        string carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;
                        
                        string rdescargas = _config.GetSection(Constantes.APP_SETTINGS_DESCARGA_EXPEDIENTE).Value;
                        string linkDescarga = _config.GetSection(Constantes.APP_SETTINGS_LINK_DESCARGA_EXPEDIENTE).Value;
                        string rutaDelExpediente = _config.GetSection(Constantes.APP_SETTINGS_EXPEDIENTE).Value;

                        string extension = Path.GetExtension(archivo.FileName);
                        string nombreArchivoConFormato = pArchivoDeExpediente.nombreArchivo + '-' + hoy.Year + extension;
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

                        iExpedienteCertificado expediente = new iExpedienteCertificado
                        {
                            fechaGeneracion = "",
                            idCertificado = 0,
                            idCotizacion = 0,
                            idExpediente = 0,
                            idFormalizacion = 0,
                            idFuncionario = pArchivoDeExpediente.idFuncionario,
                            idProyecto = 0,
                            nombreArchivo = nombreArchivoConFormato,
                            rutaFisicaPDF = rutaCompletaToDesc
                        };

                        string resultado = await _cargarCertificadoFirmado.actualizaExpediente(expediente, rutaCompletaToDesc, nombreArchivoConFormato, rutafrom, rutaDelExpedienteConFormato);

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

        private async Task<bool> RevisaSiElCertificadoEstaFirmado (string rutaDelArchivo)
        {
            bool certificadoEsValido = false;
            //WSRevisionFirma.WSArchivosClient servicio = new(WSRevisionFirma.WSArchivosClient.EndpointConfiguration.BasicHttpBinding_IWSArchivos);
            wcf_RevisionFirma.WSArchivosClient servicio = new(wcf_RevisionFirma.WSArchivosClient.EndpointConfiguration.BasicHttpBinding_IWSArchivos);
            await servicio.OpenAsync();

            try
            {
                var obtieneSiCertificadoEstaFirmado = await servicio.VerificarFirmaAsync(rutaDelArchivo);
                await servicio.CloseAsync();
                string respuestaServicio = obtieneSiCertificadoEstaFirmado.ToArray()[0];

                if (respuestaServicio == null)
                {
                    certificadoEsValido = true;
                } else {
                    if (respuestaServicio.IndexOf("no corresponde a PADES LTV") != -1 || respuestaServicio.IndexOf("Sello de Tiempo") != -1)
                    {
                        certificadoEsValido = true;
                    } else {
                        certificadoEsValido = false;
                    }
                }

            } catch (Exception ex) {
                string mensaje = ex.Message;

                return false;
            }

            return certificadoEsValido;
        }
    }
}
