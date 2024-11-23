namespace Pokedex.Infrastructure.UnitTests;

internal static class Helpers
{
  private const string TestApiResponsesFolderName = "TestApiResponses";

  public static string ReadTestApiResponseFile(string fileName)
  {
    var filePath = Path.Combine(TestApiResponsesFolderName, fileName);
    return File.ReadAllText(filePath);
  }
}
