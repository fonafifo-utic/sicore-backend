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
    public class ConstantesProcedimientosAlmacenadosRE
    {
		public const string PA_INGRESA_EVENTO = "PA_TRAZABILIDAD_INGRESA";
	}

    public class ModeloRegistroEventos
    {
        public string idTraza { get; set; }
        public string idUsuario { get; set; }
        public string modulo { get; set; }
        public string operacion { get; set; }
        public string fechaTraza { get; set; }
    }

    public class RegistroEventos
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

		public async Task<string> IngresaRegistros(ModeloRegistroEventos evento)
		{
			string resultado = string.Empty;
			string objJsonDeEvento= Newtonsoft.Json.JsonConvert.SerializeObject(evento);

			try
			{
				using (SqlConnection conexion = new SqlConnection(strconSICORE))
				{
					var eventoParaIngresar = new { @pEvento = objJsonDeEvento };
					resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosRE.PA_INGRESA_EVENTO,
						eventoParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
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
