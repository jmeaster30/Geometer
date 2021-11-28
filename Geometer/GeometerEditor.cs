using Gtk;

namespace Geometer
{
  public class GeometerEditor : Notebook
  {
    public GeometerDisplay GeometerDisplay { get; set; }

    public GeometerEditor()
    {
      CreateNewPage();
    }

    public void CreateNewPage()
    {
      GeometerEditorPage page = new(null, $"New {NPages + 1}");
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