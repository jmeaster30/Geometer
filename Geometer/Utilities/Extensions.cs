using System;
using System.Threading;
using System.Threading.Tasks;
using Gtk;

namespace Geometer.Utilities
{
  public static class GeometerExtensions
  {
    //This debounces an action by the number of milliseconds. Useful for delaying processing based on text updates
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
      CancellationTokenSource cancelTokenSource = null;

      return arg =>
      {
        cancelTokenSource?.Cancel();
        cancelTokenSource = new CancellationTokenSource();

        Task.Delay(milliseconds, cancelTokenSource.Token)
          .ContinueWith(t =>
          {
            if (t.IsCompletedSuccessfully)
            {
              func(arg);
            }
          }, TaskScheduler.Default);
      };
    }

    public static string Limit(this string str, int limit)
    {
      return str.Length > limit ? $"{str.Substring(0 , limit - 3)}..." : str;
    }
  }
}