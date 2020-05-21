using System;
using System.Collections.Generic;
using System.Text;

namespace YmlCreate
{
    public enum ValueType
    {
        One,// Simple value
        OneOrList, // One or a lot, with sumbol -
        Empty,//No value, childs
        OneOrEmpty,//value or childs
        List,// a lot with -
        ListWithValue,
        Number,
        Seconds,
        Special, //Some values with different ends like ns,ms,s,m and other
        ComboBox,//Some const values choose from
        Bool
    }
    class Options
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string HelpInfo { get; set; } //SomeUseful ingo 
        public string AdditionalInfo { get; set; }//Like Long or Short
        public ValueType ValueType { get; set; }
        public List<Options> childs { get; set; }
        public string[] ComboBoxValues { get; set; }
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
