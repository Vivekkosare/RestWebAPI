using RestWebAPI.Entities;

namespace RestWebAPI.Services
{
    public interface IMockApiService
    {
        Task<Result<List<Phone>>> GetPhonesAsync(int? page = 1, int? pageSize = 10,
            string? name = null);
        Task<Result<Phone>> GetPhoneAsync(int id);
        Task<Result<Phone>> AddPhoneAsync(Phone phone);
        Task<Result<object>> DeletePhone(int id);

    }
}
