using System;
using Gtk;

namespace Geometer
{
  public class GeometerEditor : Notebook
  {
    public GeometerEditor()
    {
      CreateNewPage();
    }

    public void CreateNewPage()
    {
      GeometerEditorPage page = new(null, $"New {NPages}");
      int new_page = AppendPage(page, page.Label);
      ShowAll();
      CurrentPage = new_page;
    }

    public void CreatePageFromSample(string name, string content)
    {
      GeometerEditorPage page = new(null, name);
      page.TextView.Buffer.Text = content;
      int new_page = AppendPage(page, page.Label);
      ShowAll();
      CurrentPage = new_page;
    }
  }
}