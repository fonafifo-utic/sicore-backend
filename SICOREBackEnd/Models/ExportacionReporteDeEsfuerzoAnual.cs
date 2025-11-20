using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Utils;
using SICOREBackEnd.Models;
using SICOREBackEnd.Utils;

namespace SICOREBackEnd.Models
{
    public class ExportacionReporteDeEsfuerzoAnual
    {
        private int _margenIzquierda = 50;
        private int _margenTop = 80;
        private string _funcionarioEjecutor = "Agente: ";
        private XImage _xImageMembrete;

        public iRutaDeDescargaDelPDF ExportaReporteDeEsfuerzoEnPDF(IEnumerable<iReporteEsfuerzoAnualColaboradorPDF> datosDelReporte, IEnumerable<iDesgloseEsfuerzoColaborador> desglose,
                                                                    string rutaImagenEncabezado, string rutaDeDescarga, string nombreArchivo, string funcionario)
        {

            var rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF();
            var columnasDelReporte = _construyeLasColumnasDelReporte();
            var columnasDelDesglose = _construyeLasColumnasDelDetalleDelReporte();
            var columnasDelDetalleDelReporte = _construyeLasColumnasDelDetalleDelReporte();

            int annoActual = DateTime.Now.Year;
            DateTime primerDiaDelAnno = new DateTime(annoActual, 1, 1);
            DateTime hoy = DateTime.Today;

            string rangoFechas = $"Desde el: {primerDiaDelAnno.ToShortDateString().Replace('/','-')} hasta el: {hoy.ToShortDateString().Replace('/', '-')}";
            string quienGeneraReporte = funcionario;
            string agente = datosDelReporte.Select(datos => datos.agente).FirstOrDefault();
            string tituloReporte = "Reporte de Esfuerzo por Agente";

            try
            {
                PdfDocument documento = new PdfDocument();
                documento.Info.Title = tituloReporte;

                PdfPage pagina = documento.AddPage();
                double alturaPagina = pagina.Height;

                XGraphics gfx = XGraphics.FromPdfPage(pagina);
                var tf = new XTextFormatter(gfx);

                _xImageMembrete = XImage.FromFile(rutaImagenEncabezado);

                _dibujaMembrete(gfx, rutaImagenEncabezado);

                _margenTop += 20;
                gfx.DrawString(tituloReporte, _fuenteNombreReporteNegrita(), XBrushes.Black, _margenIzquierda, _margenTop);

                _margenTop += 40;
                _dibujaParametroFechas(gfx, rangoFechas, pagina);

                _margenTop += 15;
                _dibujaAutorGeneradorReporte(gfx, pagina, quienGeneraReporte);

                _margenTop += 40;
                _margenIzquierda = 50;
                _dibujaFuncionarioAgrupador(gfx, agente);

                foreach (var columna in columnasDelReporte)
                {
                    _dibujaColumnasReporte(columna, gfx);
                }

                foreach (var dato in datosDelReporte)
                {
                    _margenTop += 15;
                    _margenIzquierda = 50;
                    _dibujaCantidad(gfx, dato.cantidad);

                    _margenIzquierda += 60;
                    _dibujaMonto(gfx, dato.monto);

                    _margenIzquierda += 60;
                    _dibujaFechaTransferencia(gfx, dato.ultimaVenta);
                }

                _margenIzquierda = 50;
                _dibujaCeldaDelDesglose(gfx);

                foreach (var columna in columnasDelDesglose)
                {
                    _dibujaColumnasDelDesglose(columna, gfx);
                }

                foreach (var dato in desglose)
                {
                    _margenTop += 15;
                    _margenIzquierda = 50;
                    _dibujaCertificado(gfx, dato.certificado);

                    _margenIzquierda += 60;
                    _dibujaCliente(gfx, dato.cliente);

                    _margenIzquierda += 200;
                    _dibujaEmisionCertificado(gfx, dato.fecha);

                    _margenIzquierda += 91;
                    _dibujaCantidadDelDesglose(gfx, dato.cantidad);

                    _margenIzquierda += 90;
                    _dibujaMontoDelDesglose(gfx, dato.monto);

                    
                    if (_margenTop >= (alturaPagina - 94))
                    {
                        _margenTop = 80;
                        _margenIzquierda = 50;

                        PdfPage paginaDos = documento.AddPage();
                        gfx = XGraphics.FromPdfPage(paginaDos);
                        tf = new XTextFormatter(gfx);
                        gfx.DrawImage(_xImageMembrete, _margenIzquierda, 10, 430, 35);
                    }
                }

                documento.Save(rutaDeDescarga + nombreArchivo);

                if (documento.PageCount > 0)
                {
                    rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF()
                    {
                        mensaje = string.Empty,
                        resultado = "1",
                        nombreArchivo = nombreArchivo
                    };
                }
                else
                {
                    rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF()
                    {
                        mensaje = "ERROR: No se generó el reporte.",
                        resultado = "-1",
                        nombreArchivo = nombreArchivo
                    };
                }

                return rutaParaDescargarElReporte;

            } catch (Exception ex)
            {
                rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF()
                {
                    mensaje = "ERROR: " + ex.Message,
                    resultado = "-2",
                    nombreArchivo = nombreArchivo
                };

                return rutaParaDescargarElReporte;
            }
        }

        #region FUENTES Y FORMATO
        private XFont _fuenteNombreReporte()
        {
            return new XFont("Arial", 12, XFontStyle.Regular);
        }

        private XFont _fuenteNombreReporteNegrita()
        {
            return new XFont("Arial", 12, XFontStyle.Bold);
        }

        private XFont _fuenteRegular()
        {
            return new XFont("Arial", 10, XFontStyle.Regular);
        }

        private XFont _fuenteRegularNegrita()
        {
            return new XFont("Arial", 10, XFontStyle.Bold);
        }

        private XFont _fuentePequennaNegrita()
        {
            return new XFont("Arial", 7, XFontStyle.Bold);
        }

        private XFont _fuentePequennaRegular()
        {
            return new XFont("Arial", 6, XFontStyle.Bold);
        }

        private XPen _xpen()
        {
            return new XPen(XColors.Black, 0.4);
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

        private List<iColumnas> _construyeLasColumnasDelReporte()
        {
            var columnas = new List<iColumnas>();
            columnas.Add(new iColumnas { nombreColumna = "Cantidad" });
            columnas.Add(new iColumnas { nombreColumna = "Monto" });
            columnas.Add(new iColumnas { nombreColumna = "Últ. Venta" });

            return columnas;
        }

        private List<iColumnas> _construyeLasColumnasDelDetalleDelReporte()
        {
            var columnas = new List<iColumnas>();
            columnas.Add(new iColumnas { nombreColumna = "Certificado" });
            columnas.Add(new iColumnas { nombreColumna = "Cliente" });
            columnas.Add(new iColumnas { nombreColumna = "Emisión" });
            columnas.Add(new iColumnas { nombreColumna = "Cantidad" }); 
            columnas.Add(new iColumnas { nombreColumna = "Monto" });

            return columnas;
        }

        private int _calculaAnchoColumnaSegunColumna(iColumnas columna)
        {
            int margen = 0;
            switch (columna.nombreColumna)
            {
                case "Cantidad":
                    margen = 0;
                    break;

                case "Monto":
                    margen = 61;
                    break;

                case "Últ. Venta":
                    margen = 61;
                    break;
            }

            return margen;
        }

        private int _calculaAnchoColumnaSegunColumnaDesglose(iColumnas columna)
        {
            int margen = 0;
            switch (columna.nombreColumna)
            {
                case "Certificado":
                    margen = 0;
                    break;

                case "Cliente":
                    margen = 61;
                    break;

                case "Emisión":
                    margen = 201;
                    break;

                case "Cantidad":
                    margen = 91;
                    break;

                case "Monto":
                    margen = 91;
                    break;
            }

            return margen;
        }
        #endregion

        #region DIBUJOS SECUNDARIOS DEL REPORTE

        private void _dibujaMembrete(XGraphics gfx, string rutaImagenEncabezado)
        {
            gfx.DrawImage(_xImageMembrete, _margenIzquierda, 10, 430, 35);
            gfx.DrawString(Constantes.DEPARTAMENTO, _fuenteRegular(), XBrushes.Black, _margenIzquierda, _margenTop);
        }

        private void _dibujaParametroFechas(XGraphics gfx, string rangoFechas, PdfPage pagina)
        {
            var rectanguloRangoFechas = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.WhiteSmoke, rectanguloRangoFechas);
            gfx.DrawString("Rango de Fechas:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectanguloRangoFechas.X + 5, rectanguloRangoFechas.Y + 2, rectanguloRangoFechas.Width - 5, 34), _formato());

            var textoRectanguloRangoFechas = new XRect(_margenIzquierda + 110, _margenTop - 10, pagina.Width - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, textoRectanguloRangoFechas);
            gfx.DrawString(rangoFechas, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectanguloRangoFechas.X + 5, textoRectanguloRangoFechas.Y + 2, textoRectanguloRangoFechas.Width - 5, 34), _formato());
        }

        private void _dibujaAutorGeneradorReporte(XGraphics gfx, PdfPage pagina, string funcionario)
        {
            var rectanguloFuncionario = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.WhiteSmoke, rectanguloFuncionario);
            gfx.DrawString("Generado por:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectanguloFuncionario.X + 5, rectanguloFuncionario.Y + 2, rectanguloFuncionario.Width - 5, 34), _formato());

            var textoRectanguloFuncionario = new XRect(_margenIzquierda + 110, _margenTop - 10, pagina.Width - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, textoRectanguloFuncionario);
            gfx.DrawString(funcionario, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectanguloFuncionario.X + 5, textoRectanguloFuncionario.Y + 2, textoRectanguloFuncionario.Width - 5, 34), _formato());
        }

        private void _dibujaFuncionarioAgrupador(XGraphics gfx, string usuarioAgrupador)
        {
            _funcionarioEjecutor = _funcionarioEjecutor + usuarioAgrupador;
            var celdaAgrupadora = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);

            gfx.DrawString(_funcionarioEjecutor, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celdaAgrupadora.X + 5, celdaAgrupadora.Y + 5, celdaAgrupadora.Width - 5, 34), _formato());

            _margenTop += 18;
            var celda = new XRect(_margenIzquierda, _margenTop - 10, 190, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.LightGray, celda);
        }

        private void _dibujaCeldaDelDesglose(XGraphics gfx)
        {
            _margenTop += 25;
            var celda = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.LightGray, celda);
        }

        private void _dibujaColumnasReporte(iColumnas columna, XGraphics gfx)
        {
            _margenIzquierda += _calculaAnchoColumnaSegunColumna(columna);
            var celda = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);
            gfx.DrawString(columna.nombreColumna, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celda.X + 3, celda.Y + 3, celda.Width - 5, 34), _formato());
        }

        private void _dibujaColumnasDelDesglose(iColumnas columna, XGraphics gfx)
        {
            _margenIzquierda += _calculaAnchoColumnaSegunColumnaDesglose(columna);
            var celda = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);
            gfx.DrawString(columna.nombreColumna, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celda.X + 3, celda.Y + 3, celda.Width - 5, 34), _formato());
        }

        #endregion

        #region DIBUJA CUERPO DEL REPORTE
        private void _dibujaCantidad(XGraphics gfx, decimal cantidad)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 60, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(cantidad.ToString() + " tons CO2", _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaMonto(XGraphics gfx, decimal montoTotalDolares)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 60, 15);
            string monto = "$ " + montoTotalDolares.ToString();
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(monto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaFechaTransferencia(XGraphics gfx, string fechaTransferencia)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 70, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(fechaTransferencia, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaCertificado(XGraphics gfx, string certificado)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 60, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(certificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaCliente(XGraphics gfx, string cliente)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 200, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(cliente, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaEmisionCertificado(XGraphics gfx, string fechaEmisionCertificado)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 91, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(fechaEmisionCertificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaCantidadDelDesglose(XGraphics gfx, decimal cantidad)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 90, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(cantidad.ToString() + " tons CO2", _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaMontoDelDesglose(XGraphics gfx, decimal montoTotalDolares)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 59, 15);
            string monto = "$ " + montoTotalDolares.ToString();
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(monto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        #endregion
    }

}
