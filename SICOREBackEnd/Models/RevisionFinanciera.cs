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
	public static class ConstantesPARevisionFinanciera
	{
		public const string PA_FORMALIZACION_TRAE_LISTADO_PORID = "PA_FORMALIZACION_TRAE_LISTADO_PORID";
		public const string PA_FORMALIZACION_TRAE_LISTADO = "PA_FORMALIZACION_TRAE_LISTADO";
		public const string PA_FORMALIZACION_INGRESA = "PA_FORMALIZACION_INGRESA";
		public const string PA_FORMALIZACION_ACTUALIZA = "PA_FORMALIZACION_ACTUALIZA";
		public const string PA_FORMALIZACION_TRAE_FORMALIZACION_PORID = "PA_FORMALIZACION_TRAE_FORMALIZACION_PORID";
		public const string PA_FORMALIZACION_TRAE_RUTA_PDF_PORID = "PA_FORMALIZACION_TRAE_RUTA_PDF_PORID";
		public const string PA_ENVIAR_FORMALIZACION = "PA_ENVIAR_FORMALIZACION";
	}

	public class iFormalizacion
    {
		public string idFormalizacion { get; set; }
		public string idCotizacion { get; set; }
		public string idCliente { get; set; }
		public string cedulaCliente { get; set; }
		public string nombreCliente { get; set; }
		public string nombreComercial { get; set; }
		public string fechaHora { get; set; }
		public decimal montoDolares { get; set; }
		public decimal montoColones { get; set; }
		public int consecutivo { get; set; }
		public string numeroFacturaFonafifo { get; set; }
		public string numeroTransferencia { get; set; }
		public string  numeroComprobante { get; set; }
		public string indicadorEstado { get; set; }
		public string creditoDebito { get; set; }
		public string idUsuario { get; set; }
		public string usuario { get; set; }
		public string tieneFacturas { get; set; }
		public decimal tipoCambio { get; set; }
		public string ucii { get; set; }

	}

	public class iFormalizacionParaSalvar
	{
		public int idCotizacion { get; set; }
		public int idFuncionario { get; set; }
		public string fechaHora { get; set; }
		public decimal montoDolares { get; set; }
		public decimal montoColones { get; set; }
		public int consecutivo { get; set; }
		public string numeroFacturaFonafifo { get; set; }
		public string numeroTransferencia { get; set; }
		public string numeroComprobante { get; set; }
		public string justificacionCompra { get; set; }
		public string indicadorEstado { get; set; }
		public string creditoDebito { get; set; }
		public string numeroCIIU { get; set; }
	}

	public class iActualizaFormalizacion
	{
		public string idFormalizacion { get; set; }
		public int idUsuario { get; set; }
		public string indicadorEstado { get; set; }
		public string tieneFacturas { get; set; }
		public string numeroComprobante { get; set; }
		public string numeroFactura { get; set; }
		public int consecutivo { get; set; }
		public string numeroTransferencia { get; set; }
		public string justificacionActivacion { get; set; }
	}

	public class iVerUnaFormalizacion
    {
		public int idFormalizacion { get; set; }
		public int idCotizacion { get; set; }
		public int idProyecto { get; set; }
		public int idCliente { get; set; }
		public int idFuncionario { get; set; }
		public string proyecto { get; set; }
		public int consecutivo { get; set; }
		public string creditoDebito { get; set; }
		public string fechaHora { get; set; }
		public string fechaHoraFormalizacion { get; set; }
		public string numeroFacturaFonafifo { get; set; }
		public string numeroTransferencia { get; set; }
		public string indicadorEstado { get; set; }
		public decimal cantidad { get; set; }
		public decimal montoTotalDolares { get; set; }
		public decimal precioUnitario { get; set; }
		public decimal subTotal { get; set; }
		public string anotaciones { get; set; }
		public string cedulaCliente { get; set; }
		public string contactoCliente { get; set; }
		public string direccionFisica { get; set; }
		public string emailCliente { get; set; }
		public string nombreCliente { get; set; }
		public string nombreComercial { get; set; }
		public string telefonoCliente { get; set; }
		public string contactoContador { get; set; }
		public string emailContador { get; set; }
		public string justificacionActivacion { get; set; }
		public string numeroCIIU { get; set; }
	}

	public class iRutaFacturaFormalizacion
    {
		public string ruta { get; set; }
    }

	public class iEnviaFormalizacion
    {
		public string numeroFormalizacion { get; set; }
		public string asunto { get; set; }
		public string destinatario { get; set; }
		public int idFuncionario { get; set; }
	}

	public class iFacturasYComprobantes
	{
		public string numeroFacturaFonafifo { get; set; }
		public string numeroTransferencia { get; set; }
		public string numeroComprobante { get; set; }
		
	}

	public class iActivaFormalizacion
	{
		public string idFormalizacion { get; set; }
	}

	public class iPeticionActivarFormalizacion
    {
		public int idFormalizacion { get; set; }
		public string justificacion { get; set; }
		public int idFuncionario { get; set; }
	}

	public class RevisionFinanciera
	{
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<string> registrarUnaFormalizacionVenta(iFormalizacionParaSalvar pRevisionFinanciera)
		{
			string resultado = string.Empty;
			string resultadoEnvio = string.Empty;
			string objJsonDeFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pRevisionFinanciera);
			string annoActual = DateTime.Now.Year.ToString();

			var enviarFormalizacion = new iEnviaFormalizacion()
			{
				asunto = "Notificación SICORE",
				destinatario = string.Empty,
				idFuncionario = pRevisionFinanciera.idFuncionario,
				numeroFormalizacion = "DDC-CO-" + poneCerosFormalizacion(pRevisionFinanciera.consecutivo) + "-" + annoActual
			};

			string objJsonDeEnviarFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaIngresar = new { @pFormalizacion = objJsonDeFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_INGRESA,
						formalizacionParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}

				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
					resultadoEnvio = await conexion.ExecuteScalarAsync<string>(ConstantesPARevisionFinanciera.PA_ENVIAR_FORMALIZACION,
						formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iFormalizacion>> ObtenerListadoFormalizacion()
		{
			IEnumerable<iFormalizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iFormalizacion>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_TRAE_LISTADO,
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

		public async Task<IEnumerable<iFormalizacion>> ObtenerFormalizacionPorId(int pIdFormalizacion)
		{
			IEnumerable<iFormalizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idFormalizacion = new { @pIdFormalizacion = pIdFormalizacion };
					resultado = await conexion.QueryAsync<iFormalizacion>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_TRAE_LISTADO_PORID,
						idFormalizacion, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iVerUnaFormalizacion>> ObtenerFormalizacionParaVistaPorId(string pIdFormalizacion)
		{
			IEnumerable<iVerUnaFormalizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idFormalizacion = new { @pIdFormalizacion = pIdFormalizacion };
					resultado = await conexion.QueryAsync<iVerUnaFormalizacion>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_TRAE_FORMALIZACION_PORID,
						idFormalizacion, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> ActualizaUnaFormalizacion(iActualizaFormalizacion pFormalizacion)
		{
			string resultadoEnvio = string.Empty;
			DateTime hoy = new DateTime();
			int anno = hoy.Year;
			string resultado = string.Empty;
			string objJsonFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pFormalizacion);

			//var enviarFormalizacion = new iEnviaFormalizacion()
			//{
			//	asunto = "Notificación SICORE",
			//	destinatario = "",
			//	idFuncionario = pFormalizacion.idUsuario,
			//	numeroFormalizacion = "DDC-CE-" + poneCerosFormalizacion(pFormalizacion.consecutivo) + "-" + anno.ToString()
			//};

			//string objJsonDeEnviarFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaActualizar = new { @pFormalizacion = objJsonFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_ACTUALIZA,
						formalizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}

				//using (SqlConnection conexion = new SqlConnection(strconSICORE))
				//{
				//	var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
				//	resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_ENVIAR_CERTIFICADO_PARA_FIRMAR",
				//		formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
				//}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ActualizaUnaFormalizacionSinArchivos(iActualizaFormalizacion pFormalizacion)
		{
			string resultadoEnvio = string.Empty;
			DateTime hoy = new DateTime();
			int anno = hoy.Year;
			string resultado = string.Empty;
			string objJsonFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaActualizar = new { @pFormalizacion = objJsonFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_ACTUALIZA_FORMALIZACION",
						formalizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}

			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ActualizaUnaFormalizacionCredito(iActualizaFormalizacion pFormalizacion)
		{
			string resultadoEnvio = string.Empty;
			string annoActual = DateTime.Now.Year.ToString();
			string resultado = string.Empty;
			string objJsonFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pFormalizacion);

			var enviarFormalizacion = new iEnviaFormalizacion()
			{
				asunto = "Notificación SICORE",
				destinatario = pFormalizacion.consecutivo.ToString(),
				idFuncionario = pFormalizacion.idUsuario,
				numeroFormalizacion = "DDC-CO-" + poneCerosFormalizacion(pFormalizacion.consecutivo) + "-" + annoActual.ToString()
			};

			string objJsonDeEnviarFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaActualizar = new { @pFormalizacion = objJsonFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_ACTUALIZA_CREDITO",
						formalizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
                    resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_ENVIAR_NOTIFICACION_PARA_DDC",
                        formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iRutaFacturaFormalizacion>> ObtenerRutaFacturaPorId(string pIdFormalizacion)
		{
			IEnumerable<iRutaFacturaFormalizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idFormalizacion = new { @pIdFormalizacion = pIdFormalizacion };
					resultado = await conexion.QueryAsync<iRutaFacturaFormalizacion>(ConstantesPARevisionFinanciera.PA_FORMALIZACION_TRAE_RUTA_PDF_PORID,
						idFormalizacion, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iFacturasYComprobantes>> ObtenerComprobantes()
		{
			IEnumerable<iFacturasYComprobantes> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iFacturasYComprobantes>("PA_FORMALIZACION_TRAE_COMPROBANTES",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iFacturasYComprobantes>> ObtenerFacturas()
		{
			IEnumerable<iFacturasYComprobantes> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iFacturasYComprobantes>("PA_FORMALIZACION_TRAE_FACTURAS",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iFacturasYComprobantes>> ObtenerNumeroComprobantes()
		{
			IEnumerable<iFacturasYComprobantes> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iFacturasYComprobantes>("PA_FORMALIZACION_TRAE_NUM_COMPROBANTES",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> CierraUnaFormalizacion(iActualizaFormalizacion pFormalizacion)
		{
			string resultadoEnvio = string.Empty;
			string annoActual = DateTime.Now.Year.ToString();
			string resultado = string.Empty;
			string objJsonFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pFormalizacion);
			string prefijoDeLaCotizacion = "DDC-CO-";

			if(pFormalizacion.idFormalizacion.Contains(','))
            {
				prefijoDeLaCotizacion = "DDC-AG-";
			}

			var enviarFormalizacion = new iEnviaFormalizacion()
			{
				asunto = "Notificación SICORE",
				destinatario = "",
				idFuncionario = pFormalizacion.idUsuario,
				numeroFormalizacion = prefijoDeLaCotizacion + poneCerosFormalizacion(pFormalizacion.consecutivo) + "-" + annoActual.ToString()
			};

			string objJsonDeEnviarFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaActualizar = new { @pFormalizacion = objJsonFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_CIERRA_FORMALIZACION",
						formalizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
                    resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_CERTIFICADO_FORMALIZADO_PARA_FIRMAR",
                        formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
                }

				//using (SqlConnection conexion = new SqlConnection(strconSICORE))
				//{
				//	var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
				//	resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_ENVIAR_CERTIFICADO_PARA_FIRMAR",
				//		formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
				//}

			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> peticionActivarRevisionDeFormalizacion(iPeticionActivarFormalizacion pFormalizacion)
        {
			string resultadoEnvio = string.Empty;

			string objJsonDeFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionToAprobar = new { @pFormalizacion = objJsonDeFormalizacion };
					resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_PETICION_ACTIVAR_FORMALIZACION",
						formalizacionToAprobar, commandType: System.Data.CommandType.StoredProcedure);
				}

			}
			catch (Exception e)
			{
				resultadoEnvio = e.Message;
			}

			return resultadoEnvio;
        }

		public async Task<string> activaRevisionDeFormalizacion(string idFormalizacion)
		{
			string resultadoEnvio = string.Empty;

			var activaFormalizacion = new iActivaFormalizacion()
			{
				idFormalizacion = idFormalizacion
			};

			string objJsonDeActivacion = Newtonsoft.Json.JsonConvert.SerializeObject(activaFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionToActivar = new { @pFormalizacion = objJsonDeActivacion };
					resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_ACTIVA_FORMALIZACION",
						formalizacionToActivar, commandType: System.Data.CommandType.StoredProcedure);
				}

			}
			catch (Exception e)
			{
				resultadoEnvio = e.Message;
			}

			return resultadoEnvio;
		}

		public async Task<string> rechazaRevisionDeFormalizacion(iActualizaFormalizacion formalizacionRechazada)
		{
			string resultadoEnvio = string.Empty;
			string objJsonDeActivacion = Newtonsoft.Json.JsonConvert.SerializeObject(formalizacionRechazada);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionToActivar = new { @pFormalizacion = objJsonDeActivacion };
					resultadoEnvio = await conexion.ExecuteScalarAsync<string>("PA_RECHAZA_FORMALIZACION",
						formalizacionToActivar, commandType: System.Data.CommandType.StoredProcedure);
				}

			}
			catch (Exception e)
			{
				resultadoEnvio = e.Message;
			}

			return resultadoEnvio;
		}

		public async Task<string> registraUnaFormalizacionAgrupada(iFormalizacionParaSalvar pRevisionFinanciera)
		{
			string resultado = string.Empty;
			string resultadoEnvio = string.Empty;
			string objJsonDeFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pRevisionFinanciera);
			string annoActual = DateTime.Now.Year.ToString();

			var enviarFormalizacion = new iEnviaFormalizacion()
			{
				asunto = "Notificación SICORE",
				destinatario = string.Empty,
				idFuncionario = pRevisionFinanciera.idFuncionario,
				numeroFormalizacion = "DDC-AG-" + poneCerosFormalizacion(pRevisionFinanciera.consecutivo) + "-" + annoActual
			};

			string objJsonDeEnviarFormalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(enviarFormalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaIngresar = new { @pFormalizacion = objJsonDeFormalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_FORMALIZACION_INGRESA_DESDE_AGRUPACION",
						formalizacionParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}

				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var formalizacionParaEnviar = new { @pFormalizacion = objJsonDeEnviarFormalizacion };
					resultadoEnvio = await conexion.ExecuteScalarAsync<string>(ConstantesPARevisionFinanciera.PA_ENVIAR_FORMALIZACION,
						formalizacionParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		private string poneCerosFormalizacion (int consecutivo)
        {
			string nuevoConsecutivo = string.Empty;

			switch(consecutivo.ToString().Length)
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
