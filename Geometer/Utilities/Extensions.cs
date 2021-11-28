using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Geometer.Utilities
{
  public static class GeometerExtensions
  {
    //This debounces an action by the number of milliseconds. Useful for delaying processing based on text updates
    public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
    {
      CancellationTokenSource cancelTokenSource = null;

      // return func;
      return arg =>
      {
        cancelTokenSource?.Cancel();
        cancelTokenSource = new CancellationTokenSource();

        Task.Delay(milliseconds, cancelTokenSource.Token)
          .ContinueWith(t =>
          {
            if (t.IsCompletedSuccessfully)
            {
              try
              {
                func(arg);
              }
              catch (Exception e)
              {
                Console.WriteLine("DEBOUNCE ERROR:::");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
              }
            }
          }, TaskScheduler.Default);
      };
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T element in source)
      {
        action(element);
      }
    }

    public static string Limit(this string str, int limit)
    {
      return str.Length > limit ? $"{str.Substring(0 , limit - 3)}..." : str;
    }

    public static string ToUpperCase(this string str)
    {
      return $"{str[0..1].ToUpperInvariant()}{str[1..].ToLowerInvariant()}";
    }
  }
}