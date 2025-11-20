using trifork.Models;
using trifork.Models.Input;

namespace trifork.Interfaces;

public interface IDataAccess
{
    Task<PlayerModel> CreatePlayer(CreatePlayerModel createPlayerModel);
    Task<PlayerModel?> UpdatePlayer(UpdatePlayerModel player);
    Task<PlayerModel?> DeletePlayer(Guid playerId);
    Task<IEnumerable<PlayerModel>> ListPlayers(int page, int pageSize);
    Task<IEnumerable<PlayerModel>> SearchPlayers(string searchString, int page, int pageSize);
    Task<MatchModel> CreateMatch(CreateMatchModel createMatchModel);
    Task<MatchModel?> UpdateMatch(UpdateMatchResultModel updateMatchResult);
    Task<MatchModel?> GetMatch(Guid matchId);
    Task<int> GetPlayerScore(Guid playerId);
}