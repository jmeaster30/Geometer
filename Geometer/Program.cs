using Gtk;

namespace Geometer
{
  public class Program
  {
    public static void Main()
    {
      Application.Init();

      Window win = new("Geometer");
      win.DeleteEvent += delegate { Application.Quit(); };
      win.SetDefaultSize(800, 600);

      VBox mainBox = new(false, 0);
      mainBox.PackStart(new GeometerMenuBar(), false, false, 0);

      HPaned pane = new();
      pane.Pack1(new GeometerDisplay(), true, true);
      pane.Pack2(new GeometerEditor(), true, false);

      mainBox.PackStart(pane, true, true, 0);
      win.Add(mainBox);
      win.ShowAll();

      Application.Run();
    }
  }
}
