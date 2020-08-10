using Gtk;
using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using Gdk;

namespace YmlCreate
{
    class SettingsWindow : Gtk.Window
    {
        const int WindowWidth = 230;
        const int WindowHeight = 30;
        Label ComposeVerL;
        ComboBox ComposeVerCB;
        public delegate void Closing();
        public Closing WindowClosing;

        VBox vBox;
        HBox HBoxVersion;
        public SettingsWindow() : base(Gtk.WindowType.Toplevel)
        {
            Name = "SettingsWindow";
            Title = "Настройки";
            WindowPosition = ((WindowPosition)(4));
            DefaultWidth = WindowWidth;
            DefaultHeight = WindowHeight;
            vBox = new VBox();
            HBoxVersion = new HBox();
            ComposeVerL = new Label();
            ComposeVerL.Name = "ComposeVerL";
            ComposeVerL.LabelProp = "Версия Compose файла";
            ComposeVerL.HeightRequest = 10;
            ComposeVerL.Wrap = true;
            ComposeVerL.Justify = ((Justification)(2));
            ComposeVerL.SingleLineMode = true;
            HBoxVersion.PackStart(ComposeVerL, false, true, 0);
            ComposeVerCB = new ComboBox(PersonalSettings.appConfig.ComposeFileVerions);
            TreeIter treeIter;
            ComposeVerCB.Model.IterNthChild(out treeIter, Array.IndexOf(PersonalSettings.appConfig.ComposeFileVerions, PersonalSettings.appConfig.LastChoosedVersion));
            ComposeVerCB.SetActiveIter(treeIter);
            ComposeVerCB.CanFocus = true;
            ComposeVerCB.Name = "ComposeVerCB";
            ComposeVerCB.Changed += new EventHandler(OnComposeVerEChanged);
            ComposeVerCB.MarginLeft = 5;
            HBoxVersion.PackStart(ComposeVerCB, false, true, 0);
            HBoxVersion.HeightRequest = 10;
            HBoxVersion.Margin = 2;
            vBox.PackStart(HBoxVersion,false, false, 0);
            Add(vBox);
        }

        private void OnComposeVerEChanged(object sender, EventArgs e)
        {
            PersonalSettings.appConfig.LastChoosedVersion = PersonalSettings.appConfig.ComposeFileVerions[ComposeVerCB.Active];
            PersonalSettings.Save();
        }

        protected override bool OnDeleteEvent(Event evnt)
        {
            WindowClosing();
            return base.OnDeleteEvent(evnt);
        }

        //protected void OnComposeVerEChanged(object sender, EventArgs e)
        //{
        //    if (T_Search.Status == TaskStatus.Running)
        //    {
        //        ts.Cancel();
        //        Thread.Sleep(100);
        //        ts = new CancellationTokenSource();
        //        ct = ts.Token;
        //    }
        //    T_Search = new Task(Search, ct);
        //    T_Search.Start();
        //}
    }
}
