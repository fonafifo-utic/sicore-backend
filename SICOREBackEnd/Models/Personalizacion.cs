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
	public static class ConstantesProcedimientosAlmacenadosPersonalizacion
	{
		public const string PA_PERSONALIZACION_TRAE_LISTADO = "PA_PERSONALIZACION_TRAE_LISTADO";
		public const string PA_PERSONALIZACION_ACTUALIZA = "PA_PERSONALIZACION_ACTUALIZA";
	}

	public class iPersonalizacion
    {
		public int idPersonalizacion { get; set; }
		public int idFuncionario { get; set; }
		public byte[] logoPrincipal { get; set; }
		public byte[] logoSecundario { get; set; }
		public byte[] tercerLogo { get; set; }
		public byte[] logoSistema { get; set; }
		public string leyendaDescriptivaCotizacionEspannol { get; set; }
		public string leyendaDescriptivaCotizacionIngles { get; set; }
		public string leyendaFinalidadCotizacionEspannol { get; set; }
		public string leyendaFinalidadCotizacionIngles { get; set; }
		public string leyendaDescripcionCertificadoEspannol { get; set; }
		public string leyendaDescripcionCertificadoIngles { get; set; }
		public string correoGerenciaEjecutiva { get; set; }
		public string directorEjecutivo { get; set; }
		
	}

	public class iDirectorEjecutivo
    {
		public string director { get; set; }
    }

	public class Personalizacion
	{
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<IEnumerable<iPersonalizacion>> ObtenerListadoDePersonalizacion()
		{
			IEnumerable<iPersonalizacion> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iPersonalizacion>(ConstantesProcedimientosAlmacenadosPersonalizacion.PA_PERSONALIZACION_TRAE_LISTADO,
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

		public async Task<string> ActualizaPersonalizacion(iPersonalizacion pPersonalizacion)
		{
			string resultado = string.Empty;
			string objJsonDePersonalizacion = Newtonsoft.Json.JsonConvert.SerializeObject(pPersonalizacion);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var personalizacionParaActualizar = new { @pPersonalizacion = objJsonDePersonalizacion };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosPersonalizacion.PA_PERSONALIZACION_ACTUALIZA,
						personalizacionParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iDirectorEjecutivo>> ObtenerListadoDeDirectores()
		{
			IEnumerable<iDirectorEjecutivo> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iDirectorEjecutivo>("PA_PERSONALIZACION_TRAE_DIRECTORES",
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

	}
}
