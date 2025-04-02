using RestWebAPI.Entities;

namespace RestWebAPI.Services
{
    public interface IMockApiService
    {
        Task<Result<List<Phone>>> GetPhonesAsync();
        Task<Result<Phone>> GetPhoneAsync(int id);
        Task<Result<Phone>> AddPhoneAsync(Phone phone);
        Task<object> DeletePhone(int id);

    }
}
