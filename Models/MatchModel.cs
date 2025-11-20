using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace trifork.Models;

public class MatchModel
{
    [Key]
    public Guid Id { get; init; }
    public Guid Team1Player1Id { get; set; }
    [JsonIgnore]
    public PlayerModel? Team1Player1 { get; init; }
    public Guid Team1Player2Id { get; set; }
    [JsonIgnore]
    public PlayerModel? Team1Player2 { get; init; }
    public Guid Team2Player1Id { get; set; }
    [JsonIgnore]
    public PlayerModel? Team2Player1 { get; init; }
    public Guid Team2Player2Id { get; set; }
    [JsonIgnore]
    public PlayerModel? Team2Player2 { get; init; }
    public WinningTeamEnum WinningTeam { get; set; } = WinningTeamEnum.Tbd;

    [JsonIgnore]
    public ICollection<PlayerModel> WinningPlayers => WinningTeam switch
    {
        WinningTeamEnum.Team1 => new [] { Team1Player1, Team1Player2 },
        WinningTeamEnum.Team2 => new  [] { Team2Player1, Team2Player2 },
        _ => new List<PlayerModel>()
    };
}