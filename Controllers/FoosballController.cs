using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using trifork.Interfaces;
using trifork.Models;
using trifork.Models.Input;

namespace trifork.Controllers;

[ApiController]
[Route("[controller]")]
public class FoosballController : ControllerBase
{
    private readonly IDataAccess _dataAccess;
    private readonly ILogger<FoosballController> _logger;

    public FoosballController(IDataAccess dataAccess, ILogger<FoosballController> logger)
    {
        _dataAccess = dataAccess;
        _logger = logger;
    }

    [HttpPost]
    [Route("players")]
    [ProducesResponseType(StatusCodes.Status200OK)] // More of this but out of time.
    public async Task<IResult> CreatePlayer([FromBody] CreatePlayerModel player, [FromServices] IValidator<CreatePlayerModel> validator)
    {
        ValidationResult result = await validator.ValidateAsync(player);

        if (!result.IsValid)
        {
            return CreateBadRequestResponse(result.Errors);
        }

        PlayerModel createdPlayerModel = await _dataAccess.CreatePlayer(player);

        return Results.Ok(createdPlayerModel);
    }

    [HttpPatch]
    [Route("players/{playerId:guid}")]
    public async Task<IResult> UpdatePlayer(Guid playerId, [FromBody] UpdatePlayerModel player, [FromServices] IValidator<UpdatePlayerModel> validator)
    {
        var playerWithId = player with { Id = playerId };
        ValidationResult result = await validator.ValidateAsync(playerWithId);

        if (!result.IsValid)
        {
            return CreateBadRequestResponse(result.Errors);
        }

        PlayerModel? updatedPlayerModel = await _dataAccess.UpdatePlayer(playerWithId);

        return Results.Ok(updatedPlayerModel);
    }

    [HttpDelete]
    [Route("players/{playerId:guid}")]
    public async Task<IResult> DeletePlayer(Guid playerId)
    {
        // Check how guid validation happens here to make sure validation error response is consistent in format.

        var deletedPlayerModel = await _dataAccess.DeletePlayer(playerId);

        return Results.Ok(deletedPlayerModel);
    }
    
    
    [HttpGet]
    [Route("players/{playerId:guid}/score")]
    public async Task<IResult> GetPlayer(Guid playerId)
    {
        var playerScore = await _dataAccess.GetPlayerScore(playerId);

        return Results.Ok(playerScore);
    }

    [HttpGet]
    [Route("players")]
    [Route("players/{page:int}")]
    [Route("players/{page:int}/{pageSize:int}")]
    public async Task<IEnumerable<PlayerModel>> GetPlayerList(int page = 0, int pageSize = 20)
    {
        var playerList = await _dataAccess.ListPlayers(page, pageSize);

        return playerList;
    }

    [HttpGet]
    [Route("players/search/{searchString}")]
    [Route("players/search/{searchString}/{page:int}")]
    [Route("players/search/{searchString}/{page:int}/{pageSize:int}")]
    public async Task<IEnumerable<PlayerModel>> SearchPlayers(string searchString, int page = 0, int pageSize = 20)
    {
        var playerList = await _dataAccess.SearchPlayers(searchString, page, pageSize);

        return playerList;
    }

    [HttpPost]
    [Route("match")]
    public async Task<IResult> CreateMatch([FromBody] CreateMatchModel createMatchModel, [FromServices] IValidator<CreateMatchModel> validator)
    {
        ValidationResult result = await validator.ValidateAsync(createMatchModel);

        if (!result.IsValid)
        {
            return CreateBadRequestResponse(result.Errors);
        }

        var createdMatchModel = await _dataAccess.CreateMatch(createMatchModel);

        return Results.Ok(createdMatchModel);
    }

    [HttpPatch]
    [Route("match/{matchId:guid}")]
    public async Task<IResult> UpdateMatch(Guid matchId, [FromBody] UpdateMatchResultModel updateMatchResult, [FromServices] IValidator<UpdateMatchResultModel> validator)
    {
        var matchResultWithId = updateMatchResult with { MatchId = matchId };
        ValidationResult result = await validator.ValidateAsync(matchResultWithId);

        if (!result.IsValid)
        {
            return CreateBadRequestResponse(result.Errors);
        }

        var updatedMatchModel = await _dataAccess.UpdateMatch(matchResultWithId);
        
        return Results.Ok(updatedMatchModel);
    }

    [HttpGet]
    [Route("match/{matchId:guid}")]
    public async Task<IResult> GetMatch(Guid matchId, [FromServices] IValidator<CreateMatchModel> validator)
    {
        var matchModel = await _dataAccess.GetMatch(matchId);

        return Results.Ok(matchModel);
    }

    private IResult CreateBadRequestResponse(List<ValidationFailure> errors) =>
        Results.Problem
        (
            type: "Bad Request",
            title: "Validation error",
            statusCode: StatusCodes.Status400BadRequest,
            detail: string.Join(". ", errors)
        );
}
