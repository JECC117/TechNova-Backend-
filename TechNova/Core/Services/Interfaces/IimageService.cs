using Core.Dto.ImagenResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IimageService
    {
        Task<ImagenResponseDto> ExtractTextFromImage(Stream imageStream, string FileImageName);
    }
}
