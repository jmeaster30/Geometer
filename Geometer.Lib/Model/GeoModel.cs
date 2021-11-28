using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Geometer.Lib.Translator.AST;

namespace Geometer.Lib.Model
{
  public class GeoUpdateResult
  {
    public bool Successful { get; set; }
    public bool Updated { get; set; }
    public int ErrorStart { get; set; }
    public int ErrorStop { get; set; }
    public int ErrorLine { get; set; }
    public int ErrorPos { get; set; }
    public string Message { get; set; }
  }

  public class GeoModel
  {
    public List<IModel> Models { get; set; } = new();

    public GeoModel() { }

    public GeoUpdateResult Update(string source)
    {
      bool updated = false;
      Console.WriteLine("Updating Model....");

      //parse
      ExecResult result = Geo.Execute(source, false);

      //if successful then update
      if (result.Ast != null)
      {
        //loop through AST models
        //try to update the existing model
        Root root = (Root)result.Ast;
        root.Print();
        Console.WriteLine("");

        List<ModelDef> model_defs = root.Lines.Where(x => x.Type == GeometerASTType.Model).Select(x => (ModelDef)x).ToList();
        foreach (ModelDef model_def in model_defs)
        {
          List<PointModel> new_points = CreatePointModels(model_def.ObjectRef);
          if (new_points.Any())
          {
            Models.AddRange(new_points);
            updated = true;
          }

          IModel created_model = CreateFinalModel(model_def);
          if (created_model != null)
          {
            Models.Add(created_model);
            updated = true;
          }
        }

        //TODO remove models that do not exist anymore
        RemoveOldModels(model_defs);
      }

      Console.WriteLine("Current Models");
      foreach (IModel model in Models)
      {
        Console.WriteLine($"{model.Type} - {string.Join('~', model.Name)}");
      }

      return new GeoUpdateResult()
      {
        Successful = result.Ast != null,
        Updated = updated,
        Message = result.Message,
        ErrorStart = result.ErrorStart,
        ErrorStop = result.ErrorStop,
        ErrorLine = result.ErrorLine,
        ErrorPos = result.ErrorPosition,
      };
    }

    private List<PointModel> CreatePointModels(ObjectRef objectRef)
    {
      List<PointModel> result = new();
      List<PointModel> existing_points = Models.PointModels().ToList();
      List<string> to_add = new();

      switch (objectRef.Type)
      {
        case GeometerASTType.Point:
          Point point = (Point)objectRef;
          if (!existing_points.Any(x => x.Name.Contains(point.Id.Chain.FirstOrDefault())))
          {
            PointModel newPoint = new()
            {
              Name = point.Id.Chain,
              Alias = "", // TODO ?
              Constraints = new() // TODO ?
            };
            result.Add(newPoint);
          }
          break;
        case GeometerASTType.Line:
        case GeometerASTType.Angle:
        case GeometerASTType.Circle:
        case GeometerASTType.Polygon:
          to_add = objectRef.Id.Chain.Where(x => !existing_points.Any(y => y.Name.FirstOrDefault() == x)).ToList();
          break;
        default:
          break;
      }

      foreach (string newPointName in to_add)
      {
        PointModel newPoint = new()
        {
          Name = new() { newPointName },
          Alias = "", //No alias
          Constraints = new() //No constraints
        };
        result.Add(newPoint);
      }

      return result;
    }

    private IModel CreateFinalModel(ModelDef modelDef)
    {
      List<PointModel> point_models = Models.PointModels().Where(x => modelDef.ObjectRef.Id.Chain.Contains(x.Name.FirstOrDefault())).ToList();
      List<string> name = modelDef.ObjectRef.Id.Chain;
      string name_sig = string.Join('~', name);

      IModel result = null;

      if (!Models.Any(x => string.Join('~', x.Name) == name_sig && x.MatchesASTType(modelDef.ObjectRef.Type)))
      {
        Console.WriteLine("Model not found");
        switch (modelDef.ObjectRef.Type)
        {
          case GeometerASTType.Point:
            //we have already added this case in "CreatePointModels"
            break;
          case GeometerASTType.Line:
            if (point_models.Count != 2) { throw new ArgumentException($"Incorrect number of points given for a line {point_models.Count} (Something serious is wrong)"); }
            result = new LineModel
            {
              Name = name,
              Alias = "", // TODO should we get rid of the aliases?
              Constraints = new(), // TODO
              Start = point_models[0],
              End = point_models[1]
            };
            break;
          case GeometerASTType.Angle:
            if (point_models.Count != 3) { throw new ArgumentException($"Incorrect number of points given for an angle {point_models.Count} (Something serious is wrong)"); }
            result = new AngleModel
            {
              Name = name,
              Alias = "", // TODO should we get rid of the aliases?
              Constraints = new(), // TODO
              Left = point_models[0],
              Center = point_models[1],
              Right = point_models[2]
            };
            break;
          case GeometerASTType.Circle:
            if (point_models.Count is not 1 and not 2) { throw new ArgumentException($"Incorrect number of points given for a circle {point_models.Count} (Something serious is wrong)"); }
            result = new CircleModel
            {
              Name = name,
              Alias = "", // TODO should we get rid of the aliases?
              Constraints = new(), // TODO
              Center = point_models[0],
              RadiusPoint = point_models.Count == 2 ? point_models[1] : null, // TODO figure this out
            };
            break;
          case GeometerASTType.Polygon:
            if (point_models.Count < 3) { throw new ArgumentException($"Incorrect number of points given for a polygon {point_models.Count} (Something serious is wrong)"); }
            result = new PolygonModel
            {
              Name = name,
              Alias = "", // TODO
              Constraints = new(), // TODO
              Points = point_models
            };
            break;
          default:
            //this is something we can't create a model out of
            break;
        }
      }

      return result;
    }

    private void RemoveOldModels(List<ModelDef> modelDefs)
    {
      /* 
      TODO : Enhance this by getting rid of models where the points still remain but the model itself is removed
      Example :
        point A
        point B
        line AB <- if this gets removed from the source the model still remains since the points still exist in the source
       */
      List<PointModel> existing_points = Models.PointModels().ToList();

      foreach (PointModel current_point in existing_points)
      {
        if (!modelDefs.SelectMany(x => x.ObjectRef.Id.Chain).Any(x => current_point.Name.FirstOrDefault() == x))
        {
          Models.RemoveAll(x => x.Name.Contains(current_point.Name.FirstOrDefault()));
        }
      }
    }

    //TODO need to update a specific point and everything that has dependencies on that point
    //TODO queries should be split out from the models and draw extra things to show certain values
  }
}