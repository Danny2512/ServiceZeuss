using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceZeuss.Data.Entities;
using ServiceZeuss.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ServiceZeuss.Services.Zeuss
{
    public class ZeussService : IZeussService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string apiUrl;
        private readonly string apiUser;
        private readonly string apiPassword;

        public ZeussService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            apiUrl = environment.IsDevelopment()
            ? _configuration["Zeuss:ApiUrl:dev"]
            : _configuration["Zeuss:ApiUrl:prod"];
            apiUser = environment.IsDevelopment()
                ? _configuration["Zeuss:ApiCredentials:dev:User"]
                : _configuration["Zeuss:ApiCredentials:prod:User"];
            apiPassword = environment.IsDevelopment()
                ? _configuration["Zeuss:ApiCredentials:dev:Password"]
                : _configuration["Zeuss:ApiCredentials:prod:Password"];
        }

        public async Task<List<CategoryApiResponseModel>> GetCategories(string? filter)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();

                // Obtener el token de autenticación
                string token = await Authenticate();

                // Incluir el token en el encabezado de la solicitud
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsync($"{apiUrl}/Category/GetCategories?filter={filter}", null);
                response.EnsureSuccessStatusCode();

                var categoriesResponse = await response.Content.ReadAsStringAsync();

                // Deserializar directamente el array de categorías
                var result = JsonConvert.DeserializeObject<ApiResponseGetCategoriesModel>(categoriesResponse)?.Categories;

                return result ?? new List<CategoryApiResponseModel>();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error de red al obtener categorías: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener categorías: {ex.Message}");
            }
        }

        public async Task<bool> CreateCategory(SynchronizeExternalServiceZeussCategoryModel model)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();

                // Obtener el token de autenticación
                string token = await Authenticate();

                // Incluir el token en el encabezado de la solicitud
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serializar el modelo a JSON
                var jsonModel = JsonConvert.SerializeObject(model);

                // Crear el contenido de la solicitud con el modelo serializado
                var content = new StringContent(jsonModel, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST al endpoint correspondiente
                var response = await client.PostAsync($"{apiUrl}/Category/Create", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error de red al obtener categorías: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al sincronizar categoría con el servicio exeno de zeuss: {ex.Message}");
            }
        }

        private async Task<string> Authenticate()
        {
            try
            {
                string user = apiUser;
                string password = apiPassword;

                using var client = _httpClientFactory.CreateClient();

                var requestContent = new StringContent(
                    JsonConvert.SerializeObject(new { user = user, password = password }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync($"{apiUrl}/Auth/GetToken", requestContent);

                response.EnsureSuccessStatusCode();

                var tokenResponse = await response.Content.ReadAsStringAsync();

                var tokenResult = JsonConvert.DeserializeAnonymousType(tokenResponse, new { token = "" });

                if (tokenResult?.token != null)
                {
                    return tokenResult.token;
                }
                else
                {
                    throw new AuthenticationFailedException("El token no se encontró en la respuesta.");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new AuthenticationFailedException("Error de red al autenticar con el servicio externo de Zeuss", ex);
            }
            catch (Exception ex)
            {
                throw new AuthenticationFailedException("Error en la autenticación con el servicio de Zeuss: " + ex.Message, ex);
            }
        }
    }
}
