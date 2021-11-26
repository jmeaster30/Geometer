using System;
using Gtk;
using Pango;

namespace Geometer
{
  public class GeometerDisplay : DrawingArea
  {
    public GeometerDisplay()
    {
      Realized += OnRealized;
    }

    public static void OnRealized(object sender, EventArgs args)
    {

    }
  }
}