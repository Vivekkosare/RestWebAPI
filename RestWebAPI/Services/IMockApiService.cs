using RestWebAPI.Entities;

namespace RestWebAPI.Services
{
    public interface IMockApiService
    {
        Task<Result<List<Phone>>> GetPhonesAsync(int page = 1, int pageSize = 10,
            string name = null);
        Task<Result<Phone>> GetPhoneAsync(string id);
        Task<Result<Phone>> AddPhoneAsync(PhoneInput phone);
        Task<Result<object>> DeletePhone(string id);
        Task<Result<Phone>> UpdatePhoneAsync(Phone updatedPhone);

    }
}
