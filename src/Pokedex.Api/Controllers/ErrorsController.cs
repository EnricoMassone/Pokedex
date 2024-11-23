using Microsoft.AspNetCore.Mvc;

namespace Pokedex.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class ErrorsController : ControllerBase
{
  [Route("/error")]
  public IActionResult HandleError() =>
    Problem();
}
