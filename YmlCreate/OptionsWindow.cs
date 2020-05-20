using Gtk;
using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    class OptionsWindow : Window
    {
        string ServiceName;

        public OptionsWindow(string serviceName) : base(WindowType.Toplevel)
        {
            ServiceName = serviceName;
            Build();
        }
        protected virtual void Build()
        {
            // Widget MainWindow
            Name = "MainWindow";
            Title = "Настройки "+ ServiceName;
            WindowPosition = ((WindowPosition)(4));
        }
    }
}
