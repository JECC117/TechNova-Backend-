using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IDocumentAnalysis
    {
        Task<string> AnalyzeDocumentAsync(Stream pdfStream, string UserQuestion); 
    }
}
