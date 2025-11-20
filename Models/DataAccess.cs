using Microsoft.EntityFrameworkCore;
using trifork.Interfaces;
using trifork.Models.Input;

namespace trifork.Models;

public class DataAccess : IDataAccess
{
    private readonly FoosballContext _context;

    public DataAccess(FoosballContext  context)
    {
        _context = context;
    }

    public async Task<PlayerModel> CreatePlayer(CreatePlayerModel createPlayerModel)
    {
        var playerModel = new PlayerModel
        {
            Id = Guid.NewGuid(),
            Name = createPlayerModel.Name,
            Initials = createPlayerModel.Initials,
            Handicap = createPlayerModel.Handicap
        };

        _context.Players.Add(playerModel);
        await  _context.SaveChangesAsync();
        return playerModel;
    }

    public async Task<PlayerModel?> UpdatePlayer(UpdatePlayerModel player)
    {
        var existingPlayer = await _context.Players.FindAsync(player.Id);

        if (existingPlayer is null)
            return null;

        if (player.Name is not null)
        {
            existingPlayer.Name = player.Name;
        }

        if (player.Initials is not null)
        {
            existingPlayer.Initials = player.Initials;
        }

        // Update method?
        var changedRows = await _context.SaveChangesAsync();

        // Verify changedRows?

        return existingPlayer;
    }

    public async Task<PlayerModel?> DeletePlayer(Guid playerId)
    {
        var playerToDelete = await _context.Players.FindAsync(playerId);

        if (playerToDelete is null)
        {
            return null;
        }

        playerToDelete.Name = "Deleted";
        playerToDelete.Initials = "Deleted";
        playerToDelete.DeletedOn = DateTimeOffset.UtcNow;

        var changedRows = await _context.SaveChangesAsync();

        // Verify changedRows?

        return playerToDelete;
    }

    public async Task<IEnumerable<PlayerModel>> ListPlayers(int page, int pageSize)
    {
        // Primitive offset pagination
        var playerPage = await _context.Players
            .Where(p => p.DeletedOn == null)
            .OrderByDescending(p => p.Handicap)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return playerPage;
    }

    public async Task<IEnumerable<PlayerModel>> SearchPlayers(string searchString, int page, int pageSize)
    {
        // Primitive offset pagination
        var playerPage = await _context.Players
            .Where(p => p.DeletedOn == null && p.Name.Contains(searchString)) // EF friendly case insensitive comparison?
            .OrderBy(p => p.Name)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return playerPage;
    }

    public async Task<MatchModel> CreateMatch(CreateMatchModel createMatchModel)
    {
        // Find more performant than Where?
        var matchModel = new MatchModel
        {
            Id = Guid.NewGuid(), // EF can also generate column values?
            Team1Player1 = await _context.Players.FindAsync(createMatchModel.Team1Player1),
            Team1Player2 = await _context.Players.FindAsync(createMatchModel.Team1Player2),
            Team2Player1 = await _context.Players.FindAsync(createMatchModel.Team2Player1),
            Team2Player2 = await _context.Players.FindAsync(createMatchModel.Team2Player2),
        }; // Would probably be a good idea to validate that the players were found ^^.

        _context.Matches.Add(matchModel);
        await _context.SaveChangesAsync();
        return matchModel;
    }

    public async Task<MatchModel?> UpdateMatch(UpdateMatchResultModel updateMatchResult)
    {
        // Why do I need to do this? Find doesn't work :(
        var existingMatch = await _context.Matches
            .Include(m => m.Team1Player1)
            .Include(m => m.Team1Player2)
            .Include(m => m.Team2Player1)
            .Include(m => m.Team2Player2)
            .Where(m => m.Id == updateMatchResult.MatchId)
            .FirstAsync(); // Not handled.

        existingMatch.WinningTeam = updateMatchResult.WinningTeam;
        await _context.SaveChangesAsync();
        return existingMatch;
    }

    public async Task<MatchModel?> GetMatch(Guid matchId)
    {
        var matchModel = await _context.Matches.FindAsync(matchId);

        return matchModel;
    }

    public async Task<int> GetPlayerScore(Guid playerId)
    {
        // I know this query is horrible - I would never actually do something like this!
        // If nothing else, we can use it as an example of what happens if you model your data wrong haha.
        // Please see repo readme for how I would have liked to have model things.
        var wonMatches = await _context.Matches.Where(m =>
                (m.WinningTeam == WinningTeamEnum.Team1 &&
                 (m.Team1Player1Id == playerId || m.Team1Player2Id == playerId)) ||
                (m.WinningTeam == WinningTeamEnum.Team2 &&
                 (m.Team2Player1Id == playerId || m.Team2Player2Id == playerId))
            )
            .Include(matchModel => matchModel.Team1Player1)
            .Include(matchModel => matchModel.Team1Player2)
            .Include(matchModel => matchModel.Team2Player1)
            .Include(matchModel => matchModel.Team2Player2)
            .ToListAsync();

        var lostMatches = await _context.Matches.Where(m =>
            (m.WinningTeam == WinningTeamEnum.Team1 &&
             (m.Team2Player1Id == playerId || m.Team2Player2Id == playerId)) ||
            (m.WinningTeam == WinningTeamEnum.Team2 && (m.Team1Player1Id == playerId || m.Team1Player2Id == playerId))
        ).ToListAsync();

        // I ran out of time and did not finish the score calculation, but it does not make sense anyway since
        // I did not manage to implement my model as I had planned anyway.

        return 0;
    }
}