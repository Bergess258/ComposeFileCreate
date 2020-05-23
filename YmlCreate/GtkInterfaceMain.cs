using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;
using Gdk;
using Gtk;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace YmlCreate
{
	public partial class MainWindow : Gtk.Window
	{
		const int Col_DisplayName = 1;
		const int Col_Pixbuf = 2;
		const int IV_ItemWidth = 70;
		static Pixbuf DefaultServiceIcon = new Pixbuf(Resources.DefultServiceIcon);
		static Pixbuf OwnService = new Pixbuf(Resources.IconOwnService);

		private HBox hbox1;

		private VBox vbox1;

		private Label label1;

		private Button Btn_Create;

		private VBox vbox2;

		private Label label2;

		private VBox vbox4;

		private VBox Vbox_Search;
		private Label Lbl_Search;

		private HBox hbox3;

		private static Entry SearchS;


		private static IconView IV_AllServices;
		private ScrolledWindow GtkScrolledWindow1;
		private static ListStore AllServices;
		private static CancellationTokenSource ts = new CancellationTokenSource();
		private static CancellationToken ct = ts.Token;
		private Task T_Search = new Task(Search,ct);
		private Task Loading = new Task(LoadAllServices);

		private IconView IV_SelectedServices;
		private ScrolledWindow GtkScrolledWindow2;
		private ListStore SelectedServices;

		static List<Service> AllServicesList;
		static Pixbuf[] ServicesImg;

		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			Loading.Start();
			Build();
		}

		protected virtual void Build()
		{
			//MainWindow
			Name = "MainWindow";
			Title = "Построение yml файла на основе заданных сервисов";
			WindowPosition = ((WindowPosition)(4));
			// Container child MainWindow.Gtk.Container+ContainerChild
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 2;
			// Container child hbox1.Gtk.Box+BoxChild
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 1;
			// Container child vbox1.Gtk.Box+BoxChild
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = "Выбранные на данный момент";
			label1.Justify = ((Justification)(2));
			label1.SingleLineMode = true;
			label1.Expand = false;
			vbox1.PackStart(label1, false, false, 0);

			//For Selected Services scrolled window
			GtkScrolledWindow2 = new ScrolledWindow();
			GtkScrolledWindow2.Name = "GtkScrolledWindow1";
			GtkScrolledWindow2.ShadowType = ((ShadowType)(1));

			//Selected Services StoreList of string and Image
			SelectedServices = new ListStore(typeof(int),typeof(string), typeof(Pixbuf),typeof(List<Options>));
			List<Options> temp = new List<Options>();
			foreach (Options t in AllServiceOptions.allConfigs)
				temp.Add(t);
			SelectedServices.AppendValues(0,"Свой сервис", OwnService, temp);
			SelectedServices.SetSortColumnId(0, SortType.Ascending);
			// Selected Services IconViewSettings
			IV_SelectedServices = new IconView(SelectedServices);
			IV_SelectedServices.CanFocus = true;
			IV_SelectedServices.Name = "IV_SelectedServices";
			IV_SelectedServices.ItemWidth = IV_ItemWidth;
			IV_SelectedServices.TextColumn = Col_DisplayName;
			IV_SelectedServices.PixbufColumn = Col_Pixbuf;
			GtkScrolledWindow2.Add(IV_SelectedServices);
			vbox1.Add(GtkScrolledWindow2);
			IV_SelectedServices.GrabFocus();
			hbox1.Add(vbox1);
			// Container child hbox1.Gtk.Box+BoxChild
			vbox2 = new VBox();
			vbox2.Spacing = 1;
			// Container child vbox2.Gtk.Box+BoxChild
			label2 = new Label();
			label2.Name = "label2";
			label2.LabelProp = "Список всех сервисов";
			label2.Wrap = true;
			label2.Justify = ((Justification)(2));
			label2.SingleLineMode = true;
			vbox2.Add(label2);
			vbox2.SetChildPacking(label2, false, false, 0, PackType.Start);
			// Container child vbox2.Gtk.Box+BoxChild
			vbox4 = new VBox();
			vbox4.Name = "vbox4";
			vbox4.Spacing = 1;

			// Container child vbox4.Gtk.Box+BoxChild
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;

			Vbox_Search = new VBox();
			Vbox_Search.Name = "Vbox_Search";
			Vbox_Search.Spacing = 1;

			// Container child hbox3.Gtk.Box+BoxChild
			SearchS = new Entry();
			SearchS.WidthRequest = 230;
			SearchS.CanFocus = true;
			SearchS.Name = "SearchS";
			SearchS.Text = "";
			SearchS.IsEditable = true;
			SearchS.InvisibleChar = '●';
			SearchS.Changed += new EventHandler(OnSearchSChanged);

			Lbl_Search = new Label();
			Lbl_Search.Name = "Lbl_Search";
			Lbl_Search.LabelProp = "Строка поиска сервиса";
			Lbl_Search.Justify = Justification.Left;
			Lbl_Search.SingleLineMode = true;
			Lbl_Search.Xalign = 0F;

			Vbox_Search.PackStart(Lbl_Search, false, false, 0);
			Vbox_Search.PackStart(SearchS, false, false, 0);
			//Just to create empty space after input field
			Vbox_Search.PackStart(new Separator(Orientation.Horizontal), false, false, 0);
			hbox3.PackStart(Vbox_Search, false, false, 0);
			Btn_Create = new Button();
			Btn_Create.CanFocus = true;
			Btn_Create.Name = "Btn_Search";
			Btn_Create.UseUnderline = true;
			Btn_Create.Label = "Создать Yaml";
			hbox3.PackStart(Btn_Create, false, false, 0);

			Loading.Wait();
			//All Services StoreList of their name and Image
			AllServices = new ListStore(typeof(int),typeof(string), typeof(Pixbuf));
			AllServices.SetSortColumnId(0, SortType.Ascending);

			GtkScrolledWindow1 = new ScrolledWindow();
			GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			GtkScrolledWindow1.ShadowType = ((ShadowType)(1));

			//Adding services to store
			int length = ServicesImg.Length;
			for (int i = 0; i < length; ++i)
			{
				AllServicesList[i].Img = null;
				if (ServicesImg[i] != null)
					AllServices.AppendValues(i, AllServicesList[i].Name, ServicesImg[i]);
				else
					AllServices.AppendValues(i, AllServicesList[i].Name, DefaultServiceIcon);
			}

			// All Services IconViewSettings
			IV_AllServices = new IconView(AllServices);
			IV_AllServices.CanFocus = true;
			IV_AllServices.Name = "IV_AllServices";
			IV_AllServices.ItemWidth = IV_ItemWidth;
			IV_AllServices.TextColumn = Col_DisplayName;
			IV_AllServices.PixbufColumn = Col_Pixbuf;
			GtkScrolledWindow1.Add(IV_AllServices);
			vbox4.Add(GtkScrolledWindow1);
			IV_AllServices.GrabFocus();
			
			vbox4.Add(hbox3);
			vbox4.SetChildPacking(hbox3, false, false, 0, PackType.Start);
			vbox2.Add(vbox4);
			hbox1.Add(vbox2);
			Add(hbox1);
			DefaultWidth = 817;
			DefaultHeight = 400;
			ShowAll();
			DeleteEvent += new DeleteEventHandler(OnDeleteEvent);
			IV_AllServices.ItemActivated += new ItemActivatedHandler(OnIV_AllServicesItemActivated);
			IV_SelectedServices.ItemActivated += new ItemActivatedHandler(OnIV_SelectedServicesItemActivated);
		}

		private static void LoadAllServices()
		{
			//Deserialization of all services
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
			AllServicesList = (List<Service>)formatter.Deserialize(stream);
			stream.Close();
			int length = AllServicesList.Count;
			ServicesImg = new Pixbuf[length];
			for (int i = 0; i < length; i++)
			{
				if(AllServicesList[i].Img!=null)
					ServicesImg[i] = new Pixbuf(AllServicesList[i].Img, 64, 64);
			}
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}
		#region SearchLogic
		static void Search()
		{
			if (SearchS.Text != "")
			{
				int c = 0;
				ListStore temp = new ListStore(typeof(int), typeof(string), typeof(Pixbuf));
				temp.SetSortColumnId(0, SortType.Ascending);
				IV_AllServices.FreezeChildNotify();
				IV_AllServices.Model = null;
				AWQ uwu = new AWQ(SearchS.Text.ToCharArray());
				int length = AllServicesList.Count;
				for (int i = 0; i < length; i++)
				{
					if (uwu.search(AllServicesList[i].Name.ToCharArray()))
						if (ServicesImg[i] != null)
							temp.AppendValues(c++, AllServicesList[i].Name, ServicesImg[i]);
						else
							temp.AppendValues(c++, AllServicesList[i].Name, DefaultServiceIcon);
				}
				IV_AllServices.Model = temp;
				IV_AllServices.ThawChildNotify();
			}
			else
				IV_AllServices.Model = AllServices;
		}
		protected void OnSearchSChanged(object sender, EventArgs e)
		{
			if (T_Search.Status == TaskStatus.Running)
			{
				ts.Cancel();
				Thread.Sleep(100);
				ts = new CancellationTokenSource();
				ct = ts.Token;
			}
			T_Search = new Task(Search, ct);
			T_Search.Start();
		}
		#endregion

		//Service selection from all
		protected void OnIV_AllServicesItemActivated(object o, ItemActivatedArgs a)
		{
			TreeIter iter;
			IV_AllServices.FreezeChildNotify();
			IV_AllServices.Model.GetIter(out iter,a.Path);
			int c = (int)(IV_AllServices.Model.GetValue(iter, 0));
			string name = IV_AllServices.Model.GetValue(iter, 1).ToString();
			Pixbuf Img = (Pixbuf)(IV_AllServices.Model.GetValue(iter, 2));
			List<Options> temp = new List<Options>();
			foreach (Options t in AllServiceOptions.allConfigs)
				temp.Add(t);
			SelectedServices.AppendValues(c,name,Img,temp);
			IV_AllServices.ThawChildNotify();
			AllServices.Remove(ref iter);
			OnSearchSChanged(new object(),new EventArgs());
		}

		protected void OnIV_SelectedServicesItemActivated(object o, ItemActivatedArgs a)
		{
			TreeIter iter;
			IV_SelectedServices.Model.GetIter(out iter, a.Path);
			List<Options> te = (List<Options>)(IV_SelectedServices.Model.GetValue(iter, 3));
			new OptionsWindow((string)(IV_SelectedServices.Model.GetValue(iter, 1)), te);
		}
	}
}
