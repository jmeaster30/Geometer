using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Geometer.Utilities;
using Gtk;

namespace Geometer
{
  public class GeometerMenuBar : MenuBar
  {
    public GeometerEditor Editor { get; set; }

    public GeometerMenuBar()
    {
      Menu file_menu = new();

      MenuItem new_item = new("New");
      new_item.Activated += new EventHandler(OnNew);
      file_menu.Append(new_item);

      MenuItem open_item = new("Open");
      open_item.Activated += new EventHandler(OnOpen);
      file_menu.Append(open_item);

      MenuItem save_item = new("Save");
      save_item.Activated += new EventHandler(OnSave);
      file_menu.Append(save_item);

      file_menu.Append(new SeparatorMenuItem());

      MenuItem exit_item = new("Exit");
      exit_item.Activated += new EventHandler(OnExit);
      file_menu.Append(exit_item);

      MenuItem file_item = new("File");
      file_item.Submenu = file_menu;
      Append(file_item);

      Append(CreateSampleMenu());
    }

    public void OnNew(object o, EventArgs eventArgs)
    {
      Editor.CreateNewPage();
    }

    public void OnOpen(object o, EventArgs eventArgs)
    {

    }

    public void OnSave(object o, EventArgs eventArgs)
    {

    }

    public static void OnExit(object o, EventArgs eventArgs)
    {
      Application.Quit();
    }

    public MenuItem CreateSampleMenu()
    {
      Menu sample_menu = new();
      List<string> samples = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                .Where(x => x.StartsWith("Geometer.Samples")).ToList();

      foreach (string sample in samples)
      {
        string sample_name = string.Join(' ', sample.Split('.')[2].Split('_').Select(x => x.ToUpperCase()));

        MenuItem sample_menu_item = new(sample_name);
        sample_menu_item.Activated += new EventHandler(LoadSample);
        sample_menu.Append(sample_menu_item);
      }

      MenuItem sample_item = new("Samples");
      sample_item.Submenu = sample_menu;
      return sample_item;
    }

    public void LoadSample(object o, EventArgs eventArgs)
    {
      MenuItem clicked = (MenuItem)o;
      string resource_name = $"Geometer.Samples.{clicked.Label.ToLowerInvariant().Replace(' ', '_')}.geo";

      using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource_name);
      using StreamReader reader = new(stream);
      string sample_content = reader.ReadToEnd();

      Editor.CreatePageFromSample(clicked.Label, sample_content);
    }
  }
}