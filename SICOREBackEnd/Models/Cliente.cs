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
    public static class ConstantesProcedimientosAlmacenadosCliente
    {
        public const string PA_CLIENTE_TRAE_LISTADO = "PA_CLIENTE_TRAE_LISTADO";
        public const string PA_CLIENTE_INGRESA = "PA_CLIENTE_INGRESA";
        public const string PA_CLIENTE_ACTUALIZA = "PA_CLIENTE_ACTUALIZA";
        public const string PA_CLIENTE_TRAE_PORID = "PA_CLIENTE_TRAE_PORID";
        public const string PA_CLIENTE_SECTOR_TRAE_LISTADO = "PA_CLIENTE_SECTOR_TRAE_LISTADO";
        public const string PA_CLIENTE_TRAE_LISTADO_SECTOR_TURISMO  = "PA_CLIENTE_TRAE_LISTADO_SECTOR_TURISMO";
        public const string PA_CLIENTE_TIPO_TRAE_LISTADO = "PA_CLIENTE_TIPO_TRAE_LISTADO";
        public const string PA_CLIENTE_ACTIVIDAD_TRAE_LISTADO = "PA_CLIENTE_ACTIVIDAD_TRAE_LISTADO";
        public const string PA_CLIENTE_TIPO_TRAE_LISTADO_POR_ID = "PA_CLIENTE_TIPO_TRAE_LISTADO_POR_ID";
        public const string PA_CLIENTE_TRAE_CLIENTE_POR_IDSECTOR = "PA_CLIENTE_TRAE_CLIENTE_POR_IDSECTOR";
    }

    public class iCliente
    {
        public int idCliente { get; set; }
		public int idSector { get; set; }
        public string sectorComercial { get; set; }
		public int idTipoEmpresa { get; set; }
		public string TipoEmpresa { get; set; }
		public int idActividadComercial { get; set; }
        public string actividadCormercial { get; set; }
        public string nombreCliente { get; set; }
        public string nombreComercial { get; set; }
        public string cedulaCliente { get; set; }
        public string contactoCliente { get; set; }
        public string telefonoCliente { get; set; }
        public string emailCliente { get; set; }
        public string direccionFisica { get; set; }
        public string clasificacion { get; set; }
        public int idFuncionario { get; set; }
        public string indicadorEstado { get; set; }
        public int cotizacionesAsociadas { get; set; }
        public string contactoContador { get; set; }
        public string emailContador { get; set; }
        public string esGestor { get; set; }
        public int idAgente { get; set; }
        public string ucii { get; set; }
    }

    public class iSector
    {
        public int idSectorComercial { get; set; }
        public string sectorComercial { get; set; }
    }

    public class iTipoEmpresa
    {
        public int idTipoEmpresa { get; set; }
        public int idSector { get; set; }
        public string tipoEmpresa { get; set; }
    }

    public class iActividadComercial
    {
        public int idActividadComercial { get; set; }
        public string actividadComercial { get; set; }
    }

    public class iFuncionario
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
    }

    public class Cliente
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();

        public async Task<IEnumerable<iCliente>> ObtenerListadoClientes()
        {
            IEnumerable<iCliente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iCliente>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TRAE_LISTADO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iCliente>> ObtenerListadoClientesPorAgente(int idAgente)
        {
            IEnumerable<iCliente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iCliente>("PA_CLIENTE_TRAE_LISTADO_POR_AGENTE",
                        new { @pIdUsuario = idAgente }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iCliente>> ObtenerClientePorId(int pIdCliente)
        {
            IEnumerable<iCliente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idCliente = new { @pIdCliente = pIdCliente };
                    resultado = await conexion.QueryAsync<iCliente>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TRAE_PORID,
                        idCliente, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iCliente>> ObtenerClientePorIdSector(int pIdSector)
        {
            IEnumerable<iCliente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var idSector = new { @pIdSector = pIdSector };
                    resultado = await conexion.QueryAsync<iCliente>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TRAE_CLIENTE_POR_IDSECTOR,
                        idSector, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iCliente>> ObtenerListadoClientesSectorTurismo()
        {
            IEnumerable<iCliente> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iCliente>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TRAE_LISTADO_SECTOR_TURISMO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iSector>> ObtenerListadoSectores()
        {
            IEnumerable<iSector> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iSector>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_SECTOR_TRAE_LISTADO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iActividadComercial>> ObtenerListadoActividadComercial()
        {
            IEnumerable<iActividadComercial> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iActividadComercial>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_ACTIVIDAD_TRAE_LISTADO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iTipoEmpresa>> ObtenerListadoTipoEmpresas()
        {
            IEnumerable<iTipoEmpresa> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iTipoEmpresa>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TIPO_TRAE_LISTADO,
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iTipoEmpresa>> ObtenerListadoTipoEmpresasPorId(int idSector)
        {
            IEnumerable<iTipoEmpresa> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pIdSector = new { @pIdSector = idSector };
                    resultado = await conexion.QueryAsync<iTipoEmpresa>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_TIPO_TRAE_LISTADO_POR_ID,
                        pIdSector, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<string> IngresaCliente(iCliente pCliente)
        {
            string resultado = string.Empty;
            string objJsonDeCliente = Newtonsoft.Json.JsonConvert.SerializeObject(pCliente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var clienteParaIngresar = new { @pCliente = objJsonDeCliente };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_INGRESA,
                        clienteParaIngresar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> ActualizaCliente(iCliente pCliente)
        {
            string resultado = string.Empty;
            string objJsonDeCliente = Newtonsoft.Json.JsonConvert.SerializeObject(pCliente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var clienteParaActualizar = new { @pCliente = objJsonDeCliente };
                    resultado = await conexion.ExecuteScalarAsync<string>(ConstantesProcedimientosAlmacenadosCliente.PA_CLIENTE_ACTUALIZA,
                        clienteParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<string> ActualizaEstadoCliente(iCliente pCliente)
        {
            string resultado = string.Empty;
            string objJsonDeCliente = Newtonsoft.Json.JsonConvert.SerializeObject(pCliente);

            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var clienteParaActualizar = new { @pCliente = objJsonDeCliente };
                    resultado = await conexion.ExecuteScalarAsync<string>("PA_CLIENTE_ACTUALIZA_ESTADO",
                        clienteParaActualizar, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }

            return resultado;
        }

        public async Task<IEnumerable<iSector>> ObtenerListadoCompletoSectores()
        {
            IEnumerable<iSector> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iSector>("PA_CLIENTE_SECTOR_TRAE_LISTADO_COMPLETO",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

        public async Task<IEnumerable<iFuncionario>> ObtenerListadoFuncionarios()
        {
            IEnumerable<iFuncionario> resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    resultado = await conexion.QueryAsync<iFuncionario>("PA_CLIENTE_TRAE_FUNCIONARIOS",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                resultado = null;
            }

            return resultado;
        }

    }
}
