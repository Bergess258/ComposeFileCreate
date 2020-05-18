using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    [Serializable]
    public class Service
    {
        public string Name { get; set; }
        public Gdk.Pixbuf Img { get; set; }

        public Service(string n, Gdk.Pixbuf i)
        {
            Name = n;
            Img = i;
        }
        public Service(string n)
        {
            Name = n;
        }
    }
    [Serializable]
    public class AllServices
    {
        public static List<Service> services = new List<Service>();
    }
}
