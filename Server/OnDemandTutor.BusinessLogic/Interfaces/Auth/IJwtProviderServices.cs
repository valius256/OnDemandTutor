namespace OnDemandTutor.BusinessLogic.Interfaces.Auth;

public interface IJwtProviderServices
{
    Task<string> GetForCredentialsAsync(string email, string password);
}