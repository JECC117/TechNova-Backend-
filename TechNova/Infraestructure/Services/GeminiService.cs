using Core.Services.Interfaces;
using System.Net.Http.Json;
using Core.Dto.RequestGeminiBody;
using Core.Dto.ResponseGeminiBody;

namespace Infraestructure.Services
{
    public class GeminiService: IGeminiService
    {
        private readonly HttpClient _httpClient1;
        public GeminiService(HttpClient httpClient) { 
        
            _httpClient1 = httpClient;
        }



        public async Task<string> AskAsync(string prompt)
        {

            var Payload = new ReqGeminiRequest() //Construcción del Payload siguiendo la estructura que solicita gemini
            {
                contents= new List<ReqGeminiContent>
                    {
                        new ReqGeminiContent()
                        {
                            parts= new List<ReqGeminiPart>
                            {
                                new ReqGeminiPart()
                                {
                                    text= prompt
                                }
                            }
                        }
                    }
            };
            
            //Creamos la petición mediante PostAsJsonAsync (await porque es una request), pero manejamos HttpResponseMessage porque queremos el contenido de response
            using HttpResponseMessage response = await _httpClient1.PostAsJsonAsync(
                "v1beta/models/gemini-2.5-flash:generateContent", Payload);

            //Nos aseguramos que la request haya sido exitosa
            response.EnsureSuccessStatusCode();

            var geminiResponse= await response.Content.ReadFromJsonAsync<ResGeminiResponse>();

                if (geminiResponse is null || geminiResponse.candidates.Count == 0)
                {

                throw new InvalidOperationException("La respuesta no ha podido ser procesada");

                }

            return geminiResponse.candidates[0].content.parts[0].text;

        }

    }
}
