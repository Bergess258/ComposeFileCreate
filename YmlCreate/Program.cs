﻿using Gtk;
using System.Collections.Generic;

namespace YmlCreate
{
    class Program
    {
        public static List<Service> AllServices;
        static void Main(string[] args)
        {
            //Parsing.Start();

            Application.Init();
            //Create the Window
            MainWindow main = new MainWindow();
            Application.Run();
        }
    }
}