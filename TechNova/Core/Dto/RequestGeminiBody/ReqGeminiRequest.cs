using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.RequestGeminiBody
{
    public class ReqGeminiRequest
    {
        public List<ReqGeminiContent> contents { get; set; } = new List<ReqGeminiContent>(); //Composicion: Lista que solo admite objetos instaciados de la clase Gemini Part
    }
}
