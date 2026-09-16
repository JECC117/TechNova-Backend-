using Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Imp
{
    public class DocumentAnalysis(IPDFService pDFService, IGeminiService geminiService) :IDocumentAnalysis
    {
            private readonly IPDFService _PdfService = pDFService;
            private readonly IGeminiService _GeminiService = geminiService;


            public async Task<string> AnalyzeDocumentAsync(Stream pdfStream, string UserQuestion)
            {
                var textoExtraidoPdf = await _PdfService.ExtractPDF(pdfStream);

                var PromptGemini = $"{UserQuestion}\n\nContexto del documento:\n{textoExtraidoPdf}";

                var respuestaGemini = await _GeminiService.AskAsync(PromptGemini);

                return respuestaGemini;
            }
        }
    }
