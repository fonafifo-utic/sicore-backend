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

    public class iArchivoDeFormalizacion
    {
        public string idFormalizacion { get; set; }
        public int idFuncionario { get; set; }
        public string cotizacion { get; set; }
        public IList<IFormFile> archivo { get; set; }
    }

    public class iExpediente
    {
        public int idExpediente { get; set; }
        public int idProyecto { get; set; }
        public int idCotizacion { get; set; }
        public string idFormalizacion { get; set; }
        public int idCertificado { get; set; }
        public int idFuncionario { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaFisicaPDF { get; set; }
        public string fechaGeneracion { get; set; }
    }

    public class CargarFacturasFormalizacion
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<string> guardaExpediente(iExpediente pExpediente, string rutaDescarga, string nombreArchivoConFormato, string rutaFrom, string rutaDelExpediente)
        {
            string resultado = "1";
            string objJsonDeExpediente = Newtonsoft.Json.JsonConvert.SerializeObject(pExpediente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var expedienteParaIngresar = new { @pExpediente = objJsonDeExpediente };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_INGRESA_EXPEDIENTE",
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

        public async Task<string> actualizaExpediente(iExpediente pExpediente, string rutaDescarga, string nombreArchivoConFormato, string rutaFrom, string rutaDelExpediente)
        {
            string resultado = string.Empty;
            string objJsonDeExpediente = Newtonsoft.Json.JsonConvert.SerializeObject(pExpediente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var expedienteParaIngresar = new { @pExpediente = objJsonDeExpediente };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_ACTUALIZA_EXPEDIENTE",
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
    }
}
