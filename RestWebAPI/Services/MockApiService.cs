﻿using RestWebAPI.Entities;
using RestWebAPI.Extensions;
using System.Net;

namespace RestWebAPI.Services
{
    public class MockApiService(IHttpClientFactory _httpClientFactory,
        ILogger<MockApiService> _logger) : IMockApiService
    {
        private static string _baseUrl = "https://api.restful-api.dev/objects";
        public async Task<Result<Phone>> AddPhoneAsync(Phone phone)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.PostAsJsonAsync(_baseUrl, phone);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Result<Phone>.Failure("Failed to add phone", HttpStatusCode.BadRequest);
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HttpException occured while adding phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while adding phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<object>> DeletePhone(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.DeleteAsync($"{_baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Result<object>.Success();
                }

                var errorContent = response.Content.ReadAsStringAsync();
                var error = $"Failed to delete phone with Id: {id}, Error: {errorContent}, " +
                    $"ErrorCode: {response.StatusCode}";

                _logger.LogWarning(error);
                return Result<object>.Failure(error, response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occured while deleting phone with id: {id}");
                return Result<object>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<Phone>> GetPhoneAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                {
                    var warning = $"Could not find the phone with Id: {id}";
                    _logger.LogWarning(warning);
                    return Result<Phone>.Failure(warning, HttpStatusCode.NotFound);
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while getting phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<List<Phone>>> GetPhonesAsync(int page = 1, int pageSize = 10, 
            string name = null)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Result<List<Phone>>.Failure("Could not retrieve the list of phones",
                    HttpStatusCode.NotFound);
                }
                var phones = (content.GetDeserializedObject<List<Phone>>());
                if (!string.IsNullOrWhiteSpace(name))
                {
                    phones = phones.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                phones = phones
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList();
                return Result<List<Phone>>.Success(phones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching phones");
                return Result<List<Phone>>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
