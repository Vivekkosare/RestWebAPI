using RestWebAPI.Entities;
using RestWebAPI.Extensions;

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
                    return Result<Phone>.Failure("Failed to add phone");
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HttpException occured while adding phone");
                return Result<Phone>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while adding phone");
                return Result<Phone>.Failure(ex.Message);
            }
        }

        public async Task<object> DeletePhone(int id)
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
                var error = $"Failed to delete phone with Id: {id}, Error: {errorContent}, ErrorCode: {response.StatusCode}";

                _logger.LogWarning(error);
                return Result<object>.Failure(error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occured while deleting phone with id: {id}");
                return Result<object>.Failure(ex.Message);
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
                    return Result<Phone>.Failure(warning);
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while getting phone");
                return Result<Phone>.Failure(ex.Message);
            }
        }

        public async Task<Result<List<Phone>>> GetPhonesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Result<List<Phone>>.Failure("Could not retrieve the list of phones");
                }
                return Result<List<Phone>>.Success(content.GetDeserializedObject<List<Phone>>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching phones");
                return Result<List<Phone>>.Failure(ex.Message);
            }
        }
    }
}
