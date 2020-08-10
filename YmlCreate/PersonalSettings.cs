using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YmlCreate
{
    static class PersonalSettings
    {
        public static Settings appConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("appsettings.json"));
        public static void Save()
        {
            File.WriteAllText("appsettings.json", JsonConvert.SerializeObject(appConfig, Newtonsoft.Json.Formatting.Indented));
        }
    }
    public class Settings
    {
        public int MainWindowHeight { get; set; }
        public int MainWindowWidth { get; set; }
        public string LastChoosedVersion { get; set; }
        public string[] ComposeFileVerions { get; set; }
    }
}
