using System;

namespace TokaApi.Models
{
    public class GenericResponseModel
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public object? Data { get; set; }

        public GenericResponseModel() { }

        public GenericResponseModel(bool success, string? message, object? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
