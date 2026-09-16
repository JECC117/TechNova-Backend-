using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ImagenResponseDto
{
    public class ImagenResponseDto
    {
        public string Base64data { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
    }
}
