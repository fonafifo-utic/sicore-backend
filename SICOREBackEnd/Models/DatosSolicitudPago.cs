using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
    public class iDatosSolicitudPago
    {
        public int idSolicitudPago { get; set; }
        public int idContrato { get; set; }
        public string numeroContrato { get; set; }
        public int idEstado { get; set; }
        public string NombreCompleto { get; set; }
        public string haSolicitud { get; set; }
        public string arbolesSolicitud { get; set; }
        public decimal montoContrato { get; set; }
        public string documentoID { get; set; }
        public string numeroSolicitudPago { get; set; }
        public int numeroCuota { get; set; }
        public decimal montoSolicitud { get; set; }
        public decimal totalDeduccion { get; set; }
        public decimal deduccionAfectacion { get; set; }
        public decimal deduccionImpuesto { get; set; }
        public decimal deduccionIncumpliento { get; set; }
        public decimal deduccionOtraInstitucion { get; set; }
        public decimal deduccionOtras { get; set; }
        public decimal deduccionAbonoCredito { get; set; }
        public decimal montoNetoSolicitud { get; set; }
        public string observacion { get; set; }
        public string fechaSolicitudPago { get; set; }
        public string fechaPagoProgramado { get; set; }
        public string nombreOrganizacion { get; set; }
        public int tipoMoneda { get; set; }
        public string fechaInsertoAuditoria { get; set; }
        public string cuentaBancariaSINPE { get; set; }
        public string cuentaClienteSINPE { get; set; }
        public int idListaPSAnti { get; set; }
        public decimal montoBase { get; set; }
        public decimal montoHidrico { get; set; }
        public decimal montoBiodiversidad { get; set; }
        public int idSolicitudIngreso { get; set; }
        public int totalAsociados { get; set; }
    }

    public class iParametro
    {
        public string idSP { get; set; }
        public string pEsPSEMC { get; set; }
    }

    public class DatosSolicitudPagoRepositorio
    {
        static IConfiguration configuracionBD = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json").Build());
        public static string cadenaConexion = configuracionBD["ConnectionStrings:strconnectionDESA"].ToString();

        public async Task<IEnumerable<iDatosSolicitudPago>> TraeSolicitudPago(iParametro parametros)
        {
            IEnumerable<iDatosSolicitudPago> informe = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    var pParametro = new { @idSP = parametros.idSP, @pEsPSEMC = parametros.pEsPSEMC };
                    informe = await conexion.QueryAsync<iDatosSolicitudPago>("PA_SP_GET_DATOS_SOLICITUD_PAGO",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                informe = null;
            }

            return informe;
        }
    }
}
