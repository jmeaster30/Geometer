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

      GeometerDisplay display = new();
      GeometerEditor editor = new();
      GeometerMenuBar menuBar = new();

      menuBar.Editor = editor;

      VBox mainBox = new(false, 0);
      mainBox.PackStart(menuBar, false, false, 0);

      HPaned pane = new();
      pane.Pack1(display, true, true);
      pane.Pack2(editor, true, false);

      mainBox.PackStart(pane, true, true, 0);
      win.Add(mainBox);
      win.ShowAll();

      Application.Run();
    }
  }
}
