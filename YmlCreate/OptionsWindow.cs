using GLib;
using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
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

            HBox hbox = new HBox();

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
            

            CellRendererToggle col3Toggle = new CellRendererToggle();
            col3Toggle.Activatable = true;

            CellRendererSpin col3SpinR = new CellRendererSpin();
            col3SpinR.Editable = true;
            Adjustment adjCol3SpinR = new Adjustment(0, 0, 100000, 1, 10, 0);
            col3SpinR.Adjustment = adjCol3SpinR;
            
            //ListStore m = new ListStore();
            CellRendererCombo col3Combo = new CellRendererCombo();
            col3Combo.Editable = true;

            //Using the same(no need to create another)
            col3TextRenderer.Edited += Cell_Edited;
            col3SpinR.Edited += Cell_Edited;
            col3Combo.Edited += Cell_Edited;
            col3Toggle.Xalign = 0;
            col3SpinR.Xalign = 0;
            col3Combo.Xalign = 0;

            col3.PackStart(col3TextRenderer, false);
            col3.PackStart(col3Toggle, false);
            col3.PackStart(col3SpinR, false);
            col3.PackStart(col3Combo, false);

            tree.AppendColumn(col2);
            tree.AppendColumn(col3);

            col3.AddAttribute(col3Toggle, "active", 1);
            col3.AddAttribute(col3TextRenderer, "text", 2);
            col3.AddAttribute(col3SpinR, "text", 2);
            col3.AddAttribute(col3Combo, "text", 2);
            //First Option, then there goes values for configs,string below works also for Numbers and ComboBox
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

            int length = Options.Count;
            for (int i = 0; i < length; i++)
            {
                TreeIter iter = Store.AppendValues(Options[i], null, null);
                int length2 = Options[i].childs.Count;
                for (int j = 0; j < length2; j++)
                {
                    fillStore(iter, Options[i].childs[j]);
                }  
            }

            col2.SetCellDataFunc(col2TextRendererFirst, new TreeCellDataFunc(RenderText));
            col3.SetCellDataFunc(col3Toggle, new TreeCellDataFunc(RenderToggle));
            col3.SetCellDataFunc(col3TextRenderer, new TreeCellDataFunc(RenderEditableText));
            col3.SetCellDataFunc(col3SpinR, new TreeCellDataFunc(RenderSpinner));
            col3.SetCellDataFunc(col3Combo, new TreeCellDataFunc(RenderCombo));

            tree.Model = Store;

            hbox.Add(GtkScrolledWindow1);
            Add(hbox);
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
            {
                int length = t.childs.Count;
                for (int i = 0; i < length; i++)
                {
                    fillStore(it, t.childs[i]);
                }
            }                    
        }

        private void RenderText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            if(temp!=null)
            (cell as Gtk.CellRendererText).Text = temp.Name;
        }

        private void RenderToggle(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.Bool;
            if (cell.Visible)
            {
                CellRendererToggle tog = (CellRendererToggle)cell;
                if (temp.Value == null)
                    tog.Active = (bool)temp.DefaultValue;
                else
                    tog.Active = temp.Value=="";
            }
        }

        private void RenderSpinner(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.Number;
            if (cell.Visible)
            {
                CellRendererSpin spinner = (CellRendererSpin)cell;
                if (temp.Value == null)
                    spinner.Text = (string)temp.DefaultValue;
                else
                    spinner.Text = temp.Value;
            }
        }

        private void RenderCombo(TreeViewColumn column, CellRenderer cell, ITreeModel model, TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.ComboBox;
            if (cell.Visible)
            {
                CellRendererCombo combo = (CellRendererCombo)cell;
                ListStore t = new ListStore(typeof(string));
                int length = temp.ComboBoxValues.Length;
                for (int i = 0; i < length; i++)
                {
                    t.AppendValues(temp.ComboBoxValues[i]);
                }
                combo.Model = t;
                if(temp.Value==null)
                    combo.Text = (string)temp.DefaultValue;
                else
                    combo.Text = temp.Value;
            }
        }

        private void RenderEditableText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            cell.Visible = temp.ValueType == ValueType.One || temp.ValueType == ValueType.Time;
        }

        private void Cell_Edited(object o, EditedArgs args)
        {
            TreeIter iter;
            
            Store.GetIter(out iter, new TreePath(args.Path));
            Store.SetValue(iter, 2, args.NewText);
            Options someText = (Options)Store.GetValue(iter, 0);
            someText.Value = args.NewText;
        }
    }
}
