using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ResponseGeminiBody
{
    public class ResGeminiContent
    {
        public List<ResGeminiPart> parts {  get; set; }= new List<ResGeminiPart>();
    }
}
