namespace Core.Services.Interfaces
{
    public interface IPDFService
    {
        //Uso de Stream que representa conjunto de datos independiente del tipo de archivo
        //Metodo async declarado con Task ya que es una Promesa para obtener la info desde Front
        Task<string> ExtractPDF(Stream pdfStream);
    }
}
