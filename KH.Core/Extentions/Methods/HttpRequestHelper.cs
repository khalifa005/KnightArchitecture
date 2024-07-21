using KH.Helper.Responses;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Mime;

namespace KH.Helper.Helpers
{
    public class HttpRequestHelper
    {
        private readonly HttpClient _http;
        private readonly ILogger<HttpRequestHelper> _logger;
        public HttpRequestHelper(HttpClient http, ILogger<HttpRequestHelper> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<Result<T>> GetRequestAsync<T>(string Endpoint, string token, bool returnedAsArrayByte = false)
        {
            HttpRequestMessage httpRequestMessage = new();

            httpRequestMessage.Method = new HttpMethod(HttpMethod.Get.ToString());

            httpRequestMessage.RequestUri = new Uri(Endpoint);

            //httpRequestMessage.Content.Headers.ContentType
            //    = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeHeaderTypes.ApplicationJson);
            if (!string.IsNullOrEmpty(token))
            {
                httpRequestMessage.Headers.Authorization
                   = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var result = await _http.SendAsync(httpRequestMessage); 

            dynamic content = null;

            if (returnedAsArrayByte)
                content = await result.Content.ReadAsByteArrayAsync();
            else
                content = await result.Content.ReadAsStringAsync();


            _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {content}");

            if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
               result.StatusCode == System.Net.HttpStatusCode.BadRequest ||
               result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.Log(LogLevel.Error, "httprequest error occurred while calling post Endpoint: {@Endpoint}  response  : {@result}", Endpoint, result);
                System.Console.WriteLine(content);
                return Result<T>.False(content);
            }

            //if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    return Result<T>.False(content);
            //}
            //else if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            //{
            //    System.Console.WriteLine(content);
            //    return Result<T>.False(content);
            //}
            //else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    System.Console.WriteLine(content);
            //    return Result<T>.False(content);
            //}

            try
            {
                dynamic res = null;
                if (returnedAsArrayByte)
                    res = content;
                else
                    res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);

                return Result<T>.True(res);
            }
            catch (System.Exception e)
            {
                _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject : {@Endpoint} response : {@content} result : {@result}");
                System.Console.WriteLine(e.Message + e.StackTrace);
                System.Console.WriteLine(content);
                return Result<T>.False(content);
            }
        }

        public async Task<Result<T>> GetRequestParamAsync<T>(string Endpoint, string token, Dictionary<string, string> queryStringParams)
        {
            var url = QueryHelpers.AddQueryString(Endpoint, queryStringParams);

            return await GetRequestAsync<T>(url, token);
        }

        public async Task<Result<T>> PostRequestAsync<T>(string Endpoint, object content, string token)
        {
            HttpRequestMessage httpRequestMessage = new();

            httpRequestMessage.Method = new HttpMethod(HttpMethod.Post.ToString());

            httpRequestMessage.RequestUri = new Uri(Endpoint);

            var serializedObject = JsonConvert.SerializeObject(content);

            httpRequestMessage.Content = new StringContent(serializedObject);


            httpRequestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);


            if (!string.IsNullOrEmpty(token))
            {
                httpRequestMessage.Headers.Authorization
               = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var result = await _http.SendAsync(httpRequestMessage);


            var responde = await result.Content.ReadAsStringAsync();

            _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {responde}");

            if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
                result.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.Log(LogLevel.Error, $"httprequest error occurred while calling post Endpoint: {Endpoint} body : {content} response : {responde} result : {result}");
                return Result<T>.False(responde);
            }

            try
            {
                var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responde);
                //var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responde);
                return Result<T>.True(res);
            }
            catch (System.Exception e)
            {
                _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject result: {Endpoint} body : {content} response : {responde} result : {result}");
                _logger.Log(LogLevel.Error, $"the error: {e}");
                System.Console.WriteLine(e.Message + e.StackTrace);
                System.Console.WriteLine(responde);
                return Result<T>.False(responde);
            }
        }


        public async Task<Result<T>> PostRequestAsyncForm<T>(string Endpoint, HttpModelDTO content, string token, bool isAttachments = false)
        {
            HttpRequestMessage httpRequestMessage = new();

            httpRequestMessage.Method = new HttpMethod(HttpMethod.Post.ToString());

            httpRequestMessage.RequestUri = new Uri(Endpoint);

            if (isAttachments)
            {
                var multipartContent = new MultipartFormDataContent();

                multipartContent.Add(new StringContent(JsonConvert.SerializeObject(content.Data)), "data");

                foreach (var item in content.NewFiles)
                {
                    multipartContent.Add(new StreamContent(item.File.OpenReadStream()), item.NameOfFile, item.File.FileName);
                }

                httpRequestMessage.Content = multipartContent;
            }
            else
            {
                var serializedObject = JsonConvert.SerializeObject(content.Data);

                httpRequestMessage.Content = new StringContent(serializedObject);

                httpRequestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Json);
            }

            if (!string.IsNullOrEmpty(token))
            {
                httpRequestMessage.Headers.Authorization
               = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var result = await _http.SendAsync(httpRequestMessage);

            var responde = await result.Content.ReadAsStringAsync();

            _logger.Log(LogLevel.Information, $"calling {Endpoint} status code {result.StatusCode} response as string : {responde}");

            if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
                result.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.Log(LogLevel.Error, "httprequest error occurred while calling post Endpoint: {@Endpoint} body : {@content} response : {@responde} result : {@result}", Endpoint, content, responde, result);
                System.Console.WriteLine(responde);
                return Result<T>.False(responde);
            }

            try
            {
                var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responde);
                //var res = Newtonsoft.Json.JsonConvert.DeserializeObject<Result<T>>(responde);
                return Result<T>.True(res);
            }
            catch (System.Exception e)
            {
                _logger.Log(LogLevel.Error, $"httprequest error occurred while DeserializeObject result: {@Endpoint} body : {@content} response : {@responde} result : {@result}");
                System.Console.WriteLine(e.Message + e.StackTrace);
                System.Console.WriteLine(responde);
                return Result<T>.False(responde);
            }
        }


        public Dictionary<string, IFormFile> PrepareProperties<T>(Type classType, T model)
        {
            Dictionary<string, IFormFile> propertiesAsDictionary = new Dictionary<string, IFormFile>();

            var properties = classType.GetProperties();

            foreach (PropertyInfo info in properties)
            {
                var x = info.GetValue(model, null);

                if (x is IFormFile)
                {
                    IFormFile xx = x as IFormFile;

                    propertiesAsDictionary.Add(info.Name, xx);
                }

            }
            return propertiesAsDictionary;

        }
    }

}
