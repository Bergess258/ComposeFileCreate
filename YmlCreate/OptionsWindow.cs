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
        List<Options> Options;

        private ScrolledWindow GtkScrolledWindow1;
        private TreeView tree;
        private TreeStore Store;

        public OptionsWindow(string serviceName, List<Options> options) : base(WindowType.Toplevel)
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

            TreeViewColumn col2 = new TreeViewColumn();
            CellRendererText col2TextRendererFirst = new CellRendererText();
            col2.PackStart(col2TextRendererFirst, true);

            TreeViewColumn col3 = new TreeViewColumn();
            col3.Title = "Столбец ввода значений у свойств сервисов";
            CellRendererText col3TextRenderer = new CellRendererText();
            col3TextRenderer.Editable = true;
            col3TextRenderer.Edited += OneOptionCell_Edited;
            CellRendererToggle col3Toggle = new CellRendererToggle();
            col3Toggle.Activatable = true;
            col3.PackStart(col3TextRenderer, true);
            col3.PackStart(col3Toggle, true);

            tree.AppendColumn(col2);
            tree.AppendColumn(col3);
            col3.AddAttribute(col3Toggle, "active", 1);
            col3.AddAttribute(col3TextRenderer, "text", 2);
            //First 5 strings for configs names, then there goes values for configs
            Store = new TreeStore(typeof(Options),typeof(bool),typeof(string));
            col3Toggle.Toggled += delegate (object o, ToggledArgs args) {
                TreeIter iter;
                if (Store.GetIter(out iter, new TreePath(args.Path)))
                    Store.SetValue(iter, 1, !(bool)Store.GetValue(iter, 1));
            };
            foreach (Options opt in Options)
            {
                TreeIter iter = Store.AppendValues(opt,null,null);
                foreach (Options t in opt.childs)
                    fillStore(iter,t);
            }
            col2.SetCellDataFunc(col2TextRendererFirst, new TreeCellDataFunc(RenderText));
            col3.SetCellDataFunc(col3Toggle, new TreeCellDataFunc(RenderToggle));
            col3.SetCellDataFunc(col3TextRenderer, new TreeCellDataFunc(RenderEditableText));
            tree.Model = Store;
            
            //TreeViewColumn col4 = new TreeViewColumn();
            //TreeViewColumn col5 = new TreeViewColumn();
            Add(GtkScrolledWindow1);
            ShowAll();
        }

        void fillStore(TreeIter it,Options t)
        {
            if(t.ValueType == ValueType.One)
                it = Store.AppendValues(it, t, null,"");
            else
                if(t.ValueType == ValueType.Bool)
                    it = Store.AppendValues(it, t,true,null);
                else
                    it = Store.AppendValues(it,t, null,null);
            if (t.childs != null)
                foreach (Options tt in t.childs)
                    fillStore(it, tt);
        }

        private void RenderToggle(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            bool temp = (bool)model.GetValue(iter, 1);
            cell.Visible = temp;
            if(temp==true)
                (cell as CellRendererToggle).Active = false;
        }

        private void RenderText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            if(temp!=null)
            (cell as Gtk.CellRendererText).Text = temp.Name;
        }

        private void RenderEditableText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            string temp = (string)model.GetValue(iter, 2);
            if ( temp == null)
                cell.Visible = false;
            else
            {
                cell.Visible = true;
                //(cell as CellRendererText).Text = "Govno kod blyet";
            }
                
        }

        private void OneOptionCell_Edited(object o, Gtk.EditedArgs args)
        {
            Gtk.TreeIter iter;
            Store.GetIter(out iter, new Gtk.TreePath(args.Path));

            Options someText = (Options)Store.GetValue(iter, 0);
            someText.Value = args.NewText;
        }
    }
}
