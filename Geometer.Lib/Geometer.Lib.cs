using System;
using Antlr4.Runtime;
using Geometer.Lib.Translator;
using Geometer.Lib.Model;
using Geometer.Lib.Translator.AST;

namespace Geometer.Lib
{
  public class ExecResult
  {
    public GeometerAST Ast { get; set; }
    public string Message { get; set; }
    public int ErrorStart { get; set; } = -1;
    public int ErrorStop { get; set; } = -1;
    public int ErrorLine { get; set; } = -1;
    public int ErrorPosition { get; set; } = -1;
  }

  public class Geo
  {
    public GeoModel Model { get; set; }

    public Geo()
    {
      Model = new GeoModel();
    }

    public static ExecResult Execute(string code, bool replMode)
    {
      try
      {
        GeometerLexer lexer = new(new AntlrInputStream(code));
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new ErrorListener { ReplMode = replMode });

        CommonTokenStream tokens = new(lexer);

        GeometerParser parser = new(tokens);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ErrorListener { ReplMode = replMode });

        GeometerVisitor visitor = new();

        GeometerAST ast = visitor.Visit(parser.start());

        return new()
        {
          Ast = ast,
          Message = "no errors :)"
        };
      }
      catch (SourceErrorException ex)
      {
        //pull out info from the exception message
        return new()
        {
          Ast = null,
          ErrorStart = ex.StartIndex,
          ErrorStop = ex.StopIndex,
          ErrorLine = ex.Line,
          ErrorPosition = ex.PositionInLine,
          Message = ex.Message
        };
      }
    }
  }
}
