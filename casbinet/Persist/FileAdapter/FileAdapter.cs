namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
     
    using casbinet.Model;
    using casbinet.Util;

    public class FileAdapter : Adapter
    {
        private string filePath;

        public event Helper.LoadPolicyLineHandler<string, Model> OnAcceptedPolicy;

        public FileAdapter(string filePath)
        {
            this.filePath = filePath;
        }

        public virtual void LoadPolicy(Model model)
        {
            if (string.IsNullOrEmpty(this.filePath))
            {
                throw new Exception("Invalid file path, file path cannot be empty");
            }

            this.LoadPolicyFile(model, Helper.LoadPolicyLine);  
        }

        public virtual void SavePolicy(Model model)
        {
            if (filePath.Equals(""))
            {
                throw new Exception("invalid file path, file path cannot be empty");
            }

            StringBuilder tmp = new StringBuilder();

            if (model.model.TryGetValue("p", out Dictionary<string, Assertion> pAssertionsDictionary))
            {
                foreach (KeyValuePair<string, Assertion> entry in pAssertionsDictionary)
                {
                    string pType = entry.Key;
                    Assertion ast = entry.Value;

                    foreach (List<string> rule in ast.Policy)
                    {
                        tmp.Append(pType + ", ");
                        tmp.Append(Util.ArrayToString(rule));
                        tmp.Append("\n");
                    }
                }
            }

            if (model.model.TryGetValue("g", out Dictionary<string, Assertion> gAssertionsDictionary))
            {
                foreach (KeyValuePair<string, Assertion> entry in gAssertionsDictionary)
                {
                    string pType = entry.Key;
                    Assertion ast = entry.Value;

                    foreach (List<string> rule in ast.Policy)
                    {
                        tmp.Append(pType + ", ");
                        tmp.Append(Util.ArrayToString(rule));
                        tmp.Append("\n");
                    }
                }
            }

            this.SavePolicyFile(tmp.ToString().Trim());
        }

        private void LoadPolicyFile(Model model, Helper.LoadPolicyLineHandler<string, Model> handler)
        {
            StreamReader reader;
            try
            {
                reader = new StreamReader(File.OpenRead(this.filePath));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("policy file not found");
            }

            string line;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    this.OnAcceptedPolicy?.Invoke(line, model);
                }

                reader.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("IO error occurred");
            }
        }

        private void SavePolicyFile(string text)
        {
            try
            {
                StreamWriter writer = new StreamWriter(File.OpenWrite(this.filePath));
                writer.Write(Encoding.ASCII.GetBytes(text));
                writer.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("IO error occurred");
            }
        }

        public virtual void AddPolicy(string sec, string ptype, IList<string> rule)
        {
            throw new Exception("not implemented");
        }

        public virtual void RemovePolicy(string sec, string ptype, IList<string> rule)
        {
            throw new Exception("not implemented");
        }

        public virtual void RemoveFilteredPolicy(string sec, string ptype, int fieldIndex, params string[] fieldValues)
        {
            throw new Exception("not implemented");
        }



    }
}
