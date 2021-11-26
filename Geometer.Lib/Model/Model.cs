
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
  public abstract class Model
  {
    public ModelType Type { get; set; }
    public string Name { get; set; }
    public string Alias { get; set; }
    public List<ConstraintModel> Constraints { get; set; }

    public abstract void Draw();
  }
}