using System;
using Geometer.Lib.Model;

namespace Geometer.Drawing
{
  public static class GeometerDrawingExtensions
  {
    public static void Draw(this GeoModel geomodel, GeometerDisplay display)
    {
      Console.WriteLine("Draw Model");
      foreach (Model model in geomodel.Models)
      {
        switch (model.Type)
        {
          case ModelType.Point: model.As<PointModel>().Draw(display); break;
          case ModelType.Line: model.As<LineModel>().Draw(display); break;
          case ModelType.Angle: model.As<AngleModel>().Draw(display); break;
          case ModelType.Circle: model.As<CircleModel>().Draw(display); break;
          case ModelType.Polygon: model.As<PolygonModel>().Draw(display); break;
          default: break;
        }
      }
    }

    public static void Draw(this PointModel point, GeometerDisplay display)
    {
      Console.WriteLine($"Draw point - {string.Join('~', point.Name)}");
    }

    public static void Draw(this LineModel line, GeometerDisplay display)
    {
      Console.WriteLine($"Draw line - {string.Join('~', line.Name)}");
    }

    public static void Draw(this AngleModel angle, GeometerDisplay display)
    {
      Console.WriteLine($"Draw angle - {string.Join('~', angle.Name)}");
    }

    public static void Draw(this CircleModel circle, GeometerDisplay display)
    {
      Console.WriteLine($"Draw circle - {string.Join('~', circle.Name)}");
    }

    public static void Draw(this PolygonModel poly, GeometerDisplay display)
    {
      Console.WriteLine($"Draw poly - {string.Join('~', poly.Name)}");
    }
  }
}