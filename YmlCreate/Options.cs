﻿using System.Collections.Generic;

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
        //Time,
        Special, //Some values with different ends like for config Long
        ComboBox,//Some const values choose from
        Bool
    }
    class Options
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public object DefaultValue { get; set; } // Default Value and Used as mark for value with quotes "value" 
        public string HelpInfo { get; set; } //SomeUseful info 
        public string AdditionalInfo { get; set; }//Like Long or Short
        public ValueType ValueType { get; set; }
        public List<Options> childs { get; set; }
        public List<string> ComboBoxValues { get; set; } // Uses also for list values
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

        //Clone
        public Options(Options t)
        {
            Name = t.Name;
            ValueType = t.ValueType;
            DefaultValue = t.DefaultValue;
            HelpInfo = t.HelpInfo;
            AdditionalInfo = t.AdditionalInfo;
            ValueType = t.ValueType;
            ComboBoxValues = t.ComboBoxValues;
            if (t.childs != null)
            {
                childs = new List<Options>();
                foreach (Options temp in t.childs)
                    childs.Add(new Options(temp));
            }
        }
    }
}
