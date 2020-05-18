using System;
using System.Collections.Generic;
using System.Text;
using Gdk;
using Gtk;

namespace YmlCreate
{
    [Serializable]
    public class Service
    {
        public string Name { get; set; }
        public Pixbuf Img { get; set; }

        public Service(string n,Pixbuf i)
        {
            Name = n;
            Img = i;
        }
    }
    [Serializable]
    public class AllServices
    {
        public static List<Service> services = new List<Service>();
    }
}
