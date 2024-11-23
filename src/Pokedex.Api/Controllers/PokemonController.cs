using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Domain.Pokemons;
using System.ComponentModel.DataAnnotations;
namespace Pokedex.Api.Controllers;

[ApiController]
[Route("pokemon")]
[Produces("application/json")]
public class PokemonController : ControllerBase
{
  private readonly ISender _sender;

  public PokemonController(ISender sender)
  {
    _sender = sender ?? throw new ArgumentNullException(nameof(sender));
  }

  /// <summary>
  /// Gets a Pokemon by name
  /// </summary>
  /// <param name="name">The name of the Pokemon to fetch</param>
  /// <param name="cancellationToken">Framework provided <see cref="CancellationToken"/> object</param>
  /// <returns>Information about the fetched Pokemon</returns>
  /// <response code="200">Returns an object containing information about the fetched Pokemon</response>
  /// <response code="404">If the Pokemon being fetched by name does not exist</response>
  /// <response code="400">If the provided Pokemon name is null or white space</response>
  [HttpGet("{name}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<GetPokemonQueryResponse>> GetByName(
    [Required(ErrorMessage = "Pokemon name cannot be null or white space")] string name,
    CancellationToken cancellationToken)
  {
    var query = new GetPokemonQuery(new Name(name));

    var response = await _sender.Send(query, cancellationToken);

    return response.HasValue ? response.Value : this.NotFound();
  }
}
