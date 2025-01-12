using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lottery
{
    internal class Storage
    {
        public static void Save(string min, string max, string ignore, string quantity, bool checkbox, int fontSize)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "min", min },
                { "max", max },
                { "ignore", ignore },
                { "quantity", quantity },
                { "no duplication", checkbox.ToString() },
                { "mymessagebox fontsize", fontSize.ToString() }
            };

            using (StreamWriter writer = new StreamWriter(App.dataFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine("{");
                int count = data.Count;
                foreach (KeyValuePair<string, string> kvp in data)
                {
                    count--;
                    writer.Write($"  \"{kvp.Key}\": \"{kvp.Value}\"");
                    if (count > 0) writer.WriteLine(",");
                    else writer.WriteLine();
                }
                writer.WriteLine("}");
            }
        }

        public static (string, string, string, string, bool) Load()
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                using (StreamReader reader = new StreamReader(App.dataFilePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim().Trim('"');
                            string value = parts[1].Trim().Trim('"', ',');
                            data[key] = value;
                        }
                    }
                }
                if (data.ContainsKey("mymessagebox fontsize")) App.MyMessageBoxFontSize = int.Parse(data["mymessagebox fontsize"]);
                return (data.ContainsKey("min") ? data["min"] : string.Empty,
                data.ContainsKey("max") ? data["max"] : string.Empty,
                data.ContainsKey("ignore") ? data["ignore"] : string.Empty,
                data.ContainsKey("quantity") ? data["quantity"] : string.Empty,
                data.ContainsKey("no duplication") && bool.TryParse(data["no duplication"], out bool isChecked) && isChecked);
            }
            catch (Exception) { return ("", "", "", "", false); }
        }
    }
}
