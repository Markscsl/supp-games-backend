using SuppGamesBack.RawgApi.Models;

namespace SuppGamesBack.Services
{
    public interface IRawgClient
    {
        Task<RawgSearchResponse> SearchGamesAsync(string query);

    }
}
