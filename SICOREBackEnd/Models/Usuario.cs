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
    public static class ConstantesProcedimientosAlmacenadosUsuario
    {
        public const string PA_DOLOGIN = "PA_DOLOGIN";
        public const string PA_USUARIO_TRAE_LISTADO_A_REGISTRAR = "PA_USUARIO_TRAE_LISTADO_A_REGISTRAR";
        public const string PA_USUARIO_ACTUALIZAR_CLAVE = "PA_USUARIO_ACTUALIZAR_CLAVE";
        public const string PA_USUARIO_TRAE_LISTADO = "PA_USUARIO_TRAE_LISTADO";
        public const string PA_PERFIL_TRAE_LISTADO = "PA_PERFIL_TRAE_LISTADO";
        public const string PA_USUARIO_TRAE_PERSONA  = "PA_USUARIO_TRAE_PERSONA";
        public const string PA_USUARIO_INGRESA = "PA_USUARIO_INGRESA";
        public const string PA_USUARIO_TRAE_LISTADO_PORID = "PA_USUARIO_TRAE_LISTADO_PORID";
        public const string PA_USUARIO_ACTUALIZA = "PA_USUARIO_ACTUALIZA";
        public const string PA_USUARIO_CAMBIA_ESTADO = "PA_USUARIO_CAMBIA_ESTADO";
        public const string PA_USUARIO_CAMBIA_PERFIL = "PA_USUARIO_CAMBIA_PERFIL";
    }

    public class LoginIngreso
    {
        public string correoCedula { get; set; }
        public string clave { get; set; }
    }

    public class LoginSalida
    {
        public string nombreCompleto { get; set; }
        public int idPersona { get; set; }
        public int idUsuario { get; set; }
        public string correoUsuario { get; set; }
        public string correoNotificaciones { get; set; }
        public string requiereActualizar { get; set; }
        public string telefonoMovil { get; set; }
        public string telefonoFijoTrabajo { get; set; }
        public int idPerfil { get; set; }
        public string perfil { get; set; }
        public string token { get; set; }
        public string menu { get; set; }
    }

    public class iMenuInicio
    {
        public string titulo { get; set; }
        public string icono { get; set; }
        public string rutaEnlace { get; set; }
    }

    public class ListaUsuariosRegistrar
    {
        public string idUsuario { get; set; }
        public string usuario { get; set; }
        public string documentoID { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
    }

    public class iUsuarioElegidoParaRegistrar
    {
        public int idUsuario { get; set; }
        public int idPerfil { get; set; }
    }


    public class ParamLogin_ClienteTask //Clase que se utiliza para metodos POST como el login y registrar la cuenta de usuario de un cliente
    {
        public string correoCedula { get; set; }
        public string clave { get; set; }
        public string tipoLogin { get; set; }
        public string documentoId { get; set; }
        public string idUsuario { get; set; }
        public string idPerfil { get; set; }
        public string idPersona { get; set; }
        public string telefonoMovil { get; set; }
        public string claveActual { get; set; }
    }

    public class IUsuario
    {
        public int idUsuario { get; set; }
        public int idPerfil { get; set; }
        public string perfil { get; set; }
        public string descripcionPerfil { get; set; }
        public int idPersona { get; set; }
        public string usuario { get; set; }
        public string indicadorEstado { get; set; }
        public string fechaVenceClave { get; set; }
        public string documentoID { get; set; }
        public string nombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string indicadorGenero { get; set; }
        public int cantidadUsuarios { get; set; }
        public string telefonoFijoTrabajo { get; set; }
    }

    public class ParamAdminUsuario //Esta clase se utiliza para los metodos POST en la adm de usuarios
    {
        public string idPerfil { get; set; }
        public string usuario { get; set; }
        public string documentoId { get; set; }
        public string idUsuario { get; set; }
        public string idUsuarioLogin { get; set; }
        public string correo { get; set; }
        public string telefonoMovil { get; set; }
        public string idPersonaFun { get; set; }
    }

    public class IPerfil
    {
        public int idPerfil { get; set; }
		public string nombre { get; set; }
        public string descripcion { get; set; }
        public string doCRUDUsuarios { get; set; }
    }

    public class IUsuarioSugerido
    {
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefonoMovil { get; set; }
        public int idPerfil { get; set; }
    }

    public class Usuario
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<IEnumerable<LoginSalida>> DoLogin(LoginIngreso objLogin)
        {
            IEnumerable<LoginSalida> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pCorreoCedula = objLogin.correoCedula, @pClave = objLogin.clave };
                    resultado = await con.QueryAsync<LoginSalida>(ConstantesProcedimientosAlmacenadosUsuario.PA_DOLOGIN,
                        values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<ListaUsuariosRegistrar>> ObtenerListadoUsuariosARegistrar()
        {
            IEnumerable<ListaUsuariosRegistrar> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    resultado = await con.QueryAsync<ListaUsuariosRegistrar>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_TRAE_LISTADO_A_REGISTRAR,
                        null, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> RegistraUsuario(iUsuarioElegidoParaRegistrar pUsuario)
        {
            string resultado = string.Empty;
            string objJsonDeUsuario = Newtonsoft.Json.JsonConvert.SerializeObject(pUsuario);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var usuarioParaIngresar = new { @pUsuario = objJsonDeUsuario };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_INGRESA,
                        usuarioParaIngresar, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> ActualizarClave(ParamLogin_ClienteTask objLogin)
        {
            string resultado = "";
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pIdUsuario = objLogin.idUsuario, @pClave = objLogin.clave, @pIdPerfil = objLogin.idPerfil, @pIdPersona = objLogin.idPersona };
                        resultado = await con.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_ACTUALIZAR_CLAVE,
                        values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                resultado = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<IUsuario>> ObtenerListadoUsuarios(string idPerfil)
        {
            IEnumerable<IUsuario> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pIdPerfil = idPerfil };
                    resultado = await con.QueryAsync<IUsuario>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_TRAE_LISTADO,
                        values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<IUsuarioSugerido>> ObtenerUsuarioPorId(int idUsuario)
        {
            IEnumerable<IUsuarioSugerido> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pIdUsuario = idUsuario };
                    resultado = await con.QueryAsync<IUsuarioSugerido>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_TRAE_LISTADO_PORID,
                        values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> AdminRegistrarUsuario(ParamAdminUsuario objParam)
        {
            string resultado = string.Empty;
            string objJsonDeUsuario = Newtonsoft.Json.JsonConvert.SerializeObject(objParam);
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pUsuario = objJsonDeUsuario };
                    resultado = await con.ExecuteScalarAsync<string>("PA_USUARIO_INGRESA", values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                resultado = e.Message;
            }

            return resultado;
        }

        public async Task<string> AdminEliminarUsuario(ParamAdminUsuario objParam)
        {
            string resultado = "";
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pIdUsuario = objParam.idUsuario, @pUsuario = objParam.usuario, @pIdUsuarioLogin = objParam.idUsuarioLogin };
                    resultado = await con.ExecuteScalarAsync<string>("PA_USUARIO_ADMIN_ELIMINAR", values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                resultado = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<IPerfil>> ObtenerListadoPerfil()
        {
            IEnumerable<IPerfil> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    resultado = await con.QueryAsync<IPerfil>(ConstantesProcedimientosAlmacenadosUsuario.PA_PERFIL_TRAE_LISTADO,
                        null, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<IUsuarioSugerido>> ObtenerPersona(string idDocumento)
        {
            IEnumerable<IUsuarioSugerido> resultado = null;
            try
            {
                using (SqlConnection con = new SqlConnection(strconSICORE))
                {
                    var values = new { @pDocumentoId = idDocumento };
                    resultado = await con.QueryAsync<IUsuarioSugerido>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_TRAE_PERSONA,
                        values, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                resultado = null;
            }

            return resultado;
        }

        

        public async Task<string> ActualizaUsuario(ParamAdminUsuario pUsuario)
        {
            string resultado = string.Empty;
            string objJsonDeUsuario = Newtonsoft.Json.JsonConvert.SerializeObject(pUsuario);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var usuarioParaIngresar = new { @pUsuario = objJsonDeUsuario };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_ACTUALIZA,
                        usuarioParaIngresar, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> CambiaEstadoUsuario(iUsuarioElegidoParaRegistrar pUsuario)
        {
            string resultado = string.Empty;
            string objJsonDeUsuario = Newtonsoft.Json.JsonConvert.SerializeObject(pUsuario);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var usuarioParaActualizar = new { @pUsuario = objJsonDeUsuario };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_CAMBIA_ESTADO,
                        usuarioParaActualizar, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> CambiaPerfilUsuario(iUsuarioElegidoParaRegistrar pUsuario)
        {
            string resultado = string.Empty;
            string objJsonDeUsuario = Newtonsoft.Json.JsonConvert.SerializeObject(pUsuario);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var usuarioParaActualizar = new { @pUsuario = objJsonDeUsuario };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosUsuario.PA_USUARIO_CAMBIA_PERFIL,
                        usuarioParaActualizar, commandType: CommandType.StoredProcedure);
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