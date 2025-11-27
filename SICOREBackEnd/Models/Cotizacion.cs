using Dapper;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
	public static class ConstantesProcedimientosAlmacenadosCotizacion
    {
		public const string PA_COTIZACION_TRAE_LISTADO = "PA_COTIZACION_TRAE_LISTADO";
		public const string PA_COTIZACION_INGRESA = "PA_COTIZACION_INGRESA";
		public const string PA_COTIZACION_ACTUALIZA = "PA_COTIZACION_ACTUALIZA";
		public const string PA_COTIZACION_TRAE_PORID = "PA_COTIZACION_TRAE_PORID";
		public const string PA_COTIZACION_INACTIVA = "PA_COTIZACION_INACTIVA";
	}
    public class iCotizacion
	{
		public int idCotizacion { get; set; }
		public int idCliente { get; set; }
		public string nombreCliente { get; set; }
		public string cedulaCliente { get; set; }
		public string contactoCliente { get; set; }
		public string telefonoCliente { get; set; }
		public string emailCliente { get; set; }
		public string direccionFisica { get; set; }
		public string sectorComercial { get; set; }
		public int idUsuario { get; set; }
		public string nombreCorto { get; set; }
		public int idProyecto { get; set; }
		public string proyecto { get; set; }
		public string fechaHora { get; set; }
		public string fechaExpiracion { get; set; }
		public decimal cantidad { get; set; }
		public decimal precioUnitario { get; set; }
		public decimal subTotal { get; set; }
		public decimal montoTotalColones { get; set; }
		public decimal montoTotalDolares { get; set; }
		public int consecutivo { get; set; }
		public string anotaciones { get; set; }
		public string indicadorEstado { get; set; }
		public string cuentaConvenio { get; set; }
		public int cotizacionEnIngles { get; set; }
		public decimal tipoCambio { get; set; }
		public int cantidadDiasEnviado { get; set; }
		public string tipoCompra { get; set; }
		public string justificacionCompra { get; set; }
		public string ListaCotizacionPorId { get; set; }
		public string observacionDeAprobacion { get; set; }
	}

	public class iCotizacionParaSalvar
	{
		public int idCotizacion { get; set; }
		public int idCliente { get; set; }
		public int idFuncionario { get; set; }
		public int idProyecto { get; set; }
		public decimal cantidad { get; set; }
		public decimal precioUnitario { get; set; }
		public decimal subTotal { get; set; }
		public decimal montoTotalColones { get; set; }
		public decimal montoTotalDolares { get; set; }
		public int consecutivo { get; set; }
		public string anotaciones { get; set; }
		public string fechaExpiracion { get; set; }
		public string cuentaConvenio { get; set; }
		public int cotizacionEnIngles { get; set; }
		public string tipoCompra { get; set; }
		public string justificacionCompra { get; set; }
		
	}

	public class iAnulaCotizacion
    {
		public int idCotizacion { get; set; }
		public int idUsuario { get; set; }
		public string descripcion { get; set; }
	}

	public class iValidaCotizacion
	{
		public int idCotizacion { get; set; }
		public int idUsuario { get; set; }
		public string observacion { get; set; }
	}

	public class Cotizacion
    {
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<IEnumerable<iCotizacion>> ObtenerListadoCotizacion()
		{
			IEnumerable<iCotizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iCotizacion>(ConstantesProcedimientosAlmacenadosCotizacion.PA_COTIZACION_TRAE_LISTADO,
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iCotizacion>> ObtenerCotizacionPorId(int pIdCotizacion)
		{
			IEnumerable<iCotizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idCotizacion = new { @pIdCotizacion = pIdCotizacion };
					resultado = await conexion.QueryAsync<iCotizacion>(ConstantesProcedimientosAlmacenadosCotizacion.PA_COTIZACION_TRAE_PORID,
						idCotizacion, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> IngresaCotizacion(iCotizacionParaSalvar pCotizacion)
		{
			string resultado = string.Empty;
			string objJsonDeCotizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pCotizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var cotizacionParaIngresar = new { @pCotizacion = objJsonDeCotizacion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCotizacion.PA_COTIZACION_INGRESA,
						cotizacionParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ActualizaCotizacion(iCotizacionParaSalvar pCotizacion)
		{
			string resultado = string.Empty;
			string objJsonDeCotizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pCotizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var cotizacionParaActualizar = new { @pCotizacion = objJsonDeCotizacion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCotizacion.PA_COTIZACION_ACTUALIZA,
						cotizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> AnulaCotizacion(iAnulaCotizacion pCotizacion)
		{
			string resultado = string.Empty;

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var cotizacionParaIngresar = new { @pIdCotizacion = pCotizacion.idCotizacion, @pIdUsuario = pCotizacion.idUsuario, @pDescripcion = pCotizacion.descripcion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCotizacion.PA_COTIZACION_INACTIVA,
						cotizacionParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ValidaUnaCotizacion(iValidaCotizacion pCotizacion)
		{
			string resultado = string.Empty;

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.ExecuteScalarAsync<string>("PA_ACTIVA_COTIZACION",
						new { @idCotizacion = pCotizacion.idCotizacion, @idUsuario = pCotizacion.idUsuario }, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> RechazaUnaCotizacion(iValidaCotizacion pCotizacion)
		{
			string resultado = string.Empty;
			string objJsonDeCotizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pCotizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var cotizacionParaActualizar = new { @pCotizacion = objJsonDeCotizacion };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_RECHAZA_COTIZACION",
						cotizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
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
