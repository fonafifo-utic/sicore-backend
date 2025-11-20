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
    public class iArchivoCotizacion
    {
        public int idCotizacion { get; set; }
        public int idFuncionario { get; set; }
        public int idCliente { get; set; }
        public string destinatario { get; set; }
        public string consecutivo { get; set; }
        public IFormFile archivo { get; set; }
    }
    public class iCotizacionToEnviar
    {
        public string asunto { get; set; }
        public string destinatario { get; set; }
        public string enlace { get; set; }
        public string numeroCotizacion { get; set; }
        public int idFuncionario { get; set; }
        public int idCliente { get; set; }
        public int idCotizacion { get; set; }
    }   
    public class iOpcionesDeEnvio
    {
        public string asunto { get; set; }
        public string destinatario { get; set; }
        public string enlace { get; set; }
        public string numeroCotizacion { get; set; }
        public int idFuncionario { get; set; }
        public int idCotizacion { get; set; }
    }
    public class iTrazaCotizacionEnviada
    {
        public int idFuncionario { get; set; }
        public int idCotizacion { get; set; }
        public int idCliente { get; set; }
        public string numeroCotizacion { get; set; }
    }

    public class EnviarCotizacion
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();
        public async Task<string> guardarCotizacionPDF(iCotizacionToEnviar pCotizacion, string rutaDescarga, string nombreArchivoConFormato, string rutafrom, string rutaDelExpediente)
        {
            string resultadoPonerEnCola = string.Empty;
            string resultadoTraza = string.Empty;
            string resultado = string.Empty;

            var opciones = new iOpcionesDeEnvio()
            {
                asunto = pCotizacion.asunto,
                destinatario = pCotizacion.destinatario,
                enlace = pCotizacion.enlace + nombreArchivoConFormato,
                numeroCotizacion = pCotizacion.numeroCotizacion,
                idFuncionario = pCotizacion.idFuncionario,
                idCotizacion = pCotizacion.idCotizacion
            };

            string opcionesDeEnvio = Newtonsoft.Json.JsonConvert.SerializeObject(opciones);

            var traza = new iTrazaCotizacionEnviada()
            {
                numeroCotizacion = pCotizacion.numeroCotizacion,
                idCliente = pCotizacion.idCliente,
                idFuncionario = pCotizacion.idFuncionario,
                idCotizacion = pCotizacion.idCotizacion
            };

            string trazaCotizacion = Newtonsoft.Json.JsonConvert.SerializeObject(traza);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var opcionesEnvio = new { @pOpcionesEnvio = opcionesDeEnvio };
                    resultadoPonerEnCola = await conexion.ExecuteScalarAsync<string>("PA_ENVIAR_COTIZACION", opcionesEnvio, commandType: CommandType.StoredProcedure);

                    var trazaToIngresar = new { @pCotizacion = trazaCotizacion };
                    resultadoTraza = await conexion.ExecuteScalarAsync<string>("PA_COTIZACION_INGRESA_TRAZA_COTIZACION_ENVIADA", trazaToIngresar, commandType: CommandType.StoredProcedure);

                    if (resultadoPonerEnCola == "1" && resultadoTraza == "1")
                    {
                        if (Directory.Exists(rutaDescarga))
                        {
                            File.Copy(rutafrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                        }
                        else
                        {
                            Directory.CreateDirectory(rutaDescarga);
                            File.Copy(rutafrom + nombreArchivoConFormato, rutaDescarga + nombreArchivoConFormato, true);
                        }

                        if (Directory.Exists(rutaDelExpediente))
                        {
                            File.Copy(rutafrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                        }
                        else
                        {
                            Directory.CreateDirectory(rutaDelExpediente);
                            File.Copy(rutafrom + nombreArchivoConFormato, rutaDelExpediente + nombreArchivoConFormato, true);
                        }

                        resultado = "1";
                    }
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

    }
}
