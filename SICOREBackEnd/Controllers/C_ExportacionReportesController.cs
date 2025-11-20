using Microsoft.AspNetCore.Mvc;
using SICOREBackEnd.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using SICOREBackEnd.Utils;
using System.IO;
using System;
using System.Linq;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_ExportacionReportesController : ControllerBase
    {
        Reportes reporte = new Reportes();

        ExportacionReporteDeCertificados certificados = new ExportacionReporteDeCertificados();
        ExportacionReporteDeCotizaciones cotizaciones = new ExportacionReporteDeCotizaciones();
        ExportacionReporteDeFormalizaciones formalizaciones = new ExportacionReporteDeFormalizaciones();
        ExportacionReporteDeVentas ventas = new ExportacionReporteDeVentas();
        ExportacionReporteDeEsfuerzoAnual esfuerzo = new ExportacionReporteDeEsfuerzoAnual();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private string _carpetaParaSubirArchivos;

        public C_ExportacionReportesController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
            _carpetaParaSubirArchivos = _config.GetSection(Constantes.APP_SETTINGS_DOCS_UPLOAD).Value;
        }

        [HttpPost("ExportacionReporteCertificados")]
        public async Task<IActionResult> ExportacionReporteCertificados([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoCertificadoMensual> datosDelReporte = await reporte.TraeListadoMensualCertificados(rangoFechas);

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte(Constantes.CERTIFICADOS);

            return Ok(certificados.ExportaCertificadosEnPDF(datosDelReporte, imgEncabezado, rutaDeDescarga, nombreDelArchivo));
        }

        [HttpPost("ExportacionReporteCotizaciones")]
        public async Task<IActionResult> ExportacionReporteCotizaciones([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoCotizacionesMensual> datosDelReporte = await reporte.TraeListadoMensualCotizaciones(rangoFechas);

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte(Constantes.COTIZACIONES);

            return Ok(cotizaciones.ExportaCotizacionesEnPDF(datosDelReporte, imgEncabezado, rutaDeDescarga, nombreDelArchivo));
        }

        [HttpGet("ExportacionReporteCotizacionesExcel")]
        public async Task<IActionResult> ExportacionReporteCotizacionesExcel()
        {
            IEnumerable<iReporteListadoCotizacionesExcel> datosDelReporte = await reporte.ExportacionReporteCotizacionesExcel();

            return Ok(datosDelReporte);
        }

        [HttpGet("ExportacionReporteFormalizacionesExcel")]
        public async Task<IActionResult> ExportacionReporteFormalizacionesExcel()
        {
            IEnumerable<iReporteListadoFormalizacionesExcel> datosDelReporte = await reporte.ExportacionReporteFormalizacionesExcel();

            return Ok(datosDelReporte);
        }

        [HttpGet("ExportacionReporteCertificadosExcel")]
        public async Task<IActionResult> ExportacionReporteCertificadosExcel()
        {
            IEnumerable<iReporteListadoCertificadosExcel> datosDelReporte = await reporte.ExportacionReporteCertificadosExcel();

            return Ok(datosDelReporte);
        }

        [HttpGet("ExportacionReporteVentasExcel")]
        public async Task<IActionResult> ExportacionReporteVentasExcel()
        {
            IEnumerable<iReporteListadoVentasExcel> datosDelReporte = await reporte.ExportacionReporteVentasExcel();

            return Ok(datosDelReporte);
        }

        [HttpPost("ExportacionReporteFormalizaciones")]
        public async Task<IActionResult> ExportacionReporteFormalizaciones([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoFormalizacionMensual> datosDelReporte = await reporte.TraeListadoMensualFormalizaciones(rangoFechas);

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte(Constantes.FORMALIZACIONES);

            return Ok(formalizaciones.ExportaFormalizacionesEnPDF(datosDelReporte, imgEncabezado, rutaDeDescarga, nombreDelArchivo));
        }

        [HttpPost("ExportacionReporteVentas")]
        public async Task<IActionResult> ExportacionReporteVentas([FromBody] iRangoFechaBusqueda rangoFechas)
        {
            IEnumerable<iReporteListadoVentas> datosDelReporte = await reporte.TraeListadoVentas(rangoFechas);

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte(Constantes.VENTAS);

            return Ok(ventas.ExportaVentasEnPDF(datosDelReporte, imgEncabezado, rutaDeDescarga, nombreDelArchivo));
        }

        [HttpGet("ExportacionReporteEsfuerzoPDF/{idAgente}/{funcionario}")]
        public async Task<IActionResult> ExportacionReporteEsfuerzoPDF(int idAgente, string funcionario)
        {
            IEnumerable<iReporteEsfuerzoAnualColaboradorPDF> datosDelReporte = await reporte.ExportacionReporteEsfuerzoPDF(idAgente);
            IEnumerable<iDesgloseEsfuerzoColaborador> desglose = await reporte.TraeDesgloseEsfuerzoColaborador(idAgente);

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte(Constantes.ESFUERZO);

            return Ok(esfuerzo.ExportaReporteDeEsfuerzoEnPDF(datosDelReporte, desglose, imgEncabezado, rutaDeDescarga, nombreDelArchivo, funcionario));
        }

        [HttpGet("ExportacionReporteDeEsfuerzoAnualExcel")]
        public async Task<IActionResult> ExportacionReporteDeEsfuerzoAnualExcel()
        {
            IEnumerable<iReporteEsfuerzoAnualColaboradorExcel> datosDelReporte = await reporte.ExportacionReporteDeEsfuerzoAnualExcel();

            return Ok(datosDelReporte);
        }

        private string _poneNombreAlReporte(string reporte)
        {
            DateTime hoy = DateTime.Now;
            string nombreReporte = string.Empty;

            switch (reporte)
            {
                case "certificados":
                    nombreReporte = "Certificados_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    break;

                case "cotizaciones":
                    nombreReporte = "Cotizaciones_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    break;

                case "formalizaciones":
                    nombreReporte = "Formalizaciones_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    break;

                case "ventas":
                    nombreReporte = "Ventas_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    break;

                case "esfuerzo":
                    nombreReporte = "Esfuerzo_Anual_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";
                    break;
            }

            return nombreReporte;
        }

        private string _rutaDeLaImagenEncabezado ()
        {
            return _environment.WebRootPath + "\\" + _carpetaParaSubirArchivos + "\\" + Constantes.IMAGEN_GOBIERNO_CR;
        }

        private string _rutaDondeSeGuardaElReporte ()
        {
            return _environment.WebRootPath + "\\" + _carpetaParaSubirArchivos + "\\";
        }
    }
}