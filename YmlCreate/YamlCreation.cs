using System.Collections.Generic;
using System.Linq;

namespace YmlCreate
{
    static class Yaml
    {
        static double Version = 3.8;
        public static void Create(string path,Dictionary<string,List<Options>> Services)
        {
            if (!path.Contains(".yaml") && !path.Contains(".yml"))
                path += ".yml";
            string mainStr = "version: \""+ Version + "\"\n";
            Stack<KeyValuePair<int,string>> stack = new Stack<KeyValuePair<int, string>>();
            foreach(KeyValuePair<string, List<Options>> pair in Services)
            {
                bool t = false;
                    foreach (Options temp in pair.Value[0].childs)
                        if (WriteThisOption(temp, ref stack, 2))
                            t = true;
                if (t)
                    stack.Push(new KeyValuePair<int, string>(1, pair.Key + ":"));
            }

            if (stack.Count > 0)
            {
                stack.Push(new KeyValuePair<int, string>(0, "services:"));
                mainStr += WriteStack(stack);
            }
            //Checking another options by hands) All of services have the same values for options below
            if (WriteThisOption(Services.First().Value[1], ref stack, 0))
                mainStr += WriteStack(stack);
            if (WriteThisOption(Services.First().Value[2], ref stack, 0))
                mainStr += WriteStack(stack);
            if (WriteThisOption(Services.First().Value[3], ref stack, 0))
                mainStr += WriteStack(stack);
            if (WriteThisOption(Services.First().Value[4], ref stack, 0))
                mainStr += WriteStack(stack);
            System.IO.File.WriteAllText(path, mainStr);
        }

        static bool WriteThisOption(Options option, ref Stack<KeyValuePair<int, string>> stack,int i)
        {
            if(option.ValueType == ValueType.One || option.ValueType == ValueType.OneOrEmpty)
            {
                if (option.Value != null)
                    if (option.Value != "")
                    {
                        if(option.DefaultValue==null)// Using it as mark for "" value
                            stack.Push(new KeyValuePair<int, string>(i, option.Name + ": " + option.Value));
                        else
                            if (option.Name != "shm_size")//Should check this because of it's special writing type
                                stack.Push(new KeyValuePair<int, string>(i, option.Name + ": \"" + option.Value + "\""));
                            else
                            {
                                if (option.Value.IndexOf('\'') == 0 && option.Value.LastIndexOf('\'') == option.Value.Length - 1)
                                    stack.Push(new KeyValuePair<int, string>(i, option.Name + ": " + option.Value));
                                else
                                {
                                    option.Value.Replace("\'", "");
                                    if (option.Value.Contains('b') || option.Value.Contains('k') || option.Value.Contains('m') || option.Value.Contains('g') || option.Value.Contains("kb") || option.Value.Contains("mb") || option.Value.Contains("gb"))
                                        stack.Push(new KeyValuePair<int, string>(i, option.Name + ": \"" + option.Value + "\""));
                                    else
                                        stack.Push(new KeyValuePair<int, string>(i, option.Name + ": " + option.Value));
                                }
                            }
                        return true;
                    }
            }
            else
                if (option.ValueType == ValueType.List || option.ValueType == ValueType.ListWithValue)
                {
                    if (option.ComboBoxValues != null)//As i wrote ComboBoxValues is used for lists values
                {
                        //Write bool value with ''
                        if (option.ValueType == ValueType.List)
                            foreach (string str in option.ComboBoxValues)
                                if (option.DefaultValue != null) // Using it as mark for "" value
                                    stack.Push(new KeyValuePair<int, string>(i+1, "- \"" + str + "\""));
                                else
                                    stack.Push(new KeyValuePair<int, string>(i+1, "- " + str));
                        else
                            foreach (string str in option.ComboBoxValues)
                                if (option.DefaultValue != null)
                                {
                                    string[] temp = str.Split(": ");
                                    stack.Push(new KeyValuePair<int, string>(i+1, temp[0] + ": \"" + temp[1] + "\""));
                                }
                                else
                                    stack.Push(new KeyValuePair<int, string>(i+1, str));
                        stack.Push(new KeyValuePair<int, string>(i, option.Name));
                        return true;
                    }
                }
                else
                    if(option.ValueType == ValueType.Bool)
                    {
                        if (option.Value != null)
                        {
                            if (option.Value == "True")
                                stack.Push(new KeyValuePair<int, string>(i, option.Name + ": true"));
                            else
                                stack.Push(new KeyValuePair<int, string>(i, option.Name + ": false"));
                            return true;
                        }
                    }
                    else
                        if (option.ValueType == ValueType.ComboBox)
                            if(option.Value!=null)
                            if (option.Value != (string)option.DefaultValue)
                            {
                                stack.Push(new KeyValuePair<int, string>(i, option.Name + ": " + option.Value));
                                return true;
                            }
            bool t = false;
            if (option.childs != null)
                foreach (Options temp in option.childs)
                    if (WriteThisOption(temp, ref stack, i + 1))
                        t = true;
            if(t)
                stack.Push(new KeyValuePair<int, string>(i, option.Name + ": "));
            return t;
        }

        static string WriteStack(Stack<KeyValuePair<int, string>> stack)
        {
            string result = "";
            KeyValuePair<int, string> t;
            while (stack.Count>0)
            {
                t = stack.Pop();
                for (int i = 0; i < t.Key; i++)
                {
                    result += "  ";
                }
                result += t.Value+"\n";
            }
            return result;
        }
    }
}
