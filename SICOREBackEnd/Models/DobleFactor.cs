using Dapper;
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
    public class DobleFactor
    {
        
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<string> EnviarCodigoVerificacion(string idPersona, string codigo, string opcionEnvio, string nombreSistema, string correoUsuario, string telefonoUsuario)
        {
            string resultado = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var valores = new
                    {
                        @pIdPersona = idPersona,
                        @pCodigo = codigo,
                        @pOpcionEnvio = opcionEnvio,
                        @pNombreSistema = nombreSistema,
                        @pCorreoUsuario = correoUsuario,
                        @pTelefonoUsuario = telefonoUsuario
                    };

                    resultado = await con.ExecuteScalarAsync<string>("PA_DOBLEFACTOR_ENVIAR_CODIGOSEGURIDAD", valores, commandType : CommandType.StoredProcedure);
                }
            } catch(Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }
            return resultado;
        }
    }
}
