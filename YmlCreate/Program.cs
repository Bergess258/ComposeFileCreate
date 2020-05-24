﻿using Gtk;
using System.Collections.Generic;
using System.Threading;

namespace YmlCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Folder for yaml files
            System.IO.Directory.CreateDirectory("Yaml");

            Application.Init();
            //Create the Window
            MainWindow main = new MainWindow();
            Application.Run();
        }
    }
}
