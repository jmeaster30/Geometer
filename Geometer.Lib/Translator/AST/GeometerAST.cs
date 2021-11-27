using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace Geometer.Lib.Translator.AST
{
  public enum GeometerASTType
  {
    None, Root, Query, Model, Expression,
    Import, Save, Clear, Reset, Undo, Redo,
    OnShape, PerpBis, Bisect, Tangent, Similarity, ExprConst,
    Id, Point, Line, Circle, Polygon, Angle,
    Length, Size, Area, Perimeter, Circumference,
    Pi, Tau, Number
  }

  public enum Operator
  {
    Equal, Less, Greater, NotEqual, LessEqual, GreaterEqual,
    Plus, Minus, Multiply, Divide, Exponent
  }

  public abstract class GeometerAST
  {
    public abstract GeometerASTType Type { get; }
    public abstract void Print();
  }

  public class Alias : GeometerAST
  {
    public override GeometerASTType Type { get; } = GeometerASTType.None;
    public string Name { get; set; }
    public override void Print()
    {
      Console.Write($"{{Alias : '{Name}'}}");
    }
  }

  public abstract class SrcLine : GeometerAST { }
  public abstract class Constraint : GeometerAST { }
  public abstract class Expression : GeometerAST { }
  public abstract class ObjectRef : Expression { }
  public abstract class NumRef : ObjectRef { }

  public class Root : GeometerAST
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Root;
    public IEnumerable<SrcLine> Lines { get; set; }
    public override void Print()
    {
      Console.Write("{Root: ");
      foreach (SrcLine line in Lines)
      {
        line.Print();
        Console.Write(" ");
      }
      Console.Write("}");
    }
  }

  public class Query : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Query;
    public override void Print()
    {
      Console.Write("{Query}");
    }
  }

  public class ModelDef : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Model;
    public ObjectRef ObjectRef { get; set; }
    public Constraint Constraint { get; set; }

    public override void Print()
    {
      Console.Write("{ModelDef}");
    }
  }

  public class Import : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Import;
    public string Filename { get; set; }
    public override void Print()
    {
      Console.Write("{Import}");
    }
  }

  public class Save : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Save;
    public string Filename { get; set; }
    public override void Print()
    {
      Console.Write("{Save}");
    }
  }

  public class Clear : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Clear;
    public override void Print()
    {
      Console.Write("{Clear}");
    }
  }

  public class Reset : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Reset;
    public override void Print()
    {
      Console.Write("{Reset}");
    }
  }

  public class Undo : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Undo;
    public BigInteger Amount { get; set; }
    public override void Print()
    {
      Console.Write("{Undo}");
    }
  }

  public class Redo : SrcLine
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Redo;
    public BigInteger Amount { get; set; }
    public override void Print()
    {
      Console.Write("{Redo}");
    }
  }

  public class OnShape : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.OnShape;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{On}");
    }
  }

  public class PerpBis : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.PerpBis;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Perp}");
    }
  }

  public class Bisect : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Bisect;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Bis}");
    }
  }

  public class Tangent : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Tangent;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Tan}");
    }
  }

  public class Similarity : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Similarity;
    public ObjectRef ObjectRef { get; set; }
    public bool Congruent { get; set; }
    public override void Print()
    {
      Console.Write("{Sim}");
    }
  }

  public class ExprConstraint : Constraint
  {
    public override GeometerASTType Type { get; } = GeometerASTType.ExprConst;
    public Operator Operator { get; set; }
    public Expression Expression { get; set; }
    public override void Print()
    {
      Console.Write("{Expr}");
    }
  }

  public class Id : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Id;
    public List<string> Chain { get; set; } = new List<string>();
    public override void Print()
    {
      Console.Write("{Id}");
    }
  }

  public class Point : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Point;
    public Id Id { get; set; }
    public string Alias { get; set; }
    public override void Print()
    {
      Console.Write("{Point}");
    }
  }

  public class Line : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Line;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
    public override void Print()
    {
      Console.Write("{Line}");
    }
  }

  public class Circle : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Circle;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
    public override void Print()
    {
      Console.Write("{Circle}");
    }
  }

  public class Angle : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Angle;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
    public override void Print()
    {
      Console.Write("{Angle}");
    }
  }

  public class Polygon : ObjectRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Polygon;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
    public override void Print()
    {
      Console.Write("{Poly}");
    }
  }

  public class Length : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Length;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Length}");
    }
  }

  public class Size : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Size;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Size}");
    }
  }

  public class Area : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Area;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Area}");
    }
  }

  public class Perimeter : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Perimeter;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Peri}");
    }
  }

  public class Circumference : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Circumference;
    public ObjectRef ObjectRef { get; set; }
    public override void Print()
    {
      Console.Write("{Circum}");
    }
  }

  public class Pi : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Pi;
    public override void Print()
    {
      Console.Write("{Pi}");
    }
  }

  public class Tau : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Tau;
    public override void Print()
    {
      Console.Write("{Tau}");
    }
  }

  public class Number : NumRef
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Number;
    public BigInteger Value { get; set; }
    public override void Print()
    {
      Console.Write("{Number}");
    }
  }

  public class BinaryOp : Expression
  {
    public override GeometerASTType Type { get; } = GeometerASTType.Expression;
    public Operator Operator { get; set; }
    public Expression LeftHandSide { get; set; }
    public Expression RightHandSide { get; set; }
    public override void Print()
    {
      Console.Write("{BINOP}");
    }
  }

}