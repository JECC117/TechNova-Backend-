using Microsoft.AspNetCore.Mvc;
using Core.Services.Interfaces;
using Core.CustomEntities;
using System.Net;

namespace Api.Controllers
{
    [Route("api/process")]
    [ApiController]
    public class DocumentController(IDocumentAnalysis documentAnalysis) : ControllerBase
    {
        private readonly IDocumentAnalysis _documentAnalysis= documentAnalysis;


        [HttpPost("Analysis")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DocumentProcessing([FromForm] IFormFile pdfstream, [FromForm] string UserQuestion)
        {

            var pdfStream = pdfstream.OpenReadStream();
           var GeminiResponse= await _documentAnalysis.AnalyzeDocumentAsync(pdfStream, UserQuestion);

            var response = new Response()
            {
                Status= (int)HttpStatusCode.OK,
                Message= "El proceso ha sido exitoso",
                Description= GeminiResponse
            };

            return StatusCode((int)HttpStatusCode.OK, response);

        }
    }
}
