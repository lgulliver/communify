using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Communify.Communities.Common
{
    public static class ValidationExtensions
    {
        public static async Task<ValidatedModel<TModel>> GetJsonBody<TModel, TValidator>(this HttpRequest request)
            where TValidator : AbstractValidator<TModel>, new()
        {
            var model = await GetJsonBody<TModel>(request);
            var validator = new TValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return new ValidatedModel<TModel>()
                {
                    Value = model,
                    IsValid = false,
                    ValidationFailures = validationResult.Errors
                        .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
                        .ToList()
                };
            }

            return new ValidatedModel<TModel>
            {
                Value = model,
                IsValid = true
            };
        }

        public static async Task<T> GetJsonBody<T>(this HttpRequest request)
        {
            var requestBody = await ReadUnseekableStreamAsStringAsync(request);

            return JsonConvert.DeserializeObject<T>(requestBody);
        }

        private const int DefaultBufferSize = 1024;

      
        // This has been lifted from the Functions SDK and tweaked as using .NET Core 3.1 caused an incompatibility
        private static async Task<string> ReadUnseekableStreamAsStringAsync(this HttpRequest request)
        {
            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: DefaultBufferSize,
                leaveOpen: true);
            var buffer = new Memory<char>(new char[DefaultBufferSize]);
            await reader.ReadAsync(buffer);
            return buffer.ToString().Trim();
        }

        public static BadRequestObjectResult ToBadRequestResult<T>(this ValidatedModel<T> request, string propertyName = null, string error = null)
        {


            if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrEmpty(error))
            {
                request.ValidationFailures.Add(new ValidationError(propertyName, error));
            }

            return new BadRequestObjectResult(request.ValidationFailures);
        }
    
    }
}