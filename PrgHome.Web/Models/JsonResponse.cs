using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrgHome.Web.Models
{
    public class JsonResponse
    {
        public JsonResponse()
        {

        }
        public JsonResponse(bool success)
        {
            Success = success;
        }
        public JsonResponse(bool success,string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        public JsonResponse(bool success,object data)
        {
            Success = success;
            Data = data;
        }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
}
