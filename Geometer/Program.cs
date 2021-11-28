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
      
      GeometerEditor editor = new();
      GeometerMenuBar menuBar = new();

      menuBar.Editor = editor;

      VBox mainBox = new(false, 0);
      mainBox.PackStart(menuBar, false, false, 0);
      mainBox.PackStart(editor, true, true, 0);
      win.Add(mainBox);
      win.ShowAll();

      Application.Run();
    }
  }
}
