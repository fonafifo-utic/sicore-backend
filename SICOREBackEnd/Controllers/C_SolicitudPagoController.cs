using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SICOREBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class C_SolicitudPagoController : ControllerBase
    {
        DatosSolicitudPagoRepositorio solicitudPago = new DatosSolicitudPagoRepositorio();
        Informe informe = new Informe();

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment;
        private string _carpetaParaSubirArchivos;

        public C_SolicitudPagoController(IConfiguration configuracion, IWebHostEnvironment ambiente)
        {
            _config = configuracion;
            _environment = ambiente;
            _carpetaParaSubirArchivos = _config.GetSection("AppSettings:docsUpload").Value;
        }

        [HttpPost("TraeSolicitudPago")]
        public async Task<IActionResult> TraeSolicitudPago([FromBody] iParametro parametros)
        {
            iDatosSolicitudPago datosDelReporte = (await solicitudPago.TraeSolicitudPago(parametros)).First();

            string imgEncabezado = _rutaDeLaImagenEncabezado();
            string rutaDeDescarga = _rutaDondeSeGuardaElReporte();
            string nombreDelArchivo = _poneNombreAlReporte();

            return Ok(informe.PruebaInforme(datosDelReporte, imgEncabezado, rutaDeDescarga, nombreDelArchivo));
        }

        private string _rutaDeLaImagenEncabezado()
        {
            return _environment.WebRootPath + "\\" + _carpetaParaSubirArchivos + "\\" + "gobierno_de_costa_rica.png";
        }

        private string _rutaDondeSeGuardaElReporte()
        {
            return _environment.WebRootPath + "\\" + _carpetaParaSubirArchivos + "\\";
        }

        private string _poneNombreAlReporte()
        {
            DateTime hoy = DateTime.Now;
            string nombreReporte = "SolicitudPago_" + hoy.ToString("yyyyMMdd_HH:mm:ss.fff").Replace(" ", "_").Replace(":", "") + ".pdf";

            return nombreReporte;
        }

    }
}
