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
            DefaultWidth = 400;
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
            col3.Title = "Столбец ввода значений у свойств с bool переменными и простыми записями после них";
            CellRendererText col3TextRenderer = new CellRendererText();
            col3TextRenderer.Editable = true;
            col3TextRenderer.Edited += OneOptionCell_Edited;

            CellRendererToggle col3Toggle = new CellRendererToggle();
            col3Toggle.Activatable = true;

            CellRendererSpin col3SpinR = new CellRendererSpin();
            col3SpinR.Editable = true;
            Adjustment adjCol3SpinR = new Adjustment(0, 0, 100000, 1, 10, 0);
            col3SpinR.Adjustment = adjCol3SpinR;
            col3SpinR.Edited += col3SpinRCell_Edited;

            col3.PackStart(col3TextRenderer, true);
            col3.PackStart(col3Toggle, true);
            col3.PackStart(col3SpinR, true);

            tree.AppendColumn(col2);
            tree.AppendColumn(col3);

            col3.AddAttribute(col3Toggle, "active", 1);
            col3.AddAttribute(col3TextRenderer, "text", 2);
            col3.AddAttribute(col3SpinR, "text", 2);
            //First Option, then there goes values for configs,string below works also for Numbers
            Store = new TreeStore(typeof(Options),typeof(bool),typeof(string));

            //ToggleFunc
            col3Toggle.Toggled += delegate (object o, ToggledArgs args) {
                TreeIter iter;
                if (Store.GetIter(out iter, new TreePath(args.Path)))
                {
                    bool temp = !(bool)Store.GetValue(iter, 1);
                    Store.SetValue(iter, 1, temp);
                    ((Options)Store.GetValue(iter, 0)).Value = temp.ToString();
                }  
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
            col3.SetCellDataFunc(col3SpinR, new TreeCellDataFunc(RenderSpinner));

            tree.Model = Store;

            Add(GtkScrolledWindow1);
            ShowAll();
        }

        //I didn't find other ophion to put different values in the same column(
        //So there ifs for each type of values
        void fillStore(TreeIter it,Options t)
        {
            if(t.ValueType == ValueType.One|| t.ValueType == ValueType.Time)
                if (t.DefaultValue == null)
                    it = Store.AppendValues(it, t, null,"");
                else
                    it = Store.AppendValues(it, t, null, (string)t.DefaultValue);
            else
                if(t.ValueType == ValueType.Bool)
                    if(t.DefaultValue==null)
                        it = Store.AppendValues(it, t,false,null);
                    else
                        it = Store.AppendValues(it, t, (bool)t.DefaultValue, null);
                else
                    if(t.ValueType == ValueType.Number)
                        if (t.DefaultValue == null)
                            it = Store.AppendValues(it, t, null, 0);
                        else
                            it = Store.AppendValues(it, t, null, Convert.ToInt32(t.DefaultValue));
                    else
                        it = Store.AppendValues(it,t, null,null);
            if (t.childs != null)
                foreach (Options tt in t.childs)
                    fillStore(it, tt);
        }

        private void RenderToggle(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType==ValueType.Bool;
        }

        private void RenderText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            if(temp!=null)
            (cell as Gtk.CellRendererText).Text = temp.Name;
        }

        private void RenderSpinner(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.Number;
        }


        private void RenderEditableText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.One || temp.ValueType == ValueType.Time;
        }

        private void OneOptionCell_Edited(object o, EditedArgs args)
        {
            Gtk.TreeIter iter;
            
            Store.GetIter(out iter, new Gtk.TreePath(args.Path));
            Store.SetValue(iter, 2, args.NewText);
            Options someText = (Options)Store.GetValue(iter, 0);
            someText.Value = args.NewText;
        }

        private void col3SpinRCell_Edited(object o, EditedArgs args)
        {
            Gtk.TreeIter iter;
            Store.GetIter(out iter, new Gtk.TreePath(args.Path));
            Store.SetValue(iter, 2, args.NewText);
            Options someText = (Options)Store.GetValue(iter, 0);
            someText.Value = args.NewText;
        }
    }
}
