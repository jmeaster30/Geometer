using System;
using Gtk;

namespace Geometer
{
  public class GeometerMenuBar : MenuBar
  {
    public GeometerMenuBar()
    {
      Menu file_menu = new();

      MenuItem exit_item = new("Exit");
      exit_item.Activated += new EventHandler(OnExit);
      file_menu.Append (exit_item);

      MenuItem file_item = new("File");
      file_item.Submenu = file_menu;
      Append(file_item);
    }

    public static void OnExit(object o, EventArgs eventArgs)
    {
      Application.Quit();
    }
  }
}