using Gtk;
using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    class OptionsWindow : Window
    {
        string ServiceName;
        Dictionary<string, List<Options>> Options;

        public OptionsWindow(string serviceName, Dictionary<string, List<Options>> options) : base(WindowType.Toplevel)
        {
            ServiceName = serviceName;
            Options = options;
            Build();
        }
        protected virtual void Build()
        {
            // Widget MainWindow
            Name = "MainWindow";
            Title = "Настройки "+ ServiceName;
            WindowPosition = ((WindowPosition)(4));
            Gtk.TreeView tree = new Gtk.TreeView();
            Add(tree);
        }
    }
}
