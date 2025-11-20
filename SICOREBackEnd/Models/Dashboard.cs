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
	public static class ConstantesProcedimientosAlmacenadosDashboard
	{
		public const string PA_DASHBOARD_TRAE_INFO_PROYECTOS = "PA_DASHBOARD_TRAE_INFO_PROYECTOS";
		public const string PA_DASHBOARD_TRAE_RESUMEN_VENTAS_PROYECTOS = "PA_DASHBOARD_TRAE_RESUMEN_VENTAS_PROYECTOS";
		public const string PA_DASHBOARD_TRAE_RESUMEN_COTIZACIONES = "PA_DASHBOARD_TRAE_RESUMEN_COTIZACIONES";
	}

	public class iProyectosDashboard
	{
		public string proyecto { get; set; }
		public decimal vendido { get; set; }
		public decimal comprometido { get; set; }
	}

	public class iResumenVentasDashboard
	{
		public string proyecto { get; set; }
		public decimal remanente { get; set; }
		public decimal vendido { get; set; }
		public decimal comprometido { get; set; }
	}

	public class iResumenCotizaciones
    {
		public int idProyecto { get; set; }
		public string proyecto { get; set; }
		public decimal cotizado { get; set; }
		public decimal remanente { get; set; }
    }

	public class iResumenVentas
	{
		public int idProyecto { get; set; }
		public string proyecto { get; set; }
		public decimal vendido { get; set; }
		public decimal remanente { get; set; }
	}

	public class iResumenInventario
	{
		public string proyecto { get; set; }
		public decimal utilizado { get; set; }
		public decimal remanente { get; set; }
		public decimal montoDolares { get; set; }
	}

	public class iResumenVentasPorMes
	{
		public int mes { get; set; }
		public decimal montoTransferencia { get; set; }
	}

	public class Dashboard
    {
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<IEnumerable<iProyectosDashboard>> ObtenerProyectos()
		{
			IEnumerable<iProyectosDashboard> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iProyectosDashboard>(ConstantesProcedimientosAlmacenadosDashboard.PA_DASHBOARD_TRAE_INFO_PROYECTOS,
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

		public async Task<IEnumerable<iResumenVentasDashboard>> ObtenerResumenVentas()
		{
			IEnumerable<iResumenVentasDashboard> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iResumenVentasDashboard>(ConstantesProcedimientosAlmacenadosDashboard.PA_DASHBOARD_TRAE_RESUMEN_VENTAS_PROYECTOS,
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

		public async Task<IEnumerable<iResumenCotizaciones>> ObtenerResumenCotizaciones()
		{
			IEnumerable<iResumenCotizaciones> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iResumenCotizaciones>(ConstantesProcedimientosAlmacenadosDashboard.PA_DASHBOARD_TRAE_RESUMEN_COTIZACIONES,
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

		public async Task<IEnumerable<iResumenVentas>> ObtenerResumenVentasPorProyecto()
		{
			IEnumerable<iResumenVentas> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iResumenVentas>("PA_DASHBOARD_TRAE_RESUMEN_VENTAS",
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

		public async Task<IEnumerable<iResumenInventario>> ObtenerResumenInventarioPorProyecto()
		{
			IEnumerable<iResumenInventario> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iResumenInventario>("PA_DASHBOARD_TRAE_RESUMEN_REMANENTE",
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

		public async Task<IEnumerable<iResumenVentasPorMes>> ObtenerVentasPorMesParaGrafico()
		{
			IEnumerable<iResumenVentasPorMes> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					resultado = await conexion.QueryAsync<iResumenVentasPorMes>("PA_DASHBOARD_TRAE_MONTO_VENTAS_MES",
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

	}
}
