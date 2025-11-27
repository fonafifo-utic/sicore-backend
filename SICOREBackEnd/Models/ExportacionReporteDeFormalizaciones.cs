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
    public class ExportacionReporteDeFormalizaciones
    {
        private int _margenIzquierda = 50;
        private int _margenTop = 80;
        private string _funcionarioEjecutor = "Funcionario: ";
        private string _sector = "Sector: ";
        private XImage _xImageMembrete;
        private decimal _montoTotalSumarizado = 0;
        private decimal _totalToneladasCO2 = 0;

        public iRutaDeDescargaDelPDF ExportaFormalizacionesEnPDF(IEnumerable<iReporteListadoFormalizacionMensual> datosDelReporte, string rutaImagenEncabezado, string rutaDeDescarga, string nombreArchivo)
        {
            var rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF();
            var columnas = _construyeLasColumnasDelReporte();

            string rangoFechas = datosDelReporte.Select(datos => datos.rangoDeFechas).FirstOrDefault();
            string funcionario = datosDelReporte.Select(datos => datos.funcionario).FirstOrDefault();
            string sectoresFiltrados = datosDelReporte.Select(datos => datos.sectoresFiltrados).FirstOrDefault();

            var listaUsuarios = datosDelReporte.Select(registro => registro.usuario).Distinct();
            var listaSectores = datosDelReporte.Select(registro => registro.sectorComercial).Distinct();

            string tituloReporte = "Reporte de Formalizaciones";

            try
            {
                PdfDocument documento = new PdfDocument();
                documento.Info.Title = tituloReporte;

                PdfPage pagina = documento.AddPage();
                pagina.Orientation = PdfSharpCore.PageOrientation.Landscape;
                double alturaPagina = pagina.Height;

                XGraphics gfx = XGraphics.FromPdfPage(pagina);
                var tf = new XTextFormatter(gfx);

                _xImageMembrete = XImage.FromFile(rutaImagenEncabezado);

                _dibujaMembrete(gfx);

                _margenTop += 20;
                gfx.DrawString(tituloReporte, _fuenteNombreReporteNegrita(), XBrushes.Black, _margenIzquierda, _margenTop);

                _margenTop += 40;
                _dibujaParametroFechas(gfx, rangoFechas, pagina.Width);

                _margenTop += 15;
                _dibujaAutorGeneradorReporte(gfx, funcionario, pagina.Width);

                _margenTop += 15;
                _dibujaFiltroPorSectores(gfx, pagina, sectoresFiltrados);

                _margenTop += 15;
                foreach (var sector in listaSectores)
                {
                    _margenTop += 15;
                    _margenIzquierda = 50;
                    _dibujaSectorAgrupador(gfx, sector);

                    foreach (var usuarioAgrupador in listaUsuarios)
                    {
                        _margenTop += 15;
                        _margenIzquierda = 50;
                        _dibujaFuncionarioAgrupador(gfx, usuarioAgrupador, pagina.Width);

                        foreach (var columna in columnas)
                        {
                            _dibujaColumnasReporte(columna, gfx);
                        }

                        foreach (var dato in datosDelReporte)
                        {
                            if (usuarioAgrupador == dato.usuario)
                            {
                                _margenTop += 15;
                                _margenIzquierda = 50;
                                _dibujaConsecutivo(gfx, dato.consecutivo);

                                _margenIzquierda += 65;
                                _dibujaCliente(gfx, dato.nombreCliente);

                                _margenIzquierda += 234;
                                _dibujaFecha(gfx, dato.fechaYHora);

                                _margenIzquierda += 58;
                                _dibujaMonto(gfx, dato.montoDolares);

                                _margenIzquierda += 40;
                                _dibujaNumeroTransferencia(gfx, dato.numeroTransferencia);

                                _margenIzquierda += 42;
                                _dibujaNumeroFactura(gfx, dato.numeroFacturaFonafifo);

                                _margenIzquierda += 96;
                                _dibujaCreditoOrDebito(gfx, dato.creditoDebito);

                                _margenIzquierda += 42;
                                _dibujaTipoCompra(gfx, dato.tipoCompra);
                            }

                            if (_margenTop >= (alturaPagina - 64))
                            {
                                _margenTop = 80;    
                                _margenIzquierda = 50;

                                PdfPage paginaDos = documento.AddPage();
                                paginaDos.Orientation = PdfSharpCore.PageOrientation.Landscape;
                                gfx = XGraphics.FromPdfPage(paginaDos);
                                tf = new XTextFormatter(gfx);
                                gfx.DrawImage(_xImageMembrete, _margenIzquierda, 10, 430, 35);
                            }

                        }

                        _dibujaPieDelProximoAgrupador(gfx);
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

            }
            catch (Exception ex)
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
            columnas.Add(new iColumnas { nombreColumna = "Cotización" });
            columnas.Add(new iColumnas { nombreColumna = "Cliente" });
            columnas.Add(new iColumnas { nombreColumna = "Fecha" });
            columnas.Add(new iColumnas { nombreColumna = "Monto" });
            columnas.Add(new iColumnas { nombreColumna = "Comp." });
            columnas.Add(new iColumnas { nombreColumna = "Factura" });
            columnas.Add(new iColumnas { nombreColumna = "Tipo" });
            columnas.Add(new iColumnas { nombreColumna = "Compra" });
            
            return columnas;
        }

        private int _calculaAnchoColumnaSegunColumna(iColumnas columna)
        {
            int margen = 0;
            switch (columna.nombreColumna)
            {
                case "Cotización":
                    margen = 0;
                    break;

                case "Cliente":
                    margen = 65;
                    break;

                case "Fecha":
                    margen = 234;
                    break;

                case "Monto":
                    margen = 61;
                    break;

                case "Comp.":
                    margen = 40;
                    break;

                case "Factura":
                    margen = 40;
                    break;

                case "Tipo":
                    margen = 94;
                    break;

                case "Compra":
                    margen = 43;
                    break;
            }

            return margen;
        }

        private string _daFormatoAlCotizacion(int consecutivo)
        {
            DateTime hoy = DateTime.Now;
            string cotizacion = "DDC-CO-";

            if (consecutivo.ToString().Length == 1) cotizacion = cotizacion + "00" + consecutivo.ToString() + "-" + hoy.Year;
            if (consecutivo.ToString().Length == 2) cotizacion = cotizacion + "0" + consecutivo.ToString() + "-" + hoy.Year;
            if (consecutivo.ToString().Length == 3) cotizacion = cotizacion + consecutivo.ToString() + "-" + hoy.Year;

            return cotizacion;
        }

        #endregion

        #region DIBUJOS SECUNDARIOS DEL REPORTE

        private void _dibujaMembrete(XGraphics gfx)
        {
            gfx.DrawImage(_xImageMembrete, _margenIzquierda, 10, 430, 35);
            gfx.DrawString(Constantes.DEPARTAMENTO, _fuenteRegular(), XBrushes.Black, _margenIzquierda, _margenTop);
        }

        private void _dibujaParametroFechas(XGraphics gfx, string rangoFechas, XUnit anchoPagina)
        {
            var rectanguloRangoFechas = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.WhiteSmoke, rectanguloRangoFechas);
            gfx.DrawString("Rango de Fechas:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectanguloRangoFechas.X + 5, rectanguloRangoFechas.Y + 2, rectanguloRangoFechas.Width - 5, 34), _formato());

            var textoRectanguloRangoFechas = new XRect(_margenIzquierda + 110, _margenTop - 10, anchoPagina - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, textoRectanguloRangoFechas);
            gfx.DrawString(rangoFechas, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectanguloRangoFechas.X + 5, textoRectanguloRangoFechas.Y + 2, textoRectanguloRangoFechas.Width - 5, 34), _formato());
        }

        private void _dibujaAutorGeneradorReporte(XGraphics gfx, string funcionario, XUnit anchoPagina)
        {
            var rectanguloFuncionario = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.WhiteSmoke, rectanguloFuncionario);
            gfx.DrawString("Generado por:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectanguloFuncionario.X + 5, rectanguloFuncionario.Y + 2, rectanguloFuncionario.Width - 5, 34), _formato());

            var textoRectanguloFuncionario = new XRect(_margenIzquierda + 110, _margenTop - 10, anchoPagina - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, textoRectanguloFuncionario);
            gfx.DrawString(funcionario, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectanguloFuncionario.X + 5, textoRectanguloFuncionario.Y + 2, textoRectanguloFuncionario.Width - 5, 34), _formato());
        }

        private void _dibujaFiltroPorSectores(XGraphics gfx, PdfPage pagina, string sectores)
        {
            var rectanguloSector = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.WhiteSmoke, rectanguloSector);
            gfx.DrawString("Sectores:", _fuenteRegularNegrita(), XBrushes.Black,
                new XRect(rectanguloSector.X + 5, rectanguloSector.Y + 2, rectanguloSector.Width - 5, 34), _formato());

            var textoRectanguloSectores = new XRect(_margenIzquierda + 110, _margenTop - 10, pagina.Width - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, textoRectanguloSectores);
            gfx.DrawString(sectores, _fuenteRegular(), XBrushes.Black,
                new XRect(textoRectanguloSectores.X + 5, textoRectanguloSectores.Y + 2, textoRectanguloSectores.Width - 5, 34), _formato());
        }

        private void _dibujaFuncionarioAgrupador(XGraphics gfx, string usuarioAgrupador, XUnit anchoPagina)
        {
            _funcionarioEjecutor = _funcionarioEjecutor + usuarioAgrupador;
            var celdaAgrupadora = new XRect(_margenIzquierda, _margenTop - 10, anchoPagina - 204, 15);

            gfx.DrawString(_funcionarioEjecutor, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celdaAgrupadora.X + 5, celdaAgrupadora.Y + 5, celdaAgrupadora.Width - 5, 34), _formato());

            _margenTop += 18;
            var celda = new XRect(_margenIzquierda, _margenTop - 10, anchoPagina - 204, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.LightGray, celda);
        }

        private void _dibujaColumnasReporte(iColumnas columna, XGraphics gfx)
        {
            _margenIzquierda += _calculaAnchoColumnaSegunColumna(columna);
            var celda = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);
            gfx.DrawString(columna.nombreColumna, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celda.X + 3, celda.Y + 3, celda.Width - 5, 34), _formato());
        }

        private void _dibujaPieDelProximoAgrupador(XGraphics gfx)
        {
            _margenTop += 15;
            int margenIzquierda = _margenIzquierda;

            _margenIzquierda = 390;
            _dibujaMontoTotalSumarizado(gfx);
            _montoTotalSumarizado = 0;

            _margenTop += 15;
            _margenIzquierda = margenIzquierda;
            _funcionarioEjecutor = "Funcionario: ";
        }

        private void _dibujaMontoTotalSumarizado(XGraphics gfx)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 37, 15);
            string montoTotalFormato = _montoTotalSumarizado.ToString("#.##");
            string elMonto = "Total: $ " + montoTotalFormato;
            gfx.DrawString(elMonto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaTotalToneladasCO2(XGraphics gfx)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 37, 15);
            string totalToneladas = "Total: " + _totalToneladasCO2.ToString("#.##");
            gfx.DrawString(totalToneladas, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaSectorAgrupador(XGraphics gfx, string sector)
        {
            _sector = _sector + sector;
            var celdaAgrupadora = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);

            gfx.DrawString(_sector, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celdaAgrupadora.X + 5, celdaAgrupadora.Y + 5, celdaAgrupadora.Width - 5, 34), _formato());
        }

        #endregion

        #region DIBUJA CUERPO DEL REPORTE

        private void _dibujaSectorComercial(XGraphics gfx, string sectorComercial)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 110, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(sectorComercial, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaCliente(XGraphics gfx, string nombreCliente)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 235, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(nombreCliente, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaConsecutivo(XGraphics gfx, int consecutivo)
        {
            string consecutivoConFormato = _daFormatoAlCotizacion(consecutivo);
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 65, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(consecutivoConFormato, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaMonto(XGraphics gfx, decimal montoTotalDolares)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 40, 15);
            string monto = "$ " + montoTotalDolares.ToString();
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(monto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());

            _montoTotalSumarizado += montoTotalDolares;
        }

        private void _dibujaFecha(XGraphics gfx, string fechaYHora)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 58, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(fechaYHora, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaNumeroTransferencia(XGraphics gfx, string numeroTransferencia)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 42, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(numeroTransferencia, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaNumeroFactura(XGraphics gfx, string numeroFactura)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 96, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(numeroFactura, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaCreditoOrDebito(XGraphics gfx, string creditoDebito)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 42, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(creditoDebito, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaJustificacion(XGraphics gfx, string justificacionCompra)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 141, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(justificacionCompra, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaTipoCompra(XGraphics gfx, string tipoCompra)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 61, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(tipoCompra, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        #endregion
    }
}
