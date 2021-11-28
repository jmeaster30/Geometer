using System.Collections.Generic;
using System.Linq;
using Geometer.Lib.Translator.AST;

namespace Geometer.Lib.Model
{
  public static class IModelExtenstions
  {
    public static IEnumerable<T> As<T>(this IEnumerable<Model> models) where T : class
    {
      return models.Select(x => x.As<T>()).Where(x => x != null);
    }

    public static IEnumerable<PointModel> PointModels(this IEnumerable<Model> models)
    {
      return models.Where(x => x.Type == ModelType.Point).Select(x => (PointModel)x);
    }

    public static IEnumerable<LineModel> LineModels(this IEnumerable<Model> models)
    {
      return models.Where(x => x.Type == ModelType.Line).Select(x => (LineModel)x);
    }

    public static IEnumerable<AngleModel> AngleModels(this IEnumerable<Model> models)
    {
      return models.Where(x => x.Type == ModelType.Angle).Select(x => (AngleModel)x);
    }

    public static IEnumerable<CircleModel> CircleModels(this IEnumerable<Model> models)
    {
      return models.Where(x => x.Type == ModelType.Circle).Select(x => (CircleModel)x);
    }

    public static IEnumerable<PolygonModel> PolygonModels(this IEnumerable<Model> models)
    {
      return models.Where(x => x.Type == ModelType.Polygon).Select(x => (PolygonModel)x);
    }

    public static bool MatchesASTType(this Model model, GeometerASTType type)
    {
      return model.Type switch
      {
        ModelType.Point => type == GeometerASTType.Point,
        ModelType.Line => type == GeometerASTType.Line,
        ModelType.Angle => type == GeometerASTType.Angle,
        ModelType.Circle => type == GeometerASTType.Circle,
        ModelType.Polygon => type == GeometerASTType.Polygon,
        _ => false
      };
    }
  }
}