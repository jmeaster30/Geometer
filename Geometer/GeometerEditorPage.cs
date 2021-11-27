using System;
using Geometer.Lib;
using Geometer.Lib.Model;
using Geometer.Utilities;
using Gtk;

namespace Geometer
{
  public class GeometerEditorPage : VBox
  {
    // execution stuff
    public Geo Geo { get; set; }

    //file stuff
    public string FilePath { get; }
    public bool Saved { get; set; }

    // main page stuff
    public Label Label { get; set; }
    public TextView TextView { get; set; }

    // toolbar variables
    public Label ErrorLabel { get; set; }
    public Label LineLabel { get; set; }
    public Label ColLabel { get; set; }
    public Label SelLabel { get; set; }

    //private junk
    private Action<string> DebouncedUpdate { get; set; }

    public GeometerEditorPage(string path) : this(path, string.IsNullOrEmpty(path) ? "New" : System.IO.Path.GetFileName(path)) { }

    public GeometerEditorPage(string path, string name)
    {
      Homogeneous = false;
      Spacing = 0;

      FilePath = string.IsNullOrEmpty(path) ? null : path;

      Label = new();
      Label.Text = name;

      TextView = new();
      TextView.Buffer.Changed += OnChanged;

      //There has to be a better way 
      TextView.KeyPressEvent += UpdateCursorLabels;
      TextView.KeyReleaseEvent += UpdateCursorLabels;
      TextView.ButtonPressEvent += UpdateCursorLabels;
      TextView.ButtonReleaseEvent += UpdateCursorLabels;

      Geo = new Geo();

      TextView.Buffer.TagTable.Add(new TextTag("ErrorTag")
      {
        Weight = Pango.Weight.Bold,
        Underline = Pango.Underline.Error,
        Foreground = "red"
      });

      ScrolledWindow swin = new();
      swin.Add(TextView);

      Toolbar tb = new();
      ToolItem ti = new();
      ErrorLabel = new()
      {
        Text = "no errors :)",
        LineWrap = false,
        MaxWidthChars = 120,
      };

      ti.Add(ErrorLabel);
      ti.Expand = true;
      tb.Add(ti);

      tb.Add(new SeparatorToolItem());

      ToolItem til = new();
      LineLabel = new()
      {
        Text = "Line: 0",
        LineWrap = false,
      };
      til.Add(LineLabel);
      tb.Add(til);

      tb.Add(new SeparatorToolItem());

      ToolItem tic = new();
      ColLabel = new()
      {
        Text = "Char: 0",
        LineWrap = false,
      };
      tic.Add(ColLabel);
      tb.Add(tic);

      tb.Add(new SeparatorToolItem());

      ToolItem tis = new();
      SelLabel = new()
      {
        Text = "Select: Nothing",
        LineWrap = false,
      };
      tis.Add(SelLabel);
      tb.Add(tis);

      tb.Add(new SeparatorToolItem());

      ToolButton refresh = new(null, "Refresh Model");
      refresh.Clicked += RefreshClicked;
      tb.Add(refresh);

      ToolButton clearQueries = new(null, "Clear Queries");
      clearQueries.Clicked += ClearQueryClicked;
      tb.Add(clearQueries);

      ToolButton closeTab = new(null, "Close Tab");
      closeTab.Clicked += CloseTab;
      tb.Add(closeTab);

      PackStart(swin, true, true, 0);
      PackEnd(tb, false, false, 0);

      Action<string> updateModel = (source) => { DoUpdateStuff(source); };
      DebouncedUpdate = updateModel.Debounce(200);
    }

    public void CloseTab(object sender, EventArgs eventArgs)
    {
      GeometerEditor editor = (GeometerEditor)Parent;
      editor.RemovePage(editor.PageNum(this));
    }

    public void OnChanged(object sender, EventArgs eventArgs)
    {
      TextBuffer buffer = (TextBuffer)sender;
      buffer.RemoveAllTags(buffer.StartIter, buffer.EndIter);
      DebouncedUpdate(buffer.Text);
    }

    public void DoUpdateStuff(string source)
    {
      GeoUpdateResult result = Geo.Model.Update(source);
      ErrorLabel.Text = result.Message.Limit(120);
      ErrorLabel.TooltipText = result.Message;
      TextBuffer buffer = TextView.Buffer;
      if (result.ErrorStart > -1 && result.ErrorStop > -1)
      {
        if (result.ErrorStart < result.ErrorStop)
        {
          //for ranged errors
          buffer.ApplyTag("ErrorTag", buffer.GetIterAtOffset(result.ErrorStart), buffer.GetIterAtOffset(result.ErrorStop + 1));
        }
        else if (result.ErrorStart == result.ErrorStop)
        {
          buffer.ApplyTag("ErrorTag", buffer.GetIterAtOffset(result.ErrorStart), buffer.GetIterAtOffset(result.ErrorStop + 1));
        }
        else
        {
          buffer.ApplyTag("ErrorTag", buffer.GetIterAtLineIndex(result.ErrorLine - 1, 0), buffer.GetIterAtOffset(result.ErrorStop));
        }
      }
      else if (result.ErrorLine > -1 && result.ErrorPos > -1)
      {
        buffer.ApplyTag("ErrorTag", buffer.GetIterAtLineIndex(result.ErrorLine - 1, result.ErrorPos), buffer.GetIterAtLineIndex(result.ErrorLine - 1, result.ErrorPos + 1));
      }
      //else there was no error
    }

    public void RefreshClicked(object sender, EventArgs eventArgs)
    {
      Console.WriteLine("Refresh");
    }

    public void ClearQueryClicked(object sender, EventArgs eventArgs)
    {
      Console.WriteLine("Clear Queries");
    }

    public void UpdateCursorLabels(object sender, EventArgs eventArgs)
    {
      TextBuffer buffer = TextView.Buffer;
      buffer.GetSelectionBounds(out TextIter start, out TextIter end);
      LineLabel.Text = $"Line: {end.Line + 1}";
      ColLabel.Text = $"Char: {end.LineOffset}";
      if (start.Offset != end.Offset)
      {
        SelLabel.TooltipText = $"From Line {start.Line + 1} Char {start.LineOffset}\n  To Line {end.Line + 1} Char {end.LineOffset}";
        SelLabel.Text = $"Select: {start.Offset} - {end.Offset}";
      }
      else
      {
        SelLabel.TooltipText = null;
        SelLabel.Text = "Select: Nothing";
      }
    }

  }
}