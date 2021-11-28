using System;
using System.Collections.Generic;

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

  public interface IModel
  {
    public T As<T>() where T : class;
    public ModelType Type { get; set; }
    public List<string> Name { get; set; }
    public string Alias { get; set; }
    public List<ConstraintModel> Constraints { get; set; }
  }

  public abstract class Model<T> : IModel
  {
    public abstract T GetConcrete();
    public U As<U>() where U : class => typeof(U) == typeof(T) ? (U)(object)this : null;

    public abstract ModelType Type { get; set; }
    public List<string> Name { get; set; }
    public string Alias { get; set; }
    public List<ConstraintModel> Constraints { get; set; }
  }

  public class PointModel : Model<PointModel>
  {
    public override ModelType Type { get; set; } = ModelType.Point;
    public override PointModel GetConcrete() => this;
    public Position Position { get; set; }
  }

  public class LineModel : Model<LineModel>
  {
    public override ModelType Type { get; set; } = ModelType.Line;
    public override LineModel GetConcrete() => this;
    public PointModel Start { get; set; }
    public PointModel End { get; set; }
  }

  public class AngleModel : Model<AngleModel>
  {
    public override ModelType Type { get; set; } = ModelType.Angle;
    public override AngleModel GetConcrete() => this;
    public PointModel Left { get; set; }
    public PointModel Center { get; set; }
    public PointModel Right { get; set; }
  }

  public class PolygonModel : Model<PolygonModel>
  {
    public override ModelType Type { get; set; } = ModelType.Polygon;
    public override PolygonModel GetConcrete() => this;
    public List<PointModel> Points { get; set; } = new List<PointModel>(); //should be at least three
  }

  public class CircleModel : Model<CircleModel>
  {
    public override ModelType Type { get; set; } = ModelType.Circle;
    public override CircleModel GetConcrete() => this;
    public PointModel Center { get; set; }
    public PointModel RadiusPoint { get; set; }
  }
}