using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.RequestGeminiBody
{
    public class ReqGeminiContent
    {
        public List<ReqGeminiPart> parts { get; set; } = new List<ReqGeminiPart>();
    }
}
