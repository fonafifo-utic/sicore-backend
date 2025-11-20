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

	public static class ConstantesProcedimientosAlmacenadosProyecto
	{
		public const string PA_PROYECTO_INGRESA = "PA_PROYECTO_INGRESA";
		public const string PA_PROYECTO_TRAE_LISTADO = "PA_PROYECTO_TRAE_LISTADO";
		public const string PA_PROYECTO_TRAE_PORID = "PA_PROYECTO_TRAE_PORID";
		public const string PA_PROYECTO_ACTUALIZA  = "PA_PROYECTO_ACTUALIZA";
		public const string PA_PROYECTO_TRAE_LISTADO_CON_REMANENTE = "PA_PROYECTO_TRAE_LISTADO_CON_REMANENTE";
	}

	public class iProyecto
	{
		public int idProyecto { get; set; }
		public int idFuncionario { get; set; }
		public string proyecto { get; set; }
		public string descripcionProyecto { get; set; }
		public string ubicacionGeografica { get; set; }
		public string periodoInicio { get; set; }
		public string periodoFinalizacion { get; set; }
		public string especieArboles { get; set; }
		public string contratoPSA { get; set; }
		public string indicadorEstado { get; set; }
		public int cotizacionesAsociadas { get; set; }
	}
	public class iEstadoProyecto
	{
		public int idProyecto { get; set; }
		public int idFuncionario { get; set; }
		public string indicadorEstado { get; set; }
	}


	public class iRutaExpediente
	{
		public string ruta { get; set; }
	}

	public class Proyecto
    {
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<string> IngresaUnProyecto(iProyecto pProyecto)
		{
			string resultado = string.Empty;
			string objJsonDeProyecto = Newtonsoft.Json.JsonConvert.SerializeObject(pProyecto);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var proyectoParaIngresar = new { @pProyecto = objJsonDeProyecto };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosProyecto.PA_PROYECTO_INGRESA,
						proyectoParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iProyecto>> ObtenerListadoProyectos()
		{
			IEnumerable<iProyecto> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iProyecto>(ConstantesProcedimientosAlmacenadosProyecto.PA_PROYECTO_TRAE_LISTADO,
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iProyecto>> ObtenerListadoProyectosActivos()
		{
			IEnumerable<iProyecto> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iProyecto>("PA_PROYECTO_TRAE_LISTADO_ACTIVOS",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iProyecto>> ObtenerListadoProyectosSinInventario()
		{
			IEnumerable<iProyecto> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iProyecto>("PA_PROYECTO_TRAE_LISTADO_SIN_INVENTARIO",
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iProyecto>> ObtenerListadoProyectosConRemanente()
		{
			IEnumerable<iProyecto> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iProyecto>(ConstantesProcedimientosAlmacenadosProyecto.PA_PROYECTO_TRAE_LISTADO_CON_REMANENTE,
						null, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<IEnumerable<iProyecto>> ObtenerProyectoPorId(int pIdProyecto)
		{
			IEnumerable<iProyecto> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idInventario = new { @pIdProyecto = pIdProyecto };
					resultado = await conexion.QueryAsync<iProyecto>(ConstantesProcedimientosAlmacenadosProyecto.PA_PROYECTO_TRAE_PORID,
						idInventario, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> ActualizaProyecto(iProyecto pProyecto)
		{
			string resultado = string.Empty;
			string objJsonDeProyecto = Newtonsoft.Json.JsonConvert.SerializeObject(pProyecto);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var proyectoParaActualizar = new { @pProyecto = objJsonDeProyecto };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosProyecto.PA_PROYECTO_ACTUALIZA,
						proyectoParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iRutaExpediente>> ObtenerRutaExpedientePorId(int pIdProyecto)
		{
			IEnumerable<iRutaExpediente> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idProyecto = new { @pIdProyecto = pIdProyecto };
					resultado = await conexion.QueryAsync<iRutaExpediente>("PA_PROYECTO_TRAE_RUTA_EXPEDIENTE_PDF_PORID",
						idProyecto, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> ActualizaEstadoProyecto(iEstadoProyecto pProyecto)
		{
			string resultado = string.Empty;
			string objJsonDeProyecto = Newtonsoft.Json.JsonConvert.SerializeObject(pProyecto);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var proyectoParaActualizar = new { @pProyecto = objJsonDeProyecto };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_PROYECTO_ACTUALIZA_ESTADO",
						proyectoParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
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
