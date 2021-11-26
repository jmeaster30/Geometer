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
      GeometerEditorPage page = new("");
      AppendPage(page, page.Label);
    }
  }
}