using System;
using Geometer.Lib.Model;

namespace Geometer.Drawing
{
  public static class GeometerDrawingExtensions
  {
    public static void Draw(this PointModel point)
    {
      Console.WriteLine("Draw point - " + point.Name);
    }
  }
}