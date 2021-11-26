using System.Collections.Generic;
using System.Data;
using System.Numerics;

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

  public class GeometerAST
  {
    public GeometerASTType Type { get; } = GeometerASTType.None;
  }

  public class Alias : GeometerAST
  {
    public string Name { get; set; }
  }

  public abstract class SrcLine : GeometerAST {}
  public abstract class Constraint : GeometerAST {}
  public abstract class Expression : GeometerAST {}
  public abstract class ObjectRef : Expression {}
  public abstract class NumRef : ObjectRef {}

  public class Root : GeometerAST
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Root;
    public IEnumerable<SrcLine> Lines { get; set; }
  }

  public class Query : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Query;
  }

  public class ModelDef : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Model;
    public ObjectRef ObjectRef { get; set; }
    public Constraint Constraint { get; set; }
  }

  public class Import : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Import;
    public string Filename { get; set; }
  }

  public class Save : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Save;
    public string Filename { get; set; }
  }

  public class Clear : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Clear;
  }

  public class Reset : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Reset;
  }

  public class Undo : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Undo;
    public BigInteger Amount { get; set; }
  }

  public class Redo : SrcLine
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Redo;
    public BigInteger Amount { get; set; }
  }

  public class OnShape : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.OnShape;
    public ObjectRef ObjectRef { get; set; }
  }

  public class PerpBis : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.PerpBis;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Bisect : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Bisect;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Tangent : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Tangent;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Similarity : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Similarity;
    public ObjectRef ObjectRef { get; set; }
    public bool Congruent { get; set; }
  }

  public class ExprConstraint : Constraint
  {
    public new GeometerASTType Type { get; } = GeometerASTType.ExprConst;
    public Operator Operator { get; set; }
    public Expression Expression { get; set; }
  }

  public class Id : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Id;
    public List<string> Chain { get; set; } = new List<string>();
  }

  public class Point : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Point;
    public Id Id { get; set; }
    public string Alias { get; set; }
  }

  public class Line : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Line;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
  }

  public class Circle : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Circle;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
  }

  public class Angle : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Angle;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
  }

  public class Polygon : ObjectRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Polygon;
    public List<Id> Ids { get; set; } = new List<Id>();
    public string Alias { get; set; }
  }

  public class Length : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Length;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Size : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Size;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Area : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Area;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Perimeter : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Perimeter;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Circumference : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Circumference;
    public ObjectRef ObjectRef { get; set; }
  }

  public class Pi : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Pi;
  }

  public class Tau : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Tau;
  }

  public class Number : NumRef
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Number;
    public BigInteger Value { get; set; }
  }

  public class BinaryOp : Expression
  {
    public new GeometerASTType Type { get; } = GeometerASTType.Expression;
    public Operator Operator { get; set; }
    public Expression LeftHandSide { get; set; }
    public Expression RightHandSide { get; set; }
  }

}