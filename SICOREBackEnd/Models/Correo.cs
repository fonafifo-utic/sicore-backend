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
    public static class ConstantesProcedimientosAlmacenadosCorreo
    {
        public const string PA_USUARIO_ENVIO_EMAIL = "PA_USUARIO_ENVIO_EMAIL";
    }
    
    public class ModeloCorreo
    {
        public string asunto { get; set; }
        public string cuerpoCorreo { get; set; }
        public string correo { get; set; }
        public int idPersonaEnvia { get; set; }
    }

    public class Correo
    {
        static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strcon = conf[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<string> Enviar(ModeloCorreo obj)
        {
            string resultado = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    var values = new { @pAsunto = obj.asunto, @pCuerpoCorreo = obj.cuerpoCorreo, @pCorreo = obj.correo, @pIdPersonaEnvia = obj.idPersonaEnvia };
                    resultado = await con.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCorreo.PA_USUARIO_ENVIO_EMAIL,
                        values, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception e)
            {
                resultado = e.Message;
            }


            return resultado;
        }
    }
}
