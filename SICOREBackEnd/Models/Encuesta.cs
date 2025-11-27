using Dapper;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
    public static class ConstantesProcedimientosAlmacenadosEncuesta
    {
        public const string PA_ENCUESTA_PREGUNTA_INGRESA = "PA_ENCUESTA_PREGUNTA_INGRESA";
        public const string PA_ENCUESTA_PREGUNTAS_TRAE_LISTADO = "PA_ENCUESTA_PREGUNTAS_TRAE_LISTADO";
        public const string PA_ENCUESTA_RESPUESTAS_TRAE_PORID = "PA_ENCUESTA_RESPUESTAS_TRAE_PORID";
        public const string PA_ENCUESTA_INGRESA = "PA_ENCUESTA_INGRESA";
        public const string PA_ENCUESTA_PREGUNTAS_TRAE_PORID = "PA_ENCUESTA_PREGUNTAS_TRAE_PORID";
        public const string PA_ENCUESTA_PREGUNTAS_ACTUALIZA = "PA_ENCUESTA_PREGUNTAS_ACTUALIZA";
        public const string PA_ENCUESTA_TRAE = "PA_ENCUESTA_TRAE";
        public const string PA_ENCUESTA_LISTA_TRAE = "PA_ENCUESTA_LISTA_TRAE";
        public const string PA_ENCUESTA_LISTA_TRAE_PORIDCLIENTE = "PA_ENCUESTA_LISTA_TRAE_PORIDCLIENTE";
        public const string PA_ENCUESTA_HECHA_CLIENTE_INGRESA = "PA_ENCUESTA_HECHA_CLIENTE_INGRESA";
        public const string PA_ENCUESTA_TRAE_RATING = "PA_ENCUESTA_TRAE_RATING";
        public const string PA_ENCUESTA_TRAE_SELECCION = "PA_ENCUESTA_TRAE_SELECCION";
    }

    public class iRespuestasTemporal
    {
        public string respuesta { get; set; }
        public string valorPeso { get; set; }
    }

    public class iPregunta
    {
        public string pregunta { get; set; }
        public string tipo { get; set; }
        public iRespuestasTemporal[] respuestas { get; set; }
        public int idFuncionario { get; set; }
        public int idPregunta { get; set; }
    }

    public class iPreguntas
    {
        public int idPregunta { get; set; }
        public string pregunta { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
    }

    public class iRespuestas
    {
        public int idRespuesta { get; set; }
        public int idPregunta { get; set; }
        public string pregunta { get; set; }
        public string tipoPregunta { get; set; }
        public string respuesta { get; set; }
        public string valorRespuesta { get; set; }
    }

    public class iEncuesta
    {
        public int idEncuesta { get; set; }
        public int idPregunta { get; set; }
        public int idFuncionario { get; set; }
    }

    public class iVistaEncuesta
    {
        public int idPregunta { get; set; }
        public int idRespuesta { get; set; }
        public string tipoPregunta { get; set; }
        public string pregunta { get; set; }
        public string respuestaOpcion { get; set; }
        public string valorRespuesta { get; set; }
    }

    public class iListaEncuesta
    {
        public int idEncuesta { get; set; }
        public int idPregunta { get; set; }
        public string pregunta { get; set; }
        public string tipoPregunta { get; set; }
    }

    public class iRespuestasEncuesta
    {
        public int idCliente { get; set; }
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public int valor { get; set; }
        public string fechaHoraRespuesta { get; set; }
        public string tipoPregunta { get; set; }
    }

    public class iRespuestasTipoRating
    {
        public int conteo { get; set; }
		public string pregunta { get; set; }
        public string respuesta { get; set; }
    }

    public class iRespuestasTipoSeleccion
    {
        public int conteo { get; set; }
        public string pregunta { get; set; }
        public string respuesta { get; set; }
    }

    public class iRespuestasListadoMes
    {
        public int idReporte { get; set; }
		public string nombreCliente { get; set; }
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public int numeroCertificado { get; set; }
    }

    public class iRespuestaEncuestaEnviada
    {
        public int idReporte { get; set; }
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
    }

    public class iEncuestaEnviada
    {
        public int idTrazaEncuesta { get; set; }
        public int idCliente { get; set; }
        public string emailCliente { get; set; }
        public string nombreCliente { get; set; }
        public int numeroCertificado { get; set; }
        public string usuario { get; set; }
        public string fechaHoraEnvio { get; set; }
        public string estado { get; set; }
        public int idCotizacion { get; set; }
        public int conteoEnvios { get; set; }
    }

    public class iEncuestaPendiente
    {
        public int idTrazaEncuesta { get; set; }
        public string nombreCliente { get; set; }
        public int numeroCertificado { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
    }

    public class iEncuestaTraeRespuestasParaExcel
    {
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public int contestaron { get; set; }
    }

    public class iEncuestaTraeRespuestasOpinion
    {
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public string fecha { get; set; }
        public string cliente { get; set; }
        public string agente { get; set; }
    }

    public class Encuesta
    {	
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<string> IngresaUnaPregunta(iPregunta pPregunta)
        {
            string resultado = string.Empty;
            string objJsonDePregunta = Newtonsoft.Json.JsonConvert.SerializeObject(pPregunta);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var preguntaParaIngresar = new { @pPregunta = objJsonDePregunta };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_PREGUNTA_INGRESA,
                        preguntaParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<iPreguntas>> ObtenerListadoPreguntas()
        {
            IEnumerable<iPreguntas> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iPreguntas>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_PREGUNTAS_TRAE_LISTADO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch(Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iRespuestas>> ObtenerRespuestasPorId(int pIdPregunta)
        {
            IEnumerable<iRespuestas> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idPregunta = new { @pIdPregunta = pIdPregunta };
                    resultado = await conexion.QueryAsync<iRespuestas>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_RESPUESTAS_TRAE_PORID,
                        idPregunta, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> IngresaUnaEncuesta(iEncuesta [] pEncuesta)
        {
            string resultado = string.Empty;
            string objJsonDeEncuesta = Newtonsoft.Json.JsonConvert.SerializeObject(pEncuesta);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var encuestaParaIngresar = new { @pEncuesta = objJsonDeEncuesta };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_INGRESA,
                        encuestaParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> ActualizaPregunta(iPregunta pPregunta)
        {
            string resultado = string.Empty;
            string objJsonDePregunta = Newtonsoft.Json.JsonConvert.SerializeObject(pPregunta);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var preguntaParaActualizar = new { @pPregunta = objJsonDePregunta };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_PREGUNTAS_ACTUALIZA,
                        preguntaParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<iPreguntas>> ObtenerPreguntasPorId(int pIdPregunta)
        {
            IEnumerable<iPreguntas> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idPregunta = new { @pIdPregunta = pIdPregunta };
                    resultado = await conexion.QueryAsync<iPreguntas>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_PREGUNTAS_TRAE_PORID,
                        idPregunta, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iVistaEncuesta>> ObtenerEncuesta()
        {
            IEnumerable<iVistaEncuesta> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iVistaEncuesta>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_TRAE,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iListaEncuesta>> ObtenerListaEncuesta()
        {
            IEnumerable<iListaEncuesta> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iListaEncuesta>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_LISTA_TRAE,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iVistaEncuesta>> ObtieneEncuestaPorIdCliente(int pIdCliente)
        {
            IEnumerable<iVistaEncuesta> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idCliente = new { @pIdCliente = pIdCliente };
                    resultado = await conexion.QueryAsync<iVistaEncuesta>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_LISTA_TRAE_PORIDCLIENTE,
                        idCliente, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> IngresEncuestaHechaPorCliente(iRespuestasEncuesta[] pEncuesta)
        {
            string resultado = string.Empty;
            string objJsonDeEncuesta = Newtonsoft.Json.JsonConvert.SerializeObject(pEncuesta);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var encuestaParaIngresar = new { @pEncuesta = objJsonDeEncuesta };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_HECHA_CLIENTE_INGRESA,
                        encuestaParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<iRespuestasTipoRating>> ObtenerListadoRespuestasRating()
        {
            IEnumerable<iRespuestasTipoRating> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iRespuestasTipoRating>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_TRAE_RATING,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iRespuestasTipoSeleccion>> ObtenerListadoRespuestasSeleccion()
        {
            IEnumerable<iRespuestasTipoSeleccion> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iRespuestasTipoSeleccion>(ConstantesProcedimientosAlmacenadosEncuesta.PA_ENCUESTA_TRAE_SELECCION,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iRespuestasListadoMes>> ObtenerListadoRespuestasEnviadasMes()
        {
            IEnumerable<iRespuestasListadoMes> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iRespuestasListadoMes>("PA_ENCUESTA_LISTADO_RESPUESTAS_DELMES",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iEncuestaEnviada>> ObtenerListadoEnviadas()
        {
            IEnumerable<iEncuestaEnviada> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iEncuestaEnviada>("PA_ENCUESTA_TRAE_ENVIADAS",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iEncuestaPendiente>> ObtenerListadoPendientes()
        {
            IEnumerable<iEncuestaPendiente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iEncuestaPendiente>("PA_ENCUESTA_LISTADO_PENDIENTES_CONTESTAR",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iRespuestaEncuestaEnviada>> ObtieneRespuestaEncuestaPorIdCliente(int pIdCliente)
        {
            IEnumerable<iRespuestaEncuestaEnviada> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idCliente = new { @pIdCliente = pIdCliente };
                    resultado = await conexion.QueryAsync<iRespuestaEncuestaEnviada>("PA_ENCUESTA_LISTADO_RESPUESTAS_PORIDCLIENTE",
                        idCliente, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> ReEnviaEncuesta(iOpcionesParaEnviarCertificado pCertificado)
        {
            string resultado = string.Empty;
            string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var certificadoParaEnviar = new { @pOpcionesEnvio = objJsonDeCertificado };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_REENVIAR_ENCUESTA",
                        certificadoParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<iEncuestaTraeRespuestasParaExcel>> ObtenerRespuestasDeLaEncuestaExportarExcel()
        {
            IEnumerable<iEncuestaTraeRespuestasParaExcel> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iEncuestaTraeRespuestasParaExcel>("PA_ENCUESTA_TRAE_DASHBOARD_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iEncuestaTraeRespuestasOpinion>> ObtenerRespuestasOpinion(string fechaInicio, string fechaFin)
        {
            IEnumerable<iEncuestaTraeRespuestasOpinion> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idCliente = new { @fechaInicio = fechaInicio, @fechaFin = fechaFin };
                    resultado = await conexion.QueryAsync<iEncuestaTraeRespuestasOpinion>("PA_ENCUESTA_TRAE_RESPUESTAS_OPINION",
                        idCliente, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

    }
}
