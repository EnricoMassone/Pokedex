namespace Pokedex.Framework.Logging;

// see https://blog.stephencleary.com/2020/06/a-new-pattern-for-exception-logging.html
public static class LoggingHelpers
{
  public static bool True(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    action();

    return true;
  }

  public static bool False(Action action)
  {
    ArgumentNullException.ThrowIfNull(action);

    action();

    return false;
  }
}
