using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Pokemons.GetPokemon;
using Pokedex.Domain.Pokemons;
using System.ComponentModel.DataAnnotations;
namespace Pokedex.Api.Controllers;

[ApiController]
[Route("pokemon")]
public class PokemonController : ControllerBase
{
  private readonly ISender _sender;

  public PokemonController(ISender sender)
  {
    _sender = sender;
  }

  [HttpGet("{name}")]
  public async Task<ActionResult<Pokemon>> GetByName(
    [Required(ErrorMessage = "Pokemon name cannot be null or white space")] string name,
    CancellationToken cancellationToken)
  {
    var query = new GetPokemonQuery(new Name(name));

    var response = await _sender.Send(query, cancellationToken);

    return response.HasValue ? response.Value : this.NotFound();
  }
}
