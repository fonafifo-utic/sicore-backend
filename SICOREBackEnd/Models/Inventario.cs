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
	public static class ConstantesProcedimientosAlmacenadosInventario
    {
		public const string PA_INVENTARIO_TRAE_LISTADO = "PA_INVENTARIO_TRAE_LISTADO";
		public const string PA_INVENTARIO_INGRESA = "PA_INVENTARIO_INGRESA";
		public const string PA_INVENTARIO_ACTUALIZA = "PA_INVENTARIO_ACTUALIZA";
		public const string PA_INVENTARIO_CAMBIA_ESTADO = "PA_INVENTARIO_CAMBIA_ESTADO";
		public const string PA_INVENTARIO_TRAE_PORID = "PA_INVENTARIO_TRAE_PORID";
		public const string PA_MOVIMIENTO_INVENTARIO_TRAE_LISTADO = "PA_MOVIMIENTO_INVENTARIO_TRAE_LISTADO";
	}
	public class ModeloInventario
	{
		public int idInventario { get; set; }
		public int idProyecto { get; set; }
		public string proyecto { get; set; }
		public string ubicacionGeografica { get; set; }
		public decimal remanente { get; set; }
		public decimal vendido { get; set; }
		public decimal comprometido { get; set; }
	}

	public class iIngresaMovimiento
	{
		public int idInventario { get; set; }
		public int idProyecto { get; set; }
		public int idUsuario{ get; set; }
		public decimal cantidad { get; set; }
		public string descripcionMovimiento { get; set; }
	}

	public class iMovimiento
	{
		public string idMovimiento { get; set; }
		public string idProyecto { get; set; }
		public string proyecto { get; set; }
		public string ubicacionGeografica { get; set; }
		public string idUsuario { get; set; }
		public string usuario { get; set; }
		public decimal saldoInicial { get; set; }
		public string fechaMovimiento { get; set; }
		public decimal cantidad { get; set; }
		public string tipoMovimiento { get; set; }
		public string descripcionMovimiento { get; set; }
		public decimal comprometido { get; set; }
		public decimal remanente { get; set; }
		public decimal remanenteReal { get; set; }
	}

	public class Inventario
    {
		static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
		public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<IEnumerable<ModeloInventario>> ObtenerListadoInventario()
        {
			IEnumerable<ModeloInventario> resultado = null;
			try
            {
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
					resultado = await conexion.QueryAsync<ModeloInventario>(ConstantesProcedimientosAlmacenadosInventario.PA_INVENTARIO_TRAE_LISTADO,
						null, commandType: System.Data.CommandType.StoredProcedure);
                }
            } catch
            {
				resultado = null;
            }

			return resultado;
        }

		public async Task<IEnumerable<ModeloInventario>> ObtenerInventarioPorId(int pIdInventario)
		{
			IEnumerable<ModeloInventario> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idInventario = new { @pIdInventario = pIdInventario };
					resultado = await conexion.QueryAsync<ModeloInventario>(ConstantesProcedimientosAlmacenadosInventario.PA_INVENTARIO_TRAE_PORID,
						idInventario, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> IngresaInventario(iIngresaMovimiento pInventario)
        {
			string resultado = string.Empty;
			string objJsonDeInventario = Newtonsoft.Json.JsonConvert.SerializeObject(pInventario);

            try
            {
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var inventarioParaIngresar = new { @pInventario = objJsonDeInventario };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosInventario.PA_INVENTARIO_INGRESA,
						inventarioParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
				}
            } catch(Exception e)
            {
				string mensaje = e.Message;
            }

			return resultado;
        }

		public async Task<string> ActualizaInventario(iIngresaMovimiento pInventario)
		{
			string resultado = string.Empty;
			string objJsonDeInventario = Newtonsoft.Json.JsonConvert.SerializeObject(pInventario);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var inventarioParaActualizar = new { @pInventario = objJsonDeInventario };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosInventario.PA_INVENTARIO_ACTUALIZA,
						inventarioParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<string> ActualizaInventarioAumento(iIngresaMovimiento pInventario)
		{
			string resultado = string.Empty;
			string objJsonDeInventario = Newtonsoft.Json.JsonConvert.SerializeObject(pInventario);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var inventarioParaActualizar = new { @pInventario = objJsonDeInventario };
					resultado = await conexion.ExecuteScalarAsync<string>("PA_INVENTARIO_ACTUALIZA_AUMENTAR",
						inventarioParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch (Exception e)
			{
				string mensaje = e.Message;
			}

			return resultado;
		}

		public async Task<IEnumerable<iMovimiento>> ObtenerListadoMovimientos(int pIdProyecto)
		{
			IEnumerable<iMovimiento> resultado = null;
			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var idProyecto = new { @pIdProyecto = pIdProyecto };
					resultado = await conexion.QueryAsync<iMovimiento>(ConstantesProcedimientosAlmacenadosInventario.PA_MOVIMIENTO_INVENTARIO_TRAE_LISTADO,
						idProyecto, commandType: System.Data.CommandType.StoredProcedure);
				}
			}
			catch
			{
				resultado = null;
			}

			return resultado;
		}

		public async Task<string> CambiaEstadoInventario(ModeloInventario pInventario)
		{
			string resultado = string.Empty;
			string objJsonDeInventario = Newtonsoft.Json.JsonConvert.SerializeObject(pInventario);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var inventarioParaActualizar = new { @pInventario = objJsonDeInventario };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosInventario.PA_INVENTARIO_CAMBIA_ESTADO,
						inventarioParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
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
