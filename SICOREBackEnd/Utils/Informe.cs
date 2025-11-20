using System;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using SICOREBackEnd.Models;

namespace SICOREBackEnd.Utils
{
    public class iRutaDeDescarga
    {
        public string nombreArchivo { get; set; }
        public string resultado { get; set; }
        public string mensaje { get; set; }
    }

    public class Informe
    {
        private int _margenIzquierda = 50;
        private int _margenTop = 50;
        private XImage _xImageMembrete;
        private string _tituloReporte = "SOLICITUD DE PAGO POR SERVICIOS ECOLÓGICO";
        private string _subTituloReporte = "MARINO COSTERO -PSEMC-";

        public iRutaDeDescarga PruebaInforme(iDatosSolicitudPago datosDelReporte, string rutaImagenEncabezado, string rutaDeDescarga, string nombreArchivo)
        {
            var rutaParaDescargarElReporte = new iRutaDeDescarga();

            PdfDocument documento = new PdfDocument();

            PdfPage pagina = documento.AddPage();
            pagina.Size = PdfSharpCore.PageSize.Letter;
            pagina.Orientation = PdfSharpCore.PageOrientation.Portrait;

            XGraphics gfx = XGraphics.FromPdfPage(pagina);
            var tf = new XTextFormatter(gfx);

            _xImageMembrete = XImage.FromFile(rutaImagenEncabezado);

            _dibujaMembrete(gfx, pagina);
            _dibujaSubTitulo(gfx, pagina);

            _margenTop += 80;
            _dibujaNumeroSolicitud(gfx, pagina, datosDelReporte.numeroSolicitudPago);

            _margenTop += 15;
            _dibujaNumeroContrato(gfx, datosDelReporte.numeroContrato, pagina);
            _dibujaNumeroCuota(gfx, pagina, datosDelReporte.numeroCuota.ToString());

            _margenTop += 15;
            _dibujaFechaSolicitud(gfx, pagina, datosDelReporte.fechaSolicitudPago);

            _margenTop += 30;
            _dibujaNombreCliente(gfx, datosDelReporte.NombreCompleto, pagina);

            _margenTop += 20;
            _dibujaCedulaCliente(gfx, datosDelReporte.documentoID, pagina);

            _margenTop += 20;
            _dibujaCuentaCliente(gfx, datosDelReporte.cuentaClienteSINPE, pagina);
            _dibujaMoneda(gfx, datosDelReporte.tipoMoneda.ToString(), pagina);

            _margenTop += 40;
            _dibujaUnaLinea(gfx);

            _margenTop += 20;
            _dibujaBeneficiarios(gfx, datosDelReporte.totalAsociados.ToString(), pagina);
            _dibujaMontoPorParticipante(gfx, datosDelReporte.montoBase.ToString("N2"), pagina);

            _margenTop += 20;
            _dibujaMontoTotalContrato(gfx, datosDelReporte.montoContrato.ToString("N2"), pagina);
            _dibujaPorcentajePorPagar(gfx, datosDelReporte.deduccionAfectacion.ToString("P2"), pagina);

            _margenTop += 40;
            _dibujaUnaLinea(gfx);

            _margenTop += 20;
            _dibujaMontoBruto(gfx, datosDelReporte.montoSolicitud.ToString("N2"), pagina);
            
            _margenTop += 20;
            _dibujaDeducciones(gfx, datosDelReporte.montoNetoSolicitud.ToString("N2"), pagina);

            _margenTop += 20;
            _dibujaMontoNetoPorPagar(gfx, datosDelReporte.montoNetoSolicitud.ToString("N2"), pagina);

            _margenTop += 40;
            _dibujaUnaLinea(gfx);

            _margenTop += 20;
            _dibujaObservaciones(gfx, datosDelReporte.NombreCompleto, pagina);

            _margenTop += 40;
            _dibujaUnaLinea(gfx);

            _dibujaSolicitudAprobada(gfx, pagina);
            _dibujaAprobador(gfx, "José Arnulfo Rodriguez Zúñiga", pagina);
            _dibujaLineaParaFirmar(gfx);
            _dibujaOficinaRegional(gfx, pagina);

            _dibujaFechaActual(gfx, pagina);
            _dibujaNumeroUnico(gfx, pagina);

            documento.Save(rutaDeDescarga + nombreArchivo);

            rutaParaDescargarElReporte = new iRutaDeDescarga()
            {
                mensaje = string.Empty,
                resultado = "1",
                nombreArchivo = nombreArchivo
            };

            return rutaParaDescargarElReporte;
        }


        #region FUENTES Y FORMATO
        private XFont _fuenteNombreReporteNegrita()
        {
            return new XFont("Calibri", 11, XFontStyle.Bold);
        }

        private XFont _fuenteRegular()
        {
            return new XFont("Calibri", 11, XFontStyle.Regular);
        }

        private XFont _fuenteRegularNegrita()
        {
            return new XFont("Calibri", 11, XFontStyle.Bold);
        }

        private XPen _xpenParaLinea()
        {
            return new XPen(XColors.Black, 0.5);
        }

        private XStringFormat _formato()
        {
            XStringFormat formatoDelDocumento = new XStringFormat();

            formatoDelDocumento.LineAlignment = XLineAlignment.Near;
            formatoDelDocumento.Alignment = XStringAlignment.Near;

            return formatoDelDocumento;
        }

        #endregion

        #region UTILERIAS
        private void _dibujaUnaLinea(XGraphics gfx)
        {
            var puntoUno = new XPoint(_margenIzquierda, _margenTop);
            var puntoDos = new XPoint(_margenIzquierda + 500, _margenTop);
            gfx.DrawLine(_xpenParaLinea(), puntoUno, puntoDos);
        }

        private void _dibujaLineaParaFirmar(XGraphics gfx)
        {
            var puntoUno = new XPoint(_margenIzquierda + 160, 670);
            var puntoDos = new XPoint(_margenIzquierda + 360, 670);
            gfx.DrawLine(_xpenParaLinea(), puntoUno, puntoDos);
        }

        private string _tipoDeMonedaEnLetras(string tipoMoneda)
        {
            string tipoMonedaEnLetras = string.Empty;
            switch(tipoMoneda)
            {
                case "1":
                    tipoMonedaEnLetras = "Colones";
                    break;

                case "2":
                    tipoMonedaEnLetras = "Dólares";
                    break;

                case "3":
                    tipoMonedaEnLetras = "Euros";
                    break;

                default:
                    tipoMonedaEnLetras = "Colones";
                    break;
            }

            return tipoMonedaEnLetras;
        }

        #endregion

        #region DIBUJOS ENCABEZADOS DEL REPORTE

        private void _dibujaMembrete(XGraphics gfx, PdfPage pagina)
        {
            var layoutDelTitulo = new XRect(0, -315, pagina.Width, pagina.Height);
            var formatoDelTitulo = XStringFormats.Center;
            gfx.DrawImage(_xImageMembrete, _margenIzquierda, 5, 500, 40);
            gfx.DrawString(_tituloReporte, _fuenteNombreReporteNegrita(), XBrushes.Black, layoutDelTitulo, formatoDelTitulo);
        }

        private void _dibujaSubTitulo(XGraphics gfx, PdfPage pagina)
        {
            var layoutDelSubTitulo = new XRect(0, -300, pagina.Width, pagina.Height);
            var formatoDelSubTitulo = XStringFormats.Center;
            gfx.DrawString(_subTituloReporte, _fuenteNombreReporteNegrita(), XBrushes.Black, layoutDelSubTitulo, formatoDelSubTitulo);
        }

        private void _dibujaNumeroSolicitud(XGraphics gfx, PdfPage pagina, string numeroSolicitud)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Número de Solicitud:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(numeroSolicitud, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaNumeroContrato(XGraphics gfx, string numeroContrato, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Número de Contrato:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(numeroContrato, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaNumeroCuota(XGraphics gfx, PdfPage pagina, string numeroCuota)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Número de Cuota:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(numeroCuota, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaFechaSolicitud(XGraphics gfx, PdfPage pagina, string fechaSolicitud)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Fecha de Solicitud:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(fechaSolicitud.ToString(), _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        #endregion

        #region DIBUJA CUERPO DEL REPORTE

        private void _dibujaNombreCliente(XGraphics gfx, string nombreCliente, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Nombre del Cliente:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            XTextFormatter formatter = new XTextFormatter(gfx);
            formatter.DrawString(nombreCliente, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaCedulaCliente(XGraphics gfx, string cedula, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Cédula Cliente:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(cedula, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaCuentaCliente(XGraphics gfx, string cuentaCliente, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Depositar en la Cuenta:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(cuentaCliente, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaMoneda(XGraphics gfx, string tipoMoneda, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Moneda:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            string moneda = _tipoDeMonedaEnLetras(tipoMoneda);

            gfx.DrawString(moneda, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaBeneficiarios(XGraphics gfx, string numeroBeneficiarios, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Beneficiarios participantes:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(numeroBeneficiarios, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaMontoPorParticipante(XGraphics gfx, string montoPorParticipante, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Monto Por Participante:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(montoPorParticipante, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaMontoTotalContrato(XGraphics gfx, string montoTotalContrato, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Monto Total Contrato:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(montoTotalContrato, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaPorcentajePorPagar(XGraphics gfx, string porcentajePorPagar, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda + 275, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 400, _margenTop, pagina.Width, 15);

            gfx.DrawString("Porcentaje por Pagar:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(porcentajePorPagar, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaMontoBruto(XGraphics gfx, string montoBruto, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Monto Bruto:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(montoBruto, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaDeducciones(XGraphics gfx, string deducciones, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("(-) Deducciones:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(deducciones, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaMontoNetoPorPagar(XGraphics gfx, string montoNeto, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda + 125, _margenTop, pagina.Width, 15);

            gfx.DrawString("Monto Neto por Pagar:", _fuenteRegular(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            gfx.DrawString(montoNeto, _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        private void _dibujaObservaciones(XGraphics gfx, string observaciones, PdfPage pagina)
        {
            var rectangulo = new XRect(_margenIzquierda, _margenTop, 110, 15);
            var textoRectangulo = new XRect(_margenIzquierda, _margenTop + 20, pagina.Width, 15);

            gfx.DrawString("Observaciones:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectangulo.X + 5, rectangulo.Y + 2, rectangulo.Width - 5, 34), _formato());

            XTextFormatter formatter = new XTextFormatter(gfx);
            formatter.DrawString(observaciones, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectangulo.X + 5, textoRectangulo.Y + 2, textoRectangulo.Width - 5, 34), _formato());
        }

        #endregion

        #region DIBUJA PIE DE PAGINA

        private void _dibujaSolicitudAprobada(XGraphics gfx, PdfPage pagina)
        {
            var layoutSolicitudAprobada = new XRect(_margenIzquierda, 250, pagina.Width, pagina.Height);
            var formatoSolicitudAprobada = XStringFormats.CenterLeft;

            gfx.DrawString("Solicitud Aprobada Por:", _fuenteRegular(), XBrushes.Black, layoutSolicitudAprobada, formatoSolicitudAprobada);
        }

        private void _dibujaAprobador(XGraphics gfx, string aprobador, PdfPage pagina)
        {
            var layoutAprobador = new XRect(_margenIzquierda, 265, pagina.Width, pagina.Height);
            var formatoAprobador = XStringFormats.CenterLeft;

            gfx.DrawString(aprobador, _fuenteRegular(), XBrushes.Black, layoutAprobador, formatoAprobador);
        }

        private void _dibujaOficinaRegional(XGraphics gfx, PdfPage pagina)
        {
            var layoutDeOficina = new XRect(_margenIzquierda, 280, pagina.Width, pagina.Height);
            var formatoDeOficina = XStringFormats.CenterLeft;

            gfx.DrawString("Jefe de Oficina Regional", _fuenteRegular(), XBrushes.Black, layoutDeOficina, formatoDeOficina);
        }

        private void _dibujaFechaActual(XGraphics gfx, PdfPage pagina)
        {
            DateTime hoy = DateTime.Now;
            
            var formatoFechaActual = XStringFormats.Center;
            var layoutFechaActual = new XRect(0, 360, pagina.Width, pagina.Height);

            gfx.DrawString(hoy.ToString("dd/MM/yyyy HH:mm:ss.fff"), _fuenteRegular(), XBrushes.Black, layoutFechaActual, formatoFechaActual);
        }

        private void _dibujaNumeroUnico(XGraphics gfx, PdfPage pagina)
        {
            Guid numeroReporteUnico = Guid.NewGuid();

            var layoutNumeroUnico = new XRect(0, 375, pagina.Width, pagina.Height);
            var formatoNumeroUnico = XStringFormats.Center;
            
            gfx.DrawString(numeroReporteUnico.ToString().ToUpper(), _fuenteRegular(), XBrushes.Black, layoutNumeroUnico, formatoNumeroUnico);
        }

        #endregion

    }
}
