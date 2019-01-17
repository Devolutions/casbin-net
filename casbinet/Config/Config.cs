namespace casbinet.Config
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq.Expressions;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading;

    public class Config
    {
        private const string DEFAULT_SECTION = "default";

        private const string DEFAULT_COMMENT = "#";

        private const string DEFAULT_COMMENT_SEM = ";";

        private Mutex mutex = new Mutex();

        private Dictionary<(string, string), string> data = new Dictionary<(string, string), string>();

        private Config()
        {
            //this.data = new Tuple<string, string, string>();
        }

        public static Config NewConfig(string confName)
        {
            Config c = new Config();
            c.Parse(confName);
            return c;
        }

        public static Config NewConfigFromText(string text)
        {
            Config c = new Config();
            c.ParseText(new StreamReader(text));
            return c;

        }

        private bool AddConfig(string section, string option, string value)
        {
            if (section.Equals(string.Empty))
            {
                section = DEFAULT_SECTION;
            }

            (string, string) key = (section, option);

            if (this.data.ContainsKey(key))
            {
                this.data[key] = value;
                return false;
            }

            this.data.Add(key,value);
            return true;
        }

        private void Parse(string fname)
        {
            lock (this.mutex)
            {
                StreamReader reader;
                try
                {
                    reader = new StreamReader(File.OpenRead(fname));
                    this.ParseText(reader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void ParseText(TextReader reader)
        {
            string section = string.Empty;
            string line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                line = line.Trim();

                if (line.StartsWith(DEFAULT_COMMENT) || line.StartsWith(DEFAULT_COMMENT_SEM))
                {
                    continue;
                }

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line.Substring(1, line.Length - 1);
                    continue;
                }

                string[] optionVal = line.Split("=", 2);
                if (optionVal.Length != 2)
                {
                    throw new Exception("Error parsing the content error");
                }

                string option = optionVal[0].Trim();
                string value = optionVal[1].Trim();
                this.AddConfig(section, option, value);
            }
        }

        public bool GetBool(string key)
        {
            return bool.Parse(this.Get(key));
        }

        public int GetInt(string key)
        {
            return int.Parse(this.Get(key));
        }

        public float GetFloat(string key)
        {
            return float.Parse(this.Get(key));
        }

        public string GetString(string key)
        {
            return this.Get(key);
        }

        public void Set(string key, string value)
        {
            lock (this.mutex)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new Exception("Key is empty");
                }

                string section = "";
                string option;
                string[] keys = key.ToLower().Split("::");
                if (keys.Length >= 2)
                {
                    section = keys[0];
                    option = keys[1];
                }
                else
                {
                    option = keys[0];
                }

                this.AddConfig(section, option, value);
            }
        }

        public string Get(string key)
        {
            string section;
            string option;

            string[] keys = key.ToLower().Split("::");
            if (keys.Length >= 2)
            {
                section = keys[0];
                option = keys[1];
            }
            else
            {
                section = DEFAULT_SECTION;
                option = keys[0];
            }

            (string, string) keyTuple = (section, option);
            if (this.data.ContainsKey(keyTuple))
            {
                this.data.TryGetValue(keyTuple, out string value);
                return value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
