﻿using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System;
using WebWasmClient.Services.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebWasmClient.Services
{
    public class HttpClientService
    {
        private HttpClient httpClient;
        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("authorizedClient");
            httpClient.BaseAddress = new Uri(httpClient.BaseAddress.ToString());
        }
        public async Task<T> GetFromAPI<T>(string route)
        {
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("/api" + route);
            if(httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(await httpResponseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                catch(Exception ex)
                {

                }
            }
            if(httpResponseMessage.IsSuccessStatusCode is false)
            {
                ProblemDetails problemDetails = JsonSerializer.Deserialize<ProblemDetails>(await httpResponseMessage.Content.ReadAsStringAsync());
                throw new HttpClientServiceException(problemDetails.Detail);
            }
            return default;
        }
    }
}