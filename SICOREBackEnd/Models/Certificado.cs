using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
	public static class ConstantesProcedimientosAlmacenadosCertificado
	{
		public const string PA_CERTIFICADO_TRAE_LISTADO = "PA_CERTIFICADO_TRAE_LISTADO";
		public const string PA_CERTIFICADO_INGRESA = "PA_CERTIFICADO_INGRESA";
		public const string PA_CERTIFICADO_ACTUALIZA = "PA_CERTIFICADO_ACTUALIZA";
		public const string PA_CERTIFICADO_TRAE_PORID = "PA_CERTIFICADO_TRAE_PORID";
		public const string PA_CERTIFICADO_TRAE_RUTA_PDF_PORID = "PA_CERTIFICADO_TRAE_RUTA_PDF_PORID";
		public const string PA_ENVIAR_CERTIFICADO = "PA_ENVIAR_CERTIFICADO";
		

	}
	public class iCertificado
    {
		public int idCertificado { get; set; }
		public int idFormalizacion { get; set; }
		public int idCotizacion { get; set; }
		public int idFuncionario { get; set; }
		public int idCliente { get; set; }
		public string usuario { get; set; }
		public int numeroCertificado { get; set; }
		public string nombreCertificado { get; set; }
		public string fechaEmisionCertificado { get; set; }
		public string cedulaJuridicaComprador { get; set; }
		public string montoTransferencia { get; set; }
		public string numeroTransferencia { get; set; }
		public string fechaTransferencia { get; set; }
		public string annoInventarioGEI { get; set; }
		public string consecutivo { get; set; }
		public string nombreArchivo { get; set; }
		public string anotaciones { get; set; }
		public string numeroIdentificacionUnico { get; set; }
	}

	public class iVistaCertificado
	{
		public string usuario { get; set; }
		public string emailUsuario { get; set; }
		public string numeroCertificado { get; set; }
		public string nombreCertificado { get; set; }
		public string fechaEmisionCertificado { get; set; }
		public string cedulaJuridicaComprador { get; set; }
		public decimal montoTransferencia { get; set; }
		public string numeroTransferencia { get; set; }
		public string fechaTransferencia { get; set; }
		public string annoInventarioGEI { get; set; }
		public decimal cantidad { get; set; }
		public string cuentaConvenio { get; set; }
		public string proyecto { get; set; }
		public int periodo { get; set; }
		public string nombreArchivo { get; set; }
		public string anotaciones { get; set; }
		public string directorEjecutivo { get; set; }
		public string observaciones { get; set; }
		public string numeroIdentificacionUnico { get; set; }
		public string cssCertificado { get; set; }
		public string enIngles { get; set; }
		public string indicadorEstado { get; set; }
		public string justificacionEdicion { get; set; }
		
	}

	public class iRutaCertificado
    {
		public string ruta { get; set; }
    }

	public class iOpcionesParaEnviarCertificado
	{
		public string asunto { get; set; }
		public string destinatario { get; set; }
		public string enlace { get; set; }
		public string enlaceEncuesta { get; set; }
		public string numeroCertificado { get; set; }
		public int idFuncionario { get; set; }
		public int idCotizacion { get; set; }
	}

	public class iPoneObservacionesAlCertificado
    {
		public int idFuncionario { get; set; }
		public int idCertificado { get; set; }
		public string observacion { get; set; }
		public string nombreCertificado { get; set; }
		public string cedulaJuridica { get; set; }
		public string numeroTransferencia { get; set; }
		public string justificacionEdicion { get; set; }
		public string cssCertificado { get; set; }
		public string indicadorEstado { get; set; }
		public string enIngles { get; set; }
	}

	public class iCertificadoFirmado
	{
		public string idCertificado { get; set; }
		public IFormFile certificado { get; set; }
	}

	public class Certificado
	{
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<IEnumerable<iCertificado>> ObtenerListadoCertificados()
		{
			IEnumerable<iCertificado> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iCertificado>(ConstantesProcedimientosAlmacenadosCertificado.PA_CERTIFICADO_TRAE_LISTADO,
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch(Exception ex)
			{
				resultado = null;
				string mensaje = ex.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iCertificado>> listarCertificadosAprobados()
		{
			IEnumerable<iCertificado> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iCertificado>("PA_CERTIFICADO_TRAE_LISTADO_APROBADOS",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				resultado = null;
				string mensaje = ex.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iVistaCertificado>> ObtenerCertificadoPorId(int pIdCertificado)
		{
			IEnumerable<iVistaCertificado> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idCertificado = new { @pIdCertificado = pIdCertificado };
					resultado = await conexion.QueryAsync<iVistaCertificado>(ConstantesProcedimientosAlmacenadosCertificado.PA_CERTIFICADO_TRAE_PORID,
						idCertificado, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> IngresaCertificado(iCertificado pCertificado)
		{
			string resultado = string.Empty;
			string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var certificadoParaIngresar = new { @pCertificado = objJsonDeCertificado };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCertificado.PA_CERTIFICADO_INGRESA,
						certificadoParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ActualizaCertificado(iCertificado pCertificado)
		{
			string resultado = string.Empty;
			string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var certificadoParaActualizar = new { @pCertificado = objJsonDeCertificado };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCertificado.PA_CERTIFICADO_ACTUALIZA,
						certificadoParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iRutaCertificado>> ObtenerRutaCertificadoPorId(int pIdCertificado)
		{
			IEnumerable<iRutaCertificado> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idCertificado = new { @pIdCertificado = pIdCertificado };
					resultado = await conexion.QueryAsync<iRutaCertificado>(ConstantesProcedimientosAlmacenadosCertificado.PA_CERTIFICADO_TRAE_RUTA_PDF_PORID,
						idCertificado, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> EnviaCertificado(iOpcionesParaEnviarCertificado pCertificado)
		{
			string resultado = string.Empty;
			string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var certificadoParaEnviar = new { @pOpcionesEnvio = objJsonDeCertificado };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCertificado.PA_ENVIAR_CERTIFICADO,
						certificadoParaEnviar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iRutaCertificado>> ObtieneRutaElementosExpediente()
		{
			IEnumerable<iRutaCertificado> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iRutaCertificado>("PA_CERTIFICADO_TRAE_RUTA_PDF_EXPEDIENTE",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> PoneObservacionesAlCertificado(iPoneObservacionesAlCertificado pCertificado)
		{
			string resultado = string.Empty;
			string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var certificadoParaActualizar = new { @pCertificado = objJsonDeCertificado };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_CERTIFICADO_ACTUALIZA_OBSERVACIONES",
						certificadoParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> apruebaCertificado(iPoneObservacionesAlCertificado pCertificado)
		{
			string resultado = string.Empty;
			string objJsonDeCertificado = Newtonsoft.Json.JsonConvert.SerializeObject(pCertificado);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var certificadoParaActualizar = new { @pCertificado = objJsonDeCertificado };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_CERTIFICADO_ACTUALIZA_APROBACION",
						certificadoParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
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
