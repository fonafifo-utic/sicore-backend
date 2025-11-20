using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
    public class iSubirCertificadoFirmado
    {
        public int idCertificado { get; set; }
        public int idFuncionario { get; set; }
        public string cotizacion { get; set; }
        public string extension { get; set; }
        public IFormFile archivo { get; set; }
    }

    public class iSubirArchivoAlExpediente
    {
        public int idFuncionario { get; set; }
        public string nombreArchivo { get; set; }
        public string extension { get; set; }
        public IList<IFormFile> archivo { get; set; }
    }

    public class iExpedienteCertificado
    {
        public int idExpediente { get; set; }
        public int idProyecto { get; set; }
        public int idCotizacion { get; set; }
        public int idFormalizacion { get; set; }
        public int idCertificado { get; set; }
        public int idFuncionario { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaFisicaPDF { get; set; }
        public string fechaGeneracion { get; set; }
    }

    public class CargarCertificadoFirmado
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<string> guardaExpediente(iExpedienteCertificado pExpediente, string rutaDescarga, string nombreArchivoConFormato, string rutaFrom, string numeroCertificado, string rutaDelExpediente)
        {
            string resultado = string.Empty;
            string objJsonDeExpediente = Newtonsoft.Json.JsonConvert.SerializeObject(pExpediente);
            DateTime hoy = new DateTime();
            int anno = hoy.Year;
            string resultadoEnvio = string.Empty;

            var enviarNotificacion = new iEnviaFormalizacion()
            {
                asunto = "Notificación SICORE",
                destinatario = pExpediente.idCertificado.ToString(),
                idFuncionario = pExpediente.idFuncionario,
                numeroFormalizacion = numeroCertificado
            };

            string objJsonNotificacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarNotificacion);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var expedienteParaIngresar = new { @pExpediente = objJsonDeExpediente };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_CERTIFICADO_INGRESA_EXPEDIENTE",
                        expedienteParaIngresar, commandType: CommandType.StoredProcedure);
                }

                if (resultado == "1")
                {
                    using (SqlConnection conexion = new SqlConnection(strconSICORE))
                    {
                        var notificacionParaEnviar = new { @pFormalizacion = objJsonNotificacion };
                        resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_ENVIAR_CERTIFICADO_YAFUE_FIRMADO",
                            notificacionParaEnviar, commandType: CommandType.StoredProcedure);
                    }

                    if (Directory.Exists(rutaDescarga))
                    {
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(rutaDescarga);
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                    }

                    if (Directory.Exists(rutaDelExpediente))
                    {
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(rutaDelExpediente);
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                    }
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> actualizaExpediente(iExpedienteCertificado pExpediente, string rutaDescarga, string nombreArchivoConFormato, string rutaFrom, string rutaDelExpediente)
        {
            string resultado = string.Empty;
            string objJsonDeExpediente = Newtonsoft.Json.JsonConvert.SerializeObject(pExpediente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var expedienteParaIngresar = new { @pExpediente = objJsonDeExpediente };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_CERTIFICADO_ACTUALIZA_EXPEDIENTE",
                        expedienteParaIngresar, commandType: CommandType.StoredProcedure);
                }

                if (resultado == "1")
                {
                    if (Directory.Exists(rutaDescarga))
                    {
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(rutaDescarga);
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                    }

                    if (Directory.Exists(rutaDelExpediente))
                    {
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(rutaDelExpediente);
                        File.Copy(rutaFrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                    }
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = mensaje;

                return resultado;
            }

            return resultado;
        }

        private string poneCerosFormalizacion(int consecutivo)
        {
            string nuevoConsecutivo = string.Empty;

            switch (consecutivo.ToString().Length)
            {
                case 1:
                    nuevoConsecutivo = "00" + consecutivo.ToString();
                    break;

                case 2:
                    nuevoConsecutivo = "0" + consecutivo.ToString();
                    break;

                case 3:
                    nuevoConsecutivo = consecutivo.ToString();
                    break;
            }

            return nuevoConsecutivo;
        }

    }
}
