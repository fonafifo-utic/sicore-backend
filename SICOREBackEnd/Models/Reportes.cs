using Dapper;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace SICOREBackEnd.Models
{
    public class iMeses
    {
        public int valor { get; set; }
        public string mes { get; set; }
    }

    public class iAnnos
    {
        public int anno { get; set; }
    }

    public class iParametros
    {
        public int anno { get; set; }
        public string meses { get; set; }
    }

    public class iReporteListadoCertificadoMensual
    {
        public int idCertificado { get; set; }
        public string numeroCertificado { get; set; }
        public string consecutivo { get; set; }
        public string sectorComercial { get; set; }
        public string nombreCertificado { get; set; }
        public string nombreCotizante { get; set; }
        public string fechaEmisionCertificado { get; set; }
        public string fechaEmisionDelCertificado { get; set; }
        public string cedulaJuridicaComprador { get; set; }
        public string montoDeTransferencia { get; set; }
        public decimal montoTransferencia { get; set; }
        public string numeroTransferencia { get; set; }
        public string fechaTransferencia { get; set; }
        public string fechaDeTransferencia { get; set; }
        public string annoInventarioGEI { get; set; }
        public string anotaciones { get; set; }
        public string usuario { get; set; }
        public string funcionario { get; set; }
        public string rangoDeFechas { get; set; }
        public string sectoresFiltrados { get; set; }
    }

    public class iReporteListadoCotizacionesMensual
    {
        public int idCotizacion { get; set; }
        public string sectorComercial { get; set; }
        public string nombreCliente { get; set; }
        public string proyecto { get; set; }
        public string usuario { get; set; }
        public string fechaHora { get; set; }
        public string fechaYHora { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal montoTotalDolares { get; set; }
        public int consecutivo { get; set; }
        public string indicadorEstado { get; set; }
        public string funcionario { get; set; }
        public string rangoDeFechas { get; set; }
        public string sectoresFiltrados { get; set; }
    }

    public class iReporteListadoCotizacionesExcel
    {
        public string sector_comercial { get; set; }
        public string nombre_cliente { get; set; }
        public string proyecto { get; set; }
        public string funcionario { get; set; }
        public string fecha_hora { get; set; }
        public string cantidad { get; set; }
        public string precio_unitario { get; set; }
        public string monto_dólares { get; set; }
        public string consecutivo { get; set; }
        public string estado { get; set; }

    }

    public class iReporteListadoFormalizacionesExcel
    {
        public string consecutivo { get; set; }
        public string sector_comercial { get; set; }
        public string nombre_cliente { get; set; }
        public string fecha_hora { get; set; }
        public string monto_dolares { get; set; }
        public string numero_transferencia { get; set; }
        public string numero_facturaFonafifo { get; set; }
        public string tipo_compra { get; set; }
        public string credito_debito { get; set; }
        public string usuario { get; set; }
    }

    public class iReporteListadoCertificadosExcel
    {
        public string numero_certificado { get; set; }
        public string consecutivo { get; set; }
        public string sector_comercial { get; set; }
        public string nombre_certificado { get; set; }
        public string fecha_emision_certificado { get; set; }
        public string cedula_juridica_comprador { get; set; }
        public string monto_transferencia { get; set; }
        public string monto_transferencia_colones { get; set; }
        public string numero_transferencia { get; set; }
        public string fecha_transferencia { get; set; }
        public string anno_inventario_GEI { get; set; }
        public string anotaciones { get; set; }
        public string usuario { get; set; }
    }

    public class iReporteListadoVentasExcel
    {
        public string nombre_cliente { get; set; }
        public string sector_comercial { get; set; }
        public string fecha { get; set; }
        public string cantidad { get; set; }
        public string monto_colones { get; set; }
        public string monto_dolares { get; set; }
        public string cuenta { get; set; }
        public string descuento { get; set; }
        public string usuario { get; set; }
    }

    public class iReporteListadoFormalizacionMensual
    {
        public int idFormalizacion { get; set; }
        public int consecutivo { get; set; }
        public string sectorComercial { get; set; }
        public string nombreCliente { get; set; }
        public string fechaHora { get; set; }
        public string fechaYHora { get; set; }
        public decimal montoDolares { get; set; }
        public string numeroTransferencia { get; set; }
        public string numeroFacturaFonafifo { get; set; }
        public string tipoCompra { get; set; }
        public string creditoDebito { get; set; }
        public string justificacionCompra { get; set; }
        public string cuentaPago { get; set; }
        public string usuario { get; set; }
        public string funcionario { get; set; }
        public string rangoDeFechas { get; set; }
        public string sectoresFiltrados { get; set; }
    }

    public class iReporteListadoVentas
    {

        public string nombreCliente { get; set; }
        public string sectorComercial { get; set; }
        public string fecha { get; set; }
        public string fechaYHora { get; set; }
        public int cantidad { get; set; }
        public decimal montoColones { get; set; }
        public decimal montoDolares { get; set; }
        public string cuenta { get; set; }
        public string descuento { get; set; }
        public string funcionario { get; set; }
        public string rangoDeFechas { get; set; }
        public string usuario { get; set; }
        public string sectoresFiltrados { get; set; }
    }

    public class iRangoFechaBusqueda
    {
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public int funcionario { get; set; }
        public int[] sector { get; set; }
    }
 
    public class iSectoresComerciales
    {
        public int idSectorComercial { get; set; }
        public string sectorComercial { get; set; }
    }

    public class iReporteEsfuerzoAnualColaborador
    {
        public int idFuncionario { get; set; }
        public string agente { get; set; }
        public decimal cantidad { get; set; }
        public decimal monto { get; set; }
        public string ultimaVenta { get; set; }
    }

    public class iDesgloseEsfuerzoColaborador
    {
        public string certificado { get; set; }
        public string cliente { get; set; }
        public string fecha { get; set; }
        public decimal cantidad { get; set; }
        public decimal monto { get; set; }
    }

    public class iReporteEsfuerzoAnualColaboradorExcel
    {
        public string agente { get; set; }
        public string ultimaVenta { get; set; }
        public string certificado { get; set; }
        public string cliente { get; set; }
        public string fecha { get; set; }
        public decimal cantidad { get; set; }
        public decimal monto { get; set; }
    }

    public class iReporteEsfuerzoAnualColaboradorPDF
    {
        public int idFuncionario { get; set; }
        public string agente { get; set; }
        public decimal cantidad { get; set; }
        public decimal monto { get; set; }
        public string ultimaVenta { get; set; }
        public List<iDesgloseEsfuerzoColaborador> desglose { get; set; }
    }

    public class iListadoEncuesta
    {
        public string pregunta { get; set; }
        public string respuesta { get; set; }
        public string personasQueContestaron { get; set; }
        public string totalEncuestados { get; set; }
        public string porcentaje { get; set; }
    }

    public class iListadoRespuestasPorAnno
    {
        public decimal formulariosRespondidos { get; set; }
        public decimal formulariosEnviados { get; set; }
    }

    public class Reportes
    {
        static IConfiguration confSICORE = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Constantes.APP_SETTINGS).Build());
        public static string strconSICORE = confSICORE[Constantes.CADENA_CONEXION_DESA].ToString();
        
        public async Task<IEnumerable<iReporteListadoCertificadoMensual>> TraeListadoMensualCertificados(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iReporteListadoCertificadoMensual> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iReporteListadoCertificadoMensual>("PA_REPORTES_TRAE_LISTADO_CERTIFICADO_PORMES",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoCotizacionesMensual>> TraeListadoMensualCotizaciones(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iReporteListadoCotizacionesMensual> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iReporteListadoCotizacionesMensual>("PA_REPORTES_TRAE_LISTADO_COTIZACIONES_PORMES",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoCotizacionesExcel>> ExportacionReporteCotizacionesExcel()
        {
            IEnumerable<iReporteListadoCotizacionesExcel> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteListadoCotizacionesExcel>("PA_REPORTES_TRAE_LISTADO_COTIZACIONES_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoFormalizacionesExcel>> ExportacionReporteFormalizacionesExcel()
        {
            IEnumerable<iReporteListadoFormalizacionesExcel> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteListadoFormalizacionesExcel>("PA_REPORTES_TRAE_LISTADO_FORMALIZACIONES_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoCertificadosExcel>> ExportacionReporteCertificadosExcel()
        {
            IEnumerable<iReporteListadoCertificadosExcel> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteListadoCertificadosExcel>("PA_REPORTES_TRAE_LISTADO_CERTIFICADOS_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoVentasExcel>> ExportacionReporteVentasExcel()
        {
            IEnumerable<iReporteListadoVentasExcel> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteListadoVentasExcel>("PA_REPORTES_TRAE_LISTADO_VENTAS_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoFormalizacionMensual>> TraeListadoMensualFormalizaciones(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iReporteListadoFormalizacionMensual> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iReporteListadoFormalizacionMensual>("PA_REPORTES_TRAE_LISTADO_FORMALIZACION_PORMES",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteListadoVentas>> TraeListadoVentas(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iReporteListadoVentas> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iReporteListadoVentas>("PA_REPORTES_TRAE_LISTADO_VENTAS",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iSectoresComerciales>> TraeTodosLosSectoresActivos()
        {
            IEnumerable<iSectoresComerciales> sectores = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    sectores = await conexion.QueryAsync<iSectoresComerciales>("PA_CLIENTE_SECTOR_TRAE_LISTADO",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                sectores = null;
            }

            return sectores;
        }

        public async Task<IEnumerable<iSectoresComerciales>> traeSectoresPorRangoFecha(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iSectoresComerciales> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iSectoresComerciales>("PA_REPORTES_TRAE_SECTORES_COMERCIALES_COTIZACIONES",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteEsfuerzoAnualColaborador>> TraeListadoAnualEsfuerzoColaborador()
        {
            IEnumerable<iReporteEsfuerzoAnualColaborador> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteEsfuerzoAnualColaborador>("PA_REPORTES_TRAE_LISTADO_ESFUERZO_ANUAL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iDesgloseEsfuerzoColaborador>> TraeDesgloseEsfuerzoColaborador(int idFuncionario)
        {
            IEnumerable<iDesgloseEsfuerzoColaborador> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iDesgloseEsfuerzoColaborador>("PA_REPORTES_TRAE_DETALLE_CERTIFICADOS_PORFUNCIONARIO",
                        new { @pIdFuncionario = idFuncionario }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteEsfuerzoAnualColaboradorExcel>> ExportacionReporteDeEsfuerzoAnualExcel()
        {
            IEnumerable<iReporteEsfuerzoAnualColaboradorExcel> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteEsfuerzoAnualColaboradorExcel>("PA_REPORTES_TRAE_LISTADO_DETALLADO_ESFUERZO_EXCEL",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iReporteEsfuerzoAnualColaboradorPDF>> ExportacionReporteEsfuerzoPDF(int idFuncionario)
        {
            IEnumerable<iReporteEsfuerzoAnualColaboradorPDF> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iReporteEsfuerzoAnualColaboradorPDF>("PA_REPORTES_TRAE_ESFUERZO_ANUAL_PORFUNCIONARIO",
                        new { @pIdFuncionario = idFuncionario }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iListadoEncuesta>> traeReporteEncuentas(iRangoFechaBusqueda rangoDeFechas)
        {
            IEnumerable<iListadoEncuesta> reporte = null;
            try
            {
                var objetoJsonParametro = Newtonsoft.Json.JsonConvert.SerializeObject(rangoDeFechas);

                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    var pParametro = new { @pParametros = objetoJsonParametro };
                    reporte = await conexion.QueryAsync<iListadoEncuesta>("PA_REPORTES_TRAE_LISTADO_ENCUENTAS",
                        pParametro, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }

        public async Task<IEnumerable<iListadoRespuestasPorAnno>> traeRespuestasPorAnno()
        {
            IEnumerable<iListadoRespuestasPorAnno> reporte = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(strconSICORE))
                {
                    reporte = await conexion.QueryAsync<iListadoRespuestasPorAnno>("PA_ENCUESTA_TRAE_RESPUESTAS_PORANNO",
                        null, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch
            {
                reporte = null;
            }

            return reporte;
        }
    }
}
