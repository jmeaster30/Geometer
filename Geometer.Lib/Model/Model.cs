using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Geometer.Lib.Model
{
  public enum ModelType
  {
    Point,
    Line,
    Circle,
    Polygon,
    Angle
  }

  public class Position
  {
    public double X { get; set; }
    public double Y { get; set; }
  }

  public abstract class Model
  {
    public abstract ModelType Type { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public List<ConstraintModel> Constraints { get; set; }
  }

  public class PointModel : Model
  {
    public override ModelType Type { get; set; } = ModelType.Point;
    public Position Position { get; set; }
  }

  public class LineModel : Model
  {
    public override ModelType Type { get; set; } = ModelType.Line;
    public PointModel Start { get; set; }
    public PointModel End { get; set; }
  }

  public class AngleModel : Model
  {
    public override ModelType Type { get; set; } = ModelType.Angle;
    public PointModel Left { get; set; }
    public PointModel Center { get; set; }
    public PointModel Right { get; set; }
  }

  public class PolygonModel : Model
  {
    public override ModelType Type { get; set; } = ModelType.Polygon;
    public List<PointModel> Points { get; set; } = new List<PointModel>(); //should be at least three
  }

  public class CircleModel : Model
  {
    public override ModelType Type { get; set; } = ModelType.Circle;
    public PointModel Center { get; set; }
    public PointModel RadiusPoint { get; set; }
  }
}