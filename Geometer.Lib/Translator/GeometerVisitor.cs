using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Globalization;
using Geometer.Lib.Translator.AST;
using System;
using System.ComponentModel.Design.Serialization;

namespace Geometer.Lib.Translator
{
  public class GeometerVisitor : GeometerBaseVisitor<GeometerAST>
  {
    public override GeometerAST VisitStart(GeometerParser.StartContext context)
    {
      var lines = new List<SrcLine>();

      foreach (var line in context.line())
      {
        lines.Add((SrcLine)Visit(line)); // Will always be a line
      }

      return new Root { Lines = lines };
    }

    public override GeometerAST VisitLine(GeometerParser.LineContext context)
    {
      if (context.QUERY() == null)
      {
        return new ModelDef
        {
          ObjectRef = (ObjectRef)Visit(context.objref()),
          Constraint = context.constraint() != null ? (Constraint)Visit(context.constraint()) : null
        };
      }
      else if (context.IMPORT() != null)
      {
        return new Import { Filename = context.FILE().GetText() };
      }
      else if (context.SAVE() != null)
      {
        return new Save { Filename = context.FILE() == null ? "" : context.FILE().GetText() };
      }
      else if (context.CLEAR() != null)
      {
        return new Clear();
      }
      else if (context.RESET() != null)
      {
        return new Reset();
      }
      else if (context.UNDO() != null)
      {
        return new Undo { Amount = context.NUMBER() == null ? 1 : BigInteger.Parse(context.NUMBER().GetText(), NumberStyles.Any, new GeometerFormatProvider()) };
      }
      else if (context.REDO() != null)
      {
        return new Redo { Amount = context.NUMBER() == null ? 1 : BigInteger.Parse(context.NUMBER().GetText(), NumberStyles.Any, new GeometerFormatProvider()) };
      }
      else
      {
        return new Query { };
      }
    }

    public override GeometerAST VisitOnshape(GeometerParser.OnshapeContext context)
    {
      ObjectRef objref = null;
      if (context.lineref() != null)
      {
        objref = (ObjectRef)Visit(context.lineref());
      }
      else if (context.circref() != null)
      {
        objref = (ObjectRef)Visit(context.circref());
      }
      else if (context.polyref() != null)
      {
        objref = (ObjectRef)Visit(context.polyref());
      }
      return new OnShape() { ObjectRef = objref };
    }

    public override GeometerAST VisitPerpbis(GeometerParser.PerpbisContext context)
    {
      return new PerpBis() { ObjectRef = (ObjectRef)Visit(context.lineref()) };
    }

    public override GeometerAST VisitBisect(GeometerParser.BisectContext context)
    {
      ObjectRef objref = null;
      if (context.lineref() != null)
      {
        objref = (ObjectRef)Visit(context.lineref());
      }
      else if (context.angleref() != null)
      {
        objref = (ObjectRef)Visit(context.angleref());
      }
      return new Bisect() { ObjectRef = objref };
    }

    public override GeometerAST VisitTangent(GeometerParser.TangentContext context)
    {
      return new Tangent() { ObjectRef = (ObjectRef)Visit(context.circref()) };
    }

    public override GeometerAST VisitConstraint(GeometerParser.ConstraintContext context)
    {
      if (context.IS() != null)
      {
        if (context.onshape() != null)
        {
          return Visit(context.onshape());
        }
        else if (context.perpbis() != null)
        {
          return Visit(context.perpbis());
        }
        else if (context.bisect() != null)
        {
          return Visit(context.bisect());
        }
        else if (context.tangent() != null)
        {
          return Visit(context.tangent());
        }
      }
      else if (context.SQUIGGLE() != null)
      {
        return new Similarity() { ObjectRef = (ObjectRef)Visit(context.objref()), Congruent = context.EQUAL() != null };
      }
      else
      {
        Operator op;
        if (context.LESS() != null)
        {
          op = context.EQUAL() == null ? Operator.Less : Operator.LessEqual;
        }
        else if (context.GREATER() != null)
        {
          op = context.EQUAL() == null ? Operator.Greater : Operator.GreaterEqual;
        }
        else //equal must be not null here
        {
          op = context.NOT() == null ? Operator.Equal : Operator.NotEqual;
        }
        return new ExprConstraint() { Expression = (Expression)Visit(context.expression()), Operator = op };
      }
      return null;
    }

    public override GeometerAST VisitIdchain(GeometerParser.IdchainContext context)
    {
      Id id = new();
      id.Chain.AddRange(context.IDENTIFIER().Select(x => x.GetText()));
      return id;
    }

    public override GeometerAST VisitPointref(GeometerParser.PointrefContext context)
    {
      return new Point()
      {
        Id = (Id)Visit(context.idchain()),
        Alias = (context.secname() == null ? null : (Alias)Visit(context.secname()))?.Name
      };
    }

    public override GeometerAST VisitLineref(GeometerParser.LinerefContext context)
    {
      var line = new Line() { Alias = (context.secname() == null ? null : (Alias)Visit(context.secname()))?.Name };

      foreach (var id in context.idchain())
      {
        line.Ids.Add((Id)Visit(id));
      }

      return line;
    }

    public override GeometerAST VisitCircref(GeometerParser.CircrefContext context)
    {
      var circle = new Circle() { Alias = (context.secname() == null ? null : (Alias)Visit(context.secname()))?.Name };

      foreach (var id in context.idchain())
      {
        circle.Ids.Add((Id)Visit(id));
      }

      return circle;
    }

    public override GeometerAST VisitAngleref(GeometerParser.AnglerefContext context)
    {
      var angle = new Angle() { Alias = (context.secname() == null ? null : (Alias)Visit(context.secname()))?.Name };

      foreach (var id in context.idchain())
      {
        angle.Ids.Add((Id)Visit(id));
      }

      return angle;
    }

    public override GeometerAST VisitPolyref(GeometerParser.PolyrefContext context)
    {
      var polygon = new Polygon() { Alias = (context.secname() == null ? null : (Alias)Visit(context.secname()))?.Name };

      foreach (var id in context.idchain())
      {
        polygon.Ids.Add((Id)Visit(id));
      }

      return polygon;
    }

    public override GeometerAST VisitLengthref(GeometerParser.LengthrefContext context)
    {
      return new Length { ObjectRef = (ObjectRef)Visit(context.lineref()) };
    }

    public override GeometerAST VisitSizeref(GeometerParser.SizerefContext context)
    {
      return new Size { ObjectRef = (ObjectRef)Visit(context.angleref()) };
    }

    public override GeometerAST VisitArearef(GeometerParser.ArearefContext context)
    {
      var objref = context.circref() == null ? (ObjectRef)Visit(context.polyref()) : (ObjectRef)Visit(context.circref());
      return new Area { ObjectRef = objref };
    }

    public override GeometerAST VisitPeriref(GeometerParser.PerirefContext context)
    {
      return new Perimeter { ObjectRef = (ObjectRef)Visit(context.polyref()) };
    }

    public override GeometerAST VisitCircumref(GeometerParser.CircumrefContext context)
    {
      return new Circumference { ObjectRef = (ObjectRef)Visit(context.circref()) };
    }

    public override GeometerAST VisitSecname(GeometerParser.SecnameContext context)
    {
      return new Alias { Name = context.IDENTIFIER().GetText() };
    }

    public override GeometerAST VisitExpression(GeometerParser.ExpressionContext context)
    {
      if (context.ADD() == null && context.MINUS() == null)
      {
        return Visit(context.multexpression());
      }

      return new BinaryOp
      {
        Operator = context.ADD() == null ? Operator.Minus : Operator.Plus,
        LeftHandSide = (Expression)Visit(context.expression()),
        RightHandSide = (Expression)Visit(context.multexpression())
      };
    }

    public override GeometerAST VisitMultexpression(GeometerParser.MultexpressionContext context)
    {
      if (context.MULT() == null && context.DIV() == null)
      {
        return Visit(context.expexpression());
      }

      return new BinaryOp
      {
        Operator = context.MULT() == null ? Operator.Divide : Operator.Multiply,
        LeftHandSide = (Expression)Visit(context.multexpression()),
        RightHandSide = (Expression)Visit(context.expexpression())
      };
    }

    public override GeometerAST VisitExpexpression(GeometerParser.ExpexpressionContext context)
    {
      if (context.EXP() == null)
      {
        return Visit(context.term());
      }

      return new BinaryOp
      {
        Operator = Operator.Exponent,
        LeftHandSide = (Expression)Visit(context.term()),
        RightHandSide = (Expression)Visit(context.expexpression())
      };
    }

    public override GeometerAST VisitTerm(GeometerParser.TermContext context)
    {
      if (context.NUMBER() != null)
      {
        Console.WriteLine(context.NUMBER().GetText());
        return new Number
        {
          Value = BigInteger.Parse(context.NUMBER().GetText(), NumberStyles.Any, new GeometerFormatProvider())
        };
      }
      else if (context.PI() != null)
      {
        return new Pi();
      }
      else if (context.TAU() != null)
      {
        return new Tau();
      }
      else if (context.numref() != null)
      {
        return Visit(context.numref());
      }
      else if (context.expression() != null)
      {
        return Visit(context.expression());
      }
      return null; // should not be hit
    }

    public override GeometerAST VisitNumref(GeometerParser.NumrefContext context)
    {
      if (context.lengthref() != null)
      {
        return Visit(context.lengthref());
      }
      else if (context.sizeref() != null)
      {
        return Visit(context.sizeref());
      }
      else if (context.arearef() != null)
      {
        return Visit(context.arearef());
      }
      else if (context.periref() != null)
      {
        return Visit(context.periref());
      }
      else if (context.circumref() != null)
      {
        return Visit(context.circumref());
      }
      return null; //should not be hit
    }

    public override GeometerAST VisitObjref(GeometerParser.ObjrefContext context)
    {
      if (context.idchain() != null)
      {
        return Visit(context.idchain());
      }
      else if (context.pointref() != null)
      {
        return Visit(context.pointref());
      }
      else if (context.lineref() != null)
      {
        return Visit(context.lineref());
      }
      else if (context.circref() != null)
      {
        return Visit(context.circref());
      }
      else if (context.angleref() != null)
      {
        return Visit(context.angleref());
      }
      else if (context.polyref() != null)
      {
        return Visit(context.polyref());
      }
      else if (context.numref() != null)
      {
        return Visit(context.numref());
      }
      return null; //should not be hit
    }
  }
}