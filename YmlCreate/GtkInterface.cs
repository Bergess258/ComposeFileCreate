using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Gdk;
using Gtk;

namespace YmlCreate
{
	public partial class MainWindow : Gtk.Window
	{
		const int Col_DisplayName = 0;
		const int Col_Pixbuf = 1;
		const int IV_ItemWidth = 70;

		private HBox hbox1;

		private VBox vbox1;

		private Label label1;

		private Button Btn_Search;

		private VBox vbox2;

		private Label label2;

		private VBox vbox4;

		private HBox hbox3;

		private Entry SearchS;


		private IconView IV_AllServices;
		private ScrolledWindow GtkScrolledWindow1;
		private ListStore AllServices;

		private IconView IV_SelectedServices;
		private ScrolledWindow GtkScrolledWindow2;
		private ListStore SelectedServices;


		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			Build();
		}

		protected virtual void Build()
		{
			// Widget MainWindow
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
			//All Services StoreList of string and Image
			SelectedServices = new ListStore(typeof(string), typeof(Pixbuf));
			Pixbuf OwnService = new Pixbuf(Resources.IconOwnService);
			SelectedServices.AppendValues("Свой сервис", OwnService);
			SelectedServices.SetSortColumnId(0, SortType.Ascending);
			// All Services IconViewSettings
			IV_SelectedServices = new IconView(SelectedServices);
			IV_SelectedServices.CanFocus = true;
			IV_SelectedServices.Name = "IV_SelectedServices";
			IV_SelectedServices.ItemWidth = IV_ItemWidth;
			IV_SelectedServices.TextColumn = Col_DisplayName;
			IV_SelectedServices.PixbufColumn = Col_Pixbuf;
			vbox1.PackStart(IV_SelectedServices, true, true, 0);
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

			//All Services StoreList of string and Image
			AllServices = new ListStore(typeof(string), typeof(Pixbuf));
			Pixbuf temp = new Pixbuf(@"C:\Users\Администратор.BERGESS\source\repos\ComposeFileCreate\YmlCreate\Imgs\OracleWebLogicServer_clr.png",64,64);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.AppendValues("Oracle WebLogic Server", temp);
			AllServices.SetSortColumnId(0, SortType.Ascending);

			GtkScrolledWindow1 = new ScrolledWindow();
			GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			GtkScrolledWindow1.ShadowType = ((ShadowType)(1));

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
			// Container child vbox4.Gtk.Box+BoxChild
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			SearchS = new Entry();
			SearchS.WidthRequest = 230;
			SearchS.CanFocus = true;
			SearchS.Name = "SearchS";
			SearchS.Text = "Введите название сервиса для поиска";
			SearchS.IsEditable = true;
			SearchS.InvisibleChar = '●';
			hbox3.Add(SearchS);
			hbox3.SetChildPacking(SearchS, false, false, 0, PackType.Start);
			// Container child hbox3.Gtk.Box+BoxChild
			Btn_Search = new Button();
			Btn_Search.CanFocus = true;
			Btn_Search.Name = "Btn_Search";
			Btn_Search.UseUnderline = true;
			Btn_Search.Label = "Поиск";
			hbox3.Add(Btn_Search);
			hbox3.SetChildPacking(Btn_Search, false, false, 0, PackType.Start);
			vbox4.Add(hbox3);
			vbox4.SetChildPacking(hbox3, false, false, 0, PackType.Start);
			vbox2.Add(vbox4);
			hbox1.Add(vbox2);
			Add(hbox1);
			//if ((Child != null))
			//{
			//	Child.ShowAll();
			//}
			DefaultWidth = 817;
			DefaultHeight = 400;
			ShowAll();
			DeleteEvent += new DeleteEventHandler(OnDeleteEvent);
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

	}
}
