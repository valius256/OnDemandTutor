using OnDemandTutor.DataAccess.Models;

namespace OnDemandTutor.BusinessLogic.Business.Interfaces
{
    public interface IJwtService
    {
        Task<JwtInfor?> DecodeTokenAsync(string jwt);
        Task<string> GenerateTokenAsync(User user);
        Task<string?> GetNextTokenAsync(Guid userId);
    }
}
