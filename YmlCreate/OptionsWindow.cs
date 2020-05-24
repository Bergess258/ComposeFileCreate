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
        const int Mass_ComboBox_Size = 10;
        string ServiceName;
        List<Options> Options;

        private ScrolledWindow GtkScrolledWindow1;
        private TreeView tree;
        private TreeStore Store;
        private Label name = new Label();

        private VBox CombinedViewForValueTypes = new VBox();

        private TreeView Listtree = new TreeView();
        private TreeStore ListStore = new TreeStore(typeof(string));

        private TreeView ListWithValuesTree = new TreeView();
        private TreeStore ListWithValuesStore = new TreeStore(typeof(string), typeof(string));

        private Options Selected;
        

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
            DefaultWidth = 700;
            DefaultHeight = 800;
            GtkScrolledWindow1 = new ScrolledWindow();
            GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            GtkScrolledWindow1.ShadowType = ((ShadowType)(1));
            GtkScrolledWindow1.WidthRequest = 500;

            HBox hbox = new HBox();

            tree = new TreeView();
            //tree.ButtonPressEvent +=Sele
            tree.RowActivated += new RowActivatedHandler(SelectRow);
            GtkScrolledWindow1.Add(tree);
            //TreeViewColumn col2 = new TreeViewColumn();

            TreeViewColumn col2 = new TreeViewColumn();
            CellRendererText col2TextRendererFirst = new CellRendererText();
            col2.PackStart(col2TextRendererFirst, true);

            TreeViewColumn col3 = new TreeViewColumn();
            col3.Expand = false;
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
            col3Combo.TextColumn = 0;
            col3Combo.HasEntry = false;

            //Using the same(no need to create another)
            col3TextRenderer.Edited += Cell_Edited;
            col3SpinR.Edited += Cell_Edited;
            //Using the same causes abnormal heigth change in selected row
            col3Combo.Edited += Combo_Edited;
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

            name.LabelProp = "";
            name.Wrap = true;
            name.Justify = ((Justification)(2));
            name.SingleLineMode = true;
            

            //ListOrOne
            TreeViewColumn ListCol = new TreeViewColumn();
            ListCol.Title = "Значение(я)";
            CellRendererText ListTextRenderer = new CellRendererText();
            ListTextRenderer.Editable = true;
            ListTextRenderer.Background = "white";
            ListTextRenderer.TraceConstruction = true;
            ListTextRenderer.Edited += ListCell_Edited;
            ListCol.PackStart(ListTextRenderer, true);
            Listtree.AppendColumn(ListCol);
            ListCol.AddAttribute(ListTextRenderer, "text", 0);
            Listtree.Model = ListStore;

            //ListWithValues
            TreeViewColumn ListWithValuesCol1 = new TreeViewColumn();
            ListWithValuesCol1.Title = "1 Значение";
            TreeViewColumn ListWithValuesCol2 = new TreeViewColumn();
            ListWithValuesCol2.Title = "2 Значение";

            CellRendererText ListWithValuesTR = new CellRendererText();
            ListWithValuesTR.Editable = true;
            ListWithValuesTR.Background = "white";
            ListWithValuesTR.TraceConstruction = true;
            ListWithValuesTR.Edited += ListWithValuesCell_Edited;

            CellRendererText ListWithValuesTR2 = new CellRendererText();
            ListWithValuesTR2.Editable = true;
            ListWithValuesTR2.Background = "white";
            ListWithValuesTR2.TraceConstruction = true;
            ListWithValuesTR2.Edited += ListWithValues2Cell_Edited;

            ListWithValuesCol1.PackStart(ListWithValuesTR, true);
            ListWithValuesCol2.PackStart(ListWithValuesTR2, true);
            ListWithValuesTree.AppendColumn(ListWithValuesCol1);
            ListWithValuesTree.AppendColumn(ListWithValuesCol2);
            ListWithValuesCol1.AddAttribute(ListWithValuesTR, "text", 0);
            ListWithValuesCol2.AddAttribute(ListWithValuesTR2, "text", 1);
            ListWithValuesTree.Model = ListWithValuesStore;


            CombinedViewForValueTypes.PackStart(name, false, false, 0);
            CombinedViewForValueTypes.PackStart(Listtree, false, false, 0);
            CombinedViewForValueTypes.PackStart(ListWithValuesTree, false, false, 0);
            hbox.Add(CombinedViewForValueTypes);
            Add(hbox);
            
            ShowAll();
            CombinedViewForValueTypes.Visible = false;
        }

        //I didn't find other ophion to put different values in the same column(
        //So there ifs for each type of values
        void fillStore(TreeIter it,Options t)
        {
            if(t.ValueType == ValueType.One|| t.ValueType == ValueType.Time || t.ValueType == ValueType.OneOrEmpty)
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

        #region Tree render and editing logic
        private void RenderText(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, ITreeModel model, Gtk.TreeIter iter)
        {
            Options temp = (Options)model.GetValue(iter, 0);
            if (temp != null)
            {
                (cell as Gtk.CellRendererText).Text = temp.Name;
                if(temp.AdditionalInfo!=null)
                    (cell as Gtk.CellRendererText).Text += temp.AdditionalInfo;
            } 
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
                int length = temp.ComboBoxValues.Count;
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
            cell.Visible = temp.ValueType == ValueType.One || temp.ValueType == ValueType.Time || temp.ValueType == ValueType.OneOrEmpty;
        }

        private void Cell_Edited(object o, EditedArgs args)
        {
            CombinedViewForValueTypes.Visible = false;
            TreeIter iter;
            Store.GetIter(out iter, new TreePath(args.Path));
            Store.SetValue(iter, 2, args.NewText);
            Options someText = (Options)Store.GetValue(iter, 0);
            if (args.NewText != "")
                someText.Value = args.NewText;
            else
                someText.Value = null;
        }

        private void Combo_Edited(object o, EditedArgs args)
        {
            CombinedViewForValueTypes.Visible = false;
            TreeIter iter;
            Store.GetIter(out iter, new TreePath(args.Path));
            Options someText = (Options)Store.GetValue(iter, 0);
            someText.Value = args.NewText;
        }
        #endregion

        private void SelectRow(object o, RowActivatedArgs args)
        {
            TreeIter iter;
            Store.GetIter(out iter, args.Path);
            Options someText = (Options)Store.GetValue(iter, 0);
            CombinedViewForValueTypes.Visible = false;
            Listtree.Visible = false;
            ListWithValuesTree.Visible = false;
            if (someText.ValueType==ValueType.List|| someText.ValueType == ValueType.OneOrList)
            {
                ListStore = new TreeStore(typeof(string));
                if (someText.ComboBoxValues == null)
                    someText.ComboBoxValues = new List<string>();
                int length = someText.ComboBoxValues.Count;
                for (int i = 0; i < length; i++)
                    ListStore.AppendValues(someText.ComboBoxValues[i]);
                ListStore.AppendValues("");
                Listtree.Model = ListStore;
                Selected = someText;
                CombinedViewForValueTypes.Visible = true;
                name.LabelProp = someText.Name;
                Listtree.Visible = true;
            }
            else
                if(someText.ValueType == ValueType.ListWithValue)
                {
                    ListWithValuesStore = new TreeStore(typeof(string), typeof(string));
                    if (someText.ComboBoxValues == null)
                        someText.ComboBoxValues = new List<string>();
                    int length = someText.ComboBoxValues.Count;
                    for (int i = 0; i < length; i++)
                        ListWithValuesStore.AppendValues(someText.ComboBoxValues[i].Split(':'));
                    ListWithValuesStore.AppendValues("", "");
                    ListWithValuesTree.Model = ListWithValuesStore;
                    Selected = someText;
                    CombinedViewForValueTypes.Visible = true;
                    name.LabelProp = someText.Name;
                    ListWithValuesTree.Visible = true;
                }
        }

        private void ListCell_Edited(object o, EditedArgs args)
        {
            TreeIter iter;
            ListStore.GetIter(out iter, new TreePath(args.Path));
            int i = Convert.ToInt32(args.Path);
            if (args.NewText != "")
            {
                if ((string)ListStore.GetValue(iter, 0) == "")
                {
                    ListStore.AppendValues("");
                    Selected.ComboBoxValues.Add(args.NewText);
                }
                else
                    Selected.ComboBoxValues[i] = args.NewText;
                ListStore.SetValue(iter, 0, args.NewText);

            }
            else
                if ((string)ListStore.GetValue(iter, 0) != "")
                {
                    ListStore.Remove(ref iter);
                    Selected.ComboBoxValues.RemoveAt(i);
                }
                    
        }

        private void ListWithValuesCell_Edited(object o, EditedArgs args)
        {
            TreeIter iter;
            ListWithValuesStore.GetIter(out iter, new TreePath(args.Path));
            int i = Convert.ToInt32(args.Path);
            string another = (string)ListWithValuesStore.GetValue(iter, 1);
            if (args.NewText != "")
            {
                if ((string)ListWithValuesStore.GetValue(iter, 0) == "" && another != "")
                {
                    ListWithValuesStore.AppendValues("", "");
                    Selected.ComboBoxValues.Add(args.NewText + ":" + another);
                }
            }
            else
                if ((string)ListWithValuesStore.GetValue(iter, 0) != "" && another!="")
                    Selected.ComboBoxValues.RemoveAt(i);
                else
                    ListWithValuesStore.Remove(ref iter);
            ListWithValuesStore.SetValue(iter, 0, args.NewText);
        }

        private void ListWithValues2Cell_Edited(object o, EditedArgs args)
        {
            TreeIter iter;
            ListWithValuesStore.GetIter(out iter, new TreePath(args.Path));
            int i = Convert.ToInt32(args.Path);
            string another = (string)ListWithValuesStore.GetValue(iter, 0);
            if (args.NewText != "")
            {
                if ((string)ListWithValuesStore.GetValue(iter, 1) == "" && another != "")
                {
                    ListWithValuesStore.AppendValues("", "");
                    Selected.ComboBoxValues.Add(another + ":" + args.NewText);
                }
            }
            else
                if ((string)ListWithValuesStore.GetValue(iter, 1) != "" && another != "")
                    Selected.ComboBoxValues.RemoveAt(i);
                else
                    ListWithValuesStore.Remove(ref iter);
            ListWithValuesStore.SetValue(iter, 1, args.NewText);
        }

    }
}
