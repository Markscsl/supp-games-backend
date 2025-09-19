using SuppGamesBack.Models;

namespace SuppGamesBack.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
