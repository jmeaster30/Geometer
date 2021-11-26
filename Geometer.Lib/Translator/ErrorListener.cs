using System;
using Antlr4.Runtime;

namespace Geometer.Lib.Translator
{
  public class SourceErrorException : Exception
  {
    public int StartIndex { get; set; } = -1;
    public int StopIndex { get; set; } = -1;
    public int Line { get; set; } = -1;
    public int PositionInLine { get; set; } = -1;
    public SourceErrorException(string message) : base(message) { }
  }

  public class ErrorListener : BaseErrorListener, IAntlrErrorListener<int>
  {
    public bool ReplMode { get; set; }

    public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
      string message = "";

      if (ReplMode)
      {
        for (int i = 0; i <= offendingSymbol.StopIndex; i++)
        {
          if (i == offendingSymbol.StartIndex)
          {
            message += '^';
          }
          else if (i > offendingSymbol.StartIndex)
          {
            message += '~';
          }
          else
          {
            message += ' ';
          }
        }
        message += $"\nUnexpected Token Error: {msg}";
      }
      else
      {
        message += $"Unexpected Token Error: '{offendingSymbol.Text}' in line {offendingSymbol.Line} from {offendingSymbol.StartIndex + 1} to {offendingSymbol.StopIndex + 1}.\n";
        message += $"\t{msg}";
      }

      throw new SourceErrorException(message)
      {
        StartIndex = offendingSymbol.StartIndex,
        StopIndex = offendingSymbol.StopIndex,
        Line = line,
        PositionInLine = charPositionInLine
      };
    }

    public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
      //TODO I would like to have a better error message for this case.
      //TODO Need to figure out how to get the exact offending character since offendingSymbol is always zero
      string message = "";

      if (ReplMode)
      {
        for (int i = 0; i <= charPositionInLine; i++)
        {
          if (i == charPositionInLine)
          {
            message += '^';
          }
          else
          {
            message += ' ';
          }
        }
        message += $"\nUnknown Symbol Error: {msg}";
      }
      else
      {
        message += $"Unknown Symbol Error: {msg} in line {line} at {charPositionInLine}.\n";
        message += $"\t{msg}";
      }

      throw new SourceErrorException(message)
      {
        Line = line,
        PositionInLine = charPositionInLine
      };
    }
  }
}