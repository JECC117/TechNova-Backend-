using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ResponseGeminiBody
{
    public class ResGeminiResponse
    {
        public List<ResGeminiCandidate> candidates { get; set; } = new List<ResGeminiCandidate>(); 
    }
}
