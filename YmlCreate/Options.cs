using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    public enum ValueType
    {
        One,
        OneOrMore,
        Empty,
        OneOrEmpty,
        List
    }
    class Options
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ValueType ValueType { get; set; }
        public List<Options> childs { get; set; }
        public Options(string name,ValueType valueType, List<Options> childOptions)
        {
            Name = name;
            ValueType = valueType;
            childs = childOptions;
        }
        public Options(string name, ValueType valueType)
        {
            Name = name;
            ValueType = valueType;
        }
    }
}
