using Core.Services.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace Infraestructure.Services
{
    public class PDFService : IPDFService
    {   
        
        public Task<string> ExtractPDF(Stream pdfStream)  //No requiere await ya que siguiendo el pipeline, el Controlador API es await para poder recibir el PDF, cuando invoca a ExtractPDF ya esta en memoria y listo para ser procesado de forma sincrona
        {
            try
            {
                if (pdfStream == null || pdfStream.Length == 0)
                {
                    throw new InvalidOperationException("No se pudo encontrar informacion en el archivo adjunto");
                }

                var TextBuilder = new StringBuilder(); //Herramienta para manipular cadenas string

                var pdfDocument = PdfDocument.Open(pdfStream); //Instanciamiento de clase PdfDocument y le enviamos como parámetro pdfStream. Internamente su constructor genera un new PdfDocument(...)
                {
                    foreach (var pages in pdfDocument.GetPages())
                    {
                        TextBuilder.AppendLine(pages.Text);
                    }
                }

                return Task.FromResult(TextBuilder.ToString()); //Promesa que me representa un proceso de forma sincrona, necesita ser procesada con await cuando sea invocada
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("El documento se encuentra corrupto", ex);
            }
        }

    }
}
