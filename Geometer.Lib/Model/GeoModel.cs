using System;
using System.Collections.Generic;
using System.Linq;
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
    List<Model> Models { get; set; }

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

        }
      }

      Console.WriteLine(result.Message);

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

    //TODO need to update a specific point and everything that has dependencies on that point
    //TODO queries should be split out from the models and draw extra things to show certain values

    public void Draw()
    {
      foreach (Model model in Models)
      {
        //model.Draw();
      }
    }

  }
}