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

namespace SICOREBackEnd.Utils
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

    public class GenerardorDocumentosPDF
    {
        public iRutaDeDescargaDelPDF Metodo(IEnumerable<iReporteListadoCertificadoMensual> datosDelReporte, string rutaImagenEncabezado, string rutaDeDescarga, string nombreArchivo)
        {
            var rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF();

            var columnas = new List<iColumnas>();
            columnas.Add(new iColumnas { nombreColumna  = "Cert."});
            columnas.Add(new iColumnas { nombreColumna  = "Sector"});
            columnas.Add(new iColumnas { nombreColumna  = "Cliente"});
            columnas.Add(new iColumnas { nombreColumna  = "Emisión"});
            columnas.Add(new iColumnas { nombreColumna  = "Monto"});
            columnas.Add(new iColumnas { nombreColumna  = "Fecha"});
            columnas.Add(new iColumnas { nombreColumna  = "Comentario"});

            try
            {
                var fuenteNombreReporte = new XFont("Arial", 12, XFontStyle.Regular);
                var fuenteNombreReporteNegrita = new XFont("Arial", 12, XFontStyle.Bold);

                var fuenteRegular = new XFont("Arial", 10, XFontStyle.Regular);
                var fuenteRegularNegrita = new XFont("Arial", 10, XFontStyle.Bold);

                var fuentePequennaNegrita = new XFont("Arial", 7, XFontStyle.Bold);
                var fuentePequennaRegular = new XFont("Arial", 6, XFontStyle.Regular);

                string rangoFechas = datosDelReporte.Select(datos => datos.rangoDeFechas).FirstOrDefault();
                string funcionario = datosDelReporte.Select(datos => datos.funcionario).FirstOrDefault();

                int margenIzquierda = 50;
                int margenTop = 80;

                string tituloReporte = "Reporte de Certificados";
                PdfDocument documento = new PdfDocument();
                documento.Info.Title = tituloReporte;
                PdfPage pagina = documento.AddPage();
                double alturaPagina = pagina.Height;
                XGraphics gfx = XGraphics.FromPdfPage(pagina);

                var xImageMembrete = XImage.FromFile(rutaImagenEncabezado);

                gfx.DrawImage(xImageMembrete, margenIzquierda, 10, 430, 35);
                gfx.DrawString("DEPARTAMENTO DE MERCADEO Y COMERCIALIZACIÓN", fuenteRegular, XBrushes.Black, margenIzquierda, margenTop);

                margenTop += 20;
                gfx.DrawString(tituloReporte, fuenteNombreReporteNegrita, XBrushes.Black, margenIzquierda, margenTop);

                var tf = new XTextFormatter(gfx);
                XPen xpen = new XPen(XColors.Black, 0.4);
                XStringFormat format = new XStringFormat();
                format.LineAlignment = XLineAlignment.Near;
                format.Alignment = XStringAlignment.Near;
                XBrush brush = XBrushes.Black;

                margenTop += 40;
                var rectanguloRangoFechas = new XRect(margenIzquierda, margenTop - 10, 110, 15);
                gfx.DrawRectangle(xpen, XBrushes.WhiteSmoke, rectanguloRangoFechas);
                gfx.DrawString("Rango de Fechas:", fuenteRegularNegrita, XBrushes.Black,
                    new XRect(rectanguloRangoFechas.X + 5, rectanguloRangoFechas.Y + 2, rectanguloRangoFechas.Width - 5, 34), format);
                
                var textoRectanguloRangoFechas = new XRect(margenIzquierda + 110, margenTop - 10, pagina.Width - 204, 15);
                gfx.DrawRectangle(xpen, XBrushes.White, textoRectanguloRangoFechas);
                gfx.DrawString(rangoFechas, fuenteRegular, XBrushes.Black,
                    new XRect(textoRectanguloRangoFechas.X + 5, textoRectanguloRangoFechas.Y + 2, textoRectanguloRangoFechas.Width - 5, 34), format);

                margenTop += 15;
                var rectanguloFuncionario = new XRect(margenIzquierda, margenTop - 10, 110, 15);
                gfx.DrawRectangle(xpen, XBrushes.WhiteSmoke, rectanguloFuncionario);
                gfx.DrawString("Elaborado por:", fuenteRegularNegrita, XBrushes.Black,
                    new XRect(rectanguloFuncionario.X + 5, rectanguloFuncionario.Y + 2, rectanguloFuncionario.Width - 5, 34), format);
                
                var textoRectanguloFuncionario = new XRect(margenIzquierda + 110, margenTop - 10, pagina.Width - 204, 15);
                gfx.DrawRectangle(xpen, XBrushes.White, textoRectanguloFuncionario);
                gfx.DrawString(funcionario, fuenteRegular, XBrushes.Black,
                    new XRect(textoRectanguloFuncionario.X + 5, textoRectanguloFuncionario.Y + 2, textoRectanguloFuncionario.Width - 5, 34), format);

                margenTop += 15;
                var listaUsuarios = datosDelReporte.Select(registro => registro.usuario).Distinct();
                string funcionarioEjecutor = "Funcionario: ";
                foreach (var usuarioAgrupador in listaUsuarios)
                {
                    margenTop += 15;
                    margenIzquierda = 50;
                    var celdaAgrupadora = new XRect(margenIzquierda, margenTop - 10, 500, 15);
                    funcionarioEjecutor = funcionarioEjecutor + " " + usuarioAgrupador;
                    gfx.DrawString(funcionarioEjecutor, fuentePequennaNegrita, XBrushes.Black,
                                new XRect(celdaAgrupadora.X + 5, celdaAgrupadora.Y + 5, celdaAgrupadora.Width - 5, 34), format);

                    margenTop += 18;
                    var celda = new XRect(margenIzquierda, margenTop - 10, 500, 15);
                    gfx.DrawRectangle(xpen, XBrushes.LightGray, celda);

                    foreach(var columna in columnas)
                    {
                        margenIzquierda += _calculaAnchoColumnaSegunColumna(columna);
                        celda = new XRect(margenIzquierda, margenTop - 10, 500, 15);
                        gfx.DrawString(columna.nombreColumna, fuentePequennaNegrita, XBrushes.Black,
                            new XRect(celda.X + 3, celda.Y + 3, celda.Width - 5, 34), format);
                    }

                    foreach (var dato in datosDelReporte)
                    {
                        if(usuarioAgrupador == dato.usuario)
                        {
                            margenTop += 15;
                            margenIzquierda = 50;
                            var celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 42, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.numeroCertificado, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 42;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 80, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.sectorComercial, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 80;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 103, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.nombreCertificado, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 103;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 61, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.fechaEmisionCertificado, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 61;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 37, 15);
                            string monto = "$ " + dato.montoTransferencia;
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(monto, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 37;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 44, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.fechaTransferencia, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);

                            margenIzquierda += 44;
                            celdaColumnaReporte = new XRect(margenIzquierda, margenTop - 10, 133, 15);
                            gfx.DrawRectangle(xpen, XBrushes.White, celdaColumnaReporte);
                            gfx.DrawString(dato.anotaciones, fuentePequennaRegular, XBrushes.Black,
                                new XRect(celdaColumnaReporte.X + 5, celdaColumnaReporte.Y + 5, celdaColumnaReporte.Width - 5, 34), format);
                        }

                        if (margenTop >= (alturaPagina - 94))
                        {
                            margenTop = 80;
                            margenIzquierda = 50;
                            PdfPage paginaDos = documento.AddPage();
                            gfx = XGraphics.FromPdfPage(paginaDos);
                            tf = new XTextFormatter(gfx);
                            gfx.DrawImage(xImageMembrete, margenIzquierda, 10, 430, 35);
                        }
                    }

                    margenTop += 15;
                    funcionarioEjecutor = "Funcionario: ";
                }

                documento.Save(rutaDeDescarga);

                if (documento.PageCount > 0)
                {
                    rutaParaDescargarElReporte = new iRutaDeDescargaDelPDF() {
                        mensaje = string.Empty,
                        resultado = "1",
                        nombreArchivo = nombreArchivo
                    };
                } else
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

        private int _calculaAnchoColumnaSegunColumna(iColumnas columna)
        {
            int margen = 0;
            switch (columna.nombreColumna)
            {
                case "Cert.":
                    margen = 0;
                    break;

                case "Sector":
                    margen = 42;
                    break;

                case "Cliente":
                    margen = 80;
                    break;

                case "Emisión":
                    margen = 103;
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
    }
}
