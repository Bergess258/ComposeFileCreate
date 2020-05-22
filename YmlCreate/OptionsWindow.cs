using GLib;
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

        private ScrolledWindow GtkScrolledWindow1;
        private TreeView tree;
        private TreeStore Store;

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
            DefaultWidth = 300;
            DefaultHeight = 700;
            GtkScrolledWindow1 = new ScrolledWindow();
            GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            GtkScrolledWindow1.ShadowType = ((ShadowType)(1));

            tree = new Gtk.TreeView();
            GtkScrolledWindow1.Add(tree);
            //TreeViewColumn col2 = new TreeViewColumn();
            //CellRendererText col2TextRenderer = new CellRendererText();
            //col2.PackStart(col2TextRenderer, true);
            TreeViewColumn col3 = new TreeViewColumn();
            CellRendererToggle col3Toggle = new CellRendererToggle();
            col3Toggle.Activatable = true;
            
            col3.PackStart(col3Toggle, true);
            //tree.AppendColumn(col2);
            tree.AppendColumn("", new CellRendererText(), "text", 0);
            tree.AppendColumn(col3);
            //col2.AddAttribute(col2TextRenderer, "text", 0);
            col3.AddAttribute(col3Toggle, "active", 5);

            Store = new TreeStore(typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(bool));
            col3Toggle.Toggled += delegate (object o, ToggledArgs args) {
                TreeIter iter;
                if (Store.GetIter(out iter, new TreePath(args.Path)))
                    Store.SetValue(iter, 5, !(bool)Store.GetValue(iter, 5));
            };
            foreach (KeyValuePair<string, List<Options>> keyValue in AllServiceOptions.allConfigs)
            {
                TreeIter iter = Store.AppendValues(keyValue.Key);
                foreach (Options t in keyValue.Value)
                    fillStore(iter,t);
            }
            col3.SetCellDataFunc(col3Toggle, new TreeCellDataFunc(RenderToggle));
            tree.Model = Store;
            
            //TreeViewColumn col4 = new TreeViewColumn();
            //TreeViewColumn col5 = new TreeViewColumn();
            Add(GtkScrolledWindow1);
            ShowAll();
        }

        void fillStore(TreeIter it,Options t)
        {
            if(t.ValueType!=ValueType.Bool)
                it = Store.AppendValues(it, t.Name,null);
            else
                it = Store.AppendValues(it, t.Name,true);
            if (t.childs != null)
                foreach (Options tt in t.childs)
                    fillStore(it, tt);
        }

        private void RenderToggle(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            if(model.GetValue(iter, 1)==null)
                (cell as Gtk.CellRendererToggle).Visible = false;
            else
                (cell as Gtk.CellRendererToggle).Visible = true;
        }

    }
}
