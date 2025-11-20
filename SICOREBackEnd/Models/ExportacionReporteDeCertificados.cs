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
    public class iRutaDeDescargaDelPDF
    {
        public string nombreArchivo { get; set; }
        public string resultado { get; set; }
        public string mensaje { get; set; }
    }

    public class iColumnas
    {
        public string nombreColumna { get; set; }
    }

    public class ExportacionReporteDeCertificados
    {
        private int _margenIzquierda = 50;
        private int _margenTop = 80;
        private string _sector = "Sector: ";
        private string _funcionarioEjecutor = "Funcionario: ";
        private XImage _xImageMembrete;
        private decimal _montoTotalSumarizado = 0;

        public iRutaDeDescargaDelPDF ExportaCertificadosEnPDF(IEnumerable<iReporteListadoCertificadoMensual> datosDelReporte, string rutaImagenEncabezado, string rutaDeDescarga, string nombreArchivo)
        {
            var rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF();
            var columnas = _construyeLasColumnasDelReporteCertificados();

            string rangoFechas = datosDelReporte.Select(datos => datos.rangoDeFechas).FirstOrDefault();
            string funcionario = datosDelReporte.Select(datos => datos.funcionario).FirstOrDefault();
            string sectoresFiltrados = datosDelReporte.Select(datos => datos.sectoresFiltrados).FirstOrDefault();

            var listaUsuarios = datosDelReporte.Select(registro => registro.usuario).Distinct();
            var listaSectores = datosDelReporte.Select(registro => registro.sectorComercial).Distinct();

            string tituloReporte = "Reporte de Certificados";

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
                _dibujaAutorGeneradorReporte(gfx, pagina, funcionario);

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
                        _dibujaFuncionarioAgrupador(gfx, usuarioAgrupador);

                        foreach (var columna in columnas)
                        {
                            _dibujaColumnasReporte(columna, gfx);
                        }

                        foreach (var dato in datosDelReporte)
                        {
                            if ((usuarioAgrupador == dato.usuario) && (sector == dato.sectorComercial))
                            {
                                _margenTop += 15;
                                _margenIzquierda = 50;
                                _dibujaNumeroCertificado(gfx, dato.numeroCertificado);

                                _margenIzquierda += 42;
                                _dibujaNombreCertificado(gfx, dato.nombreCertificado);

                                _margenIzquierda += 115;
                                _dibujaNombreCliente(gfx, dato.nombreCotizante);

                                _margenIzquierda += 115; 
                                _dibujaEmisionCertificado(gfx, dato.fechaEmisionCertificado);

                                _margenIzquierda += 61;
                                _dibujaMontoTransferencia(gfx, dato.montoDeTransferencia, dato.montoTransferencia);

                                _margenIzquierda += 36;
                                _dibujaFechaTransferencia(gfx, dato.fechaTransferencia);

                                _margenIzquierda += 43;
                                _dibujaAnotaciones(gfx, dato.anotaciones);
                            }

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

        private XPen _xpen ()
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

        private List<iColumnas> _construyeLasColumnasDelReporteCertificados()
        {
            var columnas = new List<iColumnas>();
            columnas.Add(new iColumnas { nombreColumna = "Cert." });
            columnas.Add(new iColumnas { nombreColumna = "Nombre" });
            columnas.Add(new iColumnas { nombreColumna = "Cliente" });
            columnas.Add(new iColumnas { nombreColumna = "Emisión" });
            columnas.Add(new iColumnas { nombreColumna = "Monto" });
            columnas.Add(new iColumnas { nombreColumna = "Fecha" });
            columnas.Add(new iColumnas { nombreColumna = "Comentario" });

            return columnas;
        }

        private int _calculaAnchoColumnaSegunColumna(iColumnas columna)
        {
            int margen = 0;
            switch (columna.nombreColumna)
            {
                case "Cert.":
                    margen = 0;
                    break;

                case "Nombre":
                    margen = 42;
                    break;

                case "Cliente":
                    margen = 115;
                    break;

                case "Emisión":
                    margen = 115;
                    break;

                case "Monto":
                    margen = 61;
                    break;

                case "Fecha":
                    margen = 37;
                    break;

                case "Comentario":
                    margen = 44;
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

        private void _dibujaMontoTotalSumarizado(XGraphics gfx)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 37, 15);
            string montoTotalFormato = _montoTotalSumarizado.ToString("#.##");
            string elMonto = "Total: $ " + montoTotalFormato;
            gfx.DrawString(elMonto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaPieDelProximoAgrupador(XGraphics gfx)
        {
            _margenTop += 20;
            int margenIzquierda = _margenIzquierda;

            _margenIzquierda = 360;
            _dibujaMontoTotalSumarizado(gfx);
            _montoTotalSumarizado = 0;

            _margenTop += 15;
            _margenIzquierda = margenIzquierda;
            _funcionarioEjecutor = "Funcionario: ";
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

        private void _dibujaSectorAgrupador(XGraphics gfx, string sector)
        {
            _sector = "Sector: ";
            _sector = _sector + sector;
            var celdaAgrupadora = new XRect(_margenIzquierda, _margenTop - 10, 500, 15);

            gfx.DrawString(_sector, _fuentePequennaNegrita(), XBrushes.Black,
                new XRect(celdaAgrupadora.X + 5, celdaAgrupadora.Y + 5, celdaAgrupadora.Width - 5, 34), _formato());
        }

        #endregion

        #region DIBUJA CUERPO DEL REPORTE

        private void _dibujaNumeroCertificado(XGraphics gfx, string numeroCertificado)
        {
            var tf = new XTextFormatter(gfx);
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 42, 20);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(numeroCertificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaSectorComercial(XGraphics gfx, string sectorComercial)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 80, 15);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            gfx.DrawString(sectorComercial, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaNombreCertificado(XGraphics gfx, string nombreCertificado)
        {
            var tf = new XTextFormatter(gfx);
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 115, 20);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(nombreCertificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaNombreCliente(XGraphics gfx, string nombreCertificado)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 115, 20);
            var tf = new XTextFormatter(gfx);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(nombreCertificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaEmisionCertificado(XGraphics gfx, string fechaEmisionCertificado)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 61, 20);
            var tf = new XTextFormatter(gfx);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(fechaEmisionCertificado, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaMontoTransferencia(XGraphics gfx, string montoDeTransferencia, decimal montoTransferencia)
        {
            var tf = new XTextFormatter(gfx);
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 37, 20);
            string monto = "$ " + montoDeTransferencia;
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(monto, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());

            _montoTotalSumarizado += montoTransferencia;
        }

        private void _dibujaFechaTransferencia(XGraphics gfx, string fechaTransferencia)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 45, 20);
            var tf = new XTextFormatter(gfx);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(fechaTransferencia, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        private void _dibujaAnotaciones(XGraphics gfx, string anotaciones)
        {
            var celdaColumnaReporte = new XRect(_margenIzquierda, _margenTop - 10, 88, 20);
            var tf = new XTextFormatter(gfx);
            gfx.DrawRectangle(_xpen(), XBrushes.White, celdaColumnaReporte);
            tf.DrawString(anotaciones, _fuentePequennaRegular(), XBrushes.Black,
                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), _formato());
        }

        #endregion

    }
}
