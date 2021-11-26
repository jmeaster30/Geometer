using System;
using System.Globalization;

namespace Geometer.Lib.Translator
{
  public class GeometerFormatProvider : IFormatProvider
  {
    public object GetFormat(Type formatType) => formatType == typeof(NumberFormatInfo) ? new NumberFormatInfo() { NegativeSign = "-", } : null;
  }
}
