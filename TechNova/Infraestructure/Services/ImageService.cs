using Core.Dto.ImagenResponseDto;
using Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Services
{
    public class ImageService : IimageService
    {
        public Task<ImagenResponseDto> ExtractTextFromImage(Stream imageStream, string FileImageName)
        {
            try
            {
                if(imageStream == null || imageStream.Length == 0)
                {
                    throw new InvalidOperationException("La imagen no posee informacion");
                }

                var NewMemoryStream= new MemoryStream();
                imageStream.CopyTo(NewMemoryStream);
                byte[] ArrayBytesImage = NewMemoryStream.ToArray();

                //Conversion a base64
                var base64data = Convert.ToBase64String(ArrayBytesImage);
                var mimeType= Path.GetExtension(FileImageName).ToLowerInvariant();

                var ImagenResponse = new ImagenResponseDto
                {
                    Base64data= base64data,
                    MimeType= mimeType,
                };

                return Task.FromResult(ImagenResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inesperado:", ex);
            }
        }
    }
}
