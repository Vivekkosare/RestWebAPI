using FluentValidation;
using RestWebAPI.Entities;
using RestWebAPI.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RestWebAPI.Services
{
    public class MockApiService: IMockApiService
        
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MockApiService> _logger;
        private readonly IValidator<Phone> _validator;
        private readonly IValidator<PhoneInput> _validatorPhoneInput;
        private readonly IConfiguration _configuration;
         private static string _baseUrl = string.Empty;
        public MockApiService(IHttpClientFactory httpClientFactory,
            ILogger<MockApiService> logger,
            IValidator<Phone> validator,
            IValidator<PhoneInput> validatorPhoneInput,
            IConfiguration configuration) 
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _validator = validator;
            _validatorPhoneInput = validatorPhoneInput;
            _configuration = configuration;
            _baseUrl = _configuration["baseUrl"];
        }
        public async Task<Result<Phone>> AddPhoneAsync(PhoneInput phone)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var validationResult = await _validatorPhoneInput.ValidateAsync(phone);
                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning(errorMessage);
                    return Result<Phone>.Failure(errorMessage, HttpStatusCode.BadRequest);
                }

                //Get list of available phones
                var phones = await GetPhonesAsync();

                if (phones.IsSuccess && phones.Value.Any(p => p.Name == phone.Name))
                {
                    var errorMessage = $"Phone with the name {phone.Name} already exists";
                    _logger.LogWarning(errorMessage);
                    return Result<Phone>.Failure(errorMessage, HttpStatusCode.Conflict);
                }

                var response = await client.PostAsJsonAsync(_baseUrl, phone);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    var warning = "Failed to add phone, no content returned";
                    _logger.LogWarning(warning);
                    return Result<Phone>.Failure(warning, HttpStatusCode.BadRequest);
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


        public async Task<Result<Phone>> UpdatePhoneAsync(Phone updatedPhone)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {               
                // Validate the phone object before sending it to the API
                var validationResult = await _validator.ValidateAsync(updatedPhone);
                if (!validationResult.IsValid)
                {
                    var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning(errorMessage);
                    return Result<Phone>.Failure(errorMessage, HttpStatusCode.BadRequest);
                }

                // Check if the phone with the given id exists
                var existingPhone = await GetPhoneAsync(updatedPhone.Id);
                if (!existingPhone.IsSuccess)
                {
                    return existingPhone;
                }

                // Update the phone object with the new values
                var phoneToUpdate = existingPhone.Value.MapPhone(updatedPhone);
                var response = await client.PutAsJsonAsync($"{_baseUrl}/{phoneToUpdate.Id}", phoneToUpdate);
                
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    var warning = "Failed to update phone, no content returned";
                    _logger.LogWarning(warning);
                    return Result<Phone>.Failure(warning, HttpStatusCode.BadRequest);
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HttpException occured while updating phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while updating phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }

        }

        public async Task<Result<object>> DeletePhone(string id)
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
                var error = $"Failed to delete phone with Id: {id}, Error: {errorContent.Result}, " +
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

        public async Task<Result<Phone>> GetPhoneAsync(string id)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                
                // Check if the content is null or empty
                // and return a failure result if it is
                if (string.IsNullOrWhiteSpace(content))
                {
                    var warning = $"Could not find the phone with Id: {id}";
                    _logger.LogWarning(warning);
                    return Result<Phone>.Failure(warning, HttpStatusCode.NotFound);
                }
                return Result<Phone>.Success(content.GetDeserializedObject<Phone>());
            }
            catch (HttpRequestException ex)
            {
                string message = $"Could not find phone with Id: {id}";
                _logger.LogError(ex, message);
                return Result<Phone>.Failure(message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something happened while getting phone");
                return Result<Phone>.Failure(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Result<List<Phone>>> GetPhonesAsync(int page = 1, int pageSize = 100, 
            string name = null)
        {
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Check if the content is null or empty
                // and return a failure result if it is
                if (string.IsNullOrWhiteSpace(content))
                {
                    string warning = "Could not retrieve the list of phones";
                    _logger.LogWarning(warning);
                    return Result<List<Phone>>.Failure(warning, HttpStatusCode.NotFound);
                }

                // Deserialize the content to a list of Phone objects
                // and filter by name if provided
                var phones = content.GetDeserializedObject<List<Phone>>();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    phones = phones.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Paginate the list of phones
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
