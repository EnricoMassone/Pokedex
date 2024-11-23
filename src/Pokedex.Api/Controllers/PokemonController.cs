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
    _sender = sender;
  }

  /// <summary>
  /// Gets a Pokemon by name
  /// </summary>
  /// <param name="name">The name of the Pokemon to fetch</param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
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
