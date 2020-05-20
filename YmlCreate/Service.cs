using System;
using System.Collections.Generic;

namespace YmlCreate
{
    [Serializable]
    public class Service
    {
        public string Name { get; set; }
        public byte[] Img { get; set; }

        public Service(string n, byte[] i)
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
    static class AllServiceOptions
    {
        public static readonly List<Options> GlobalOPtions = new List<Options>() {
            {new Options("args",ValueType.OneOrMore) },
            {new Options("build",ValueType.OneOrEmpty, new List<Options>()
                {
                    GlobalOPtions[0],
                    {new Options("context",ValueType.One)},
                    {new Options("dockerfile",ValueType.One)},
                    {new Options("cache_from",ValueType.One)},
                    {new Options("labels",ValueType.OneOrMore)},
                    {new Options("network",ValueType.One)},
                    {new Options("shm_size",ValueType.One)},
                    {new Options("target",ValueType.One)}
                }
                ) },
            {new Options("cap_add",ValueType.List)},
            {new Options("cap_drop",ValueType.List)},
            {new Options("cgroup_parent",ValueType.One)},
            {new Options("command",ValueType.One)},

        };
        //BUILD If you specify image as well as build, then Compose names the built image with the webapp and optional tag specified in image
    }
}