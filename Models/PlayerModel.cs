using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace trifork.Models;

public class PlayerModel
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Initials { get; set; }
    public int Handicap { get; init; } = 10;
    [JsonIgnore]
    public DateTimeOffset? DeletedOn { get; set; }
    [JsonIgnore]
    public Guid? MatchId { get; set; }
    [JsonIgnore]
    public MatchModel? Match { get; set; }
}