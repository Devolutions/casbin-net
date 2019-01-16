namespace casbinet.persist.FileAdapter
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Security.Cryptography;

    using Assertion = casbinet.model.Assertion;
	using Model = casbinet.model.Model;
	using Util = casbinet.util.Util;

    public class FileAdapter : Adapter
    {
        private string filePath;

        public FileAdapter(string filePath)
        {
            this.filePath = filePath;
        }

        public virtual void loadPolicy(Model model)
        {
            if (filePath == string.Empty)
            {
                throw new Exception("invalid file path, file path cannot be empty");
                return;
            }

            loadPolicyFile(model, Helper.loadPolicyLine);
        }

        public virtual void savePolicy(Model model)
        {
            if (filePath.Equals(""))
            {
                throw new Exception("invalid file path, file path cannot be empty");
            }

            StringBuilder tmp = new StringBuilder();

            foreach (KeyValuePair<string, Assertion> entry in model.model["p"].SetOfKeyValuePairs())
            {
                string ptype = entry.Key;
                Assertion ast = entry.Value;

                foreach (IList<string> rule in ast.policy)
                {
                    tmp.Append(ptype + ", ");
                    tmp.Append(Util.arrayToString(rule));
                    tmp.Append("\n");
                }
            }

            foreach (KeyValuePair<string, Assertion> entry in model.model["g"].SetOfKeyValuePairs())
            {
                string ptype = entry.Key;
                Assertion ast = entry.Value;

                foreach (IList<string> rule in ast.policy)
                {
                    tmp.Append(ptype + ", ");
                    tmp.Append(Util.arrayToString(rule));
                    tmp.Append("\n");
                }
            }

            savePolicyFile(tmp.ToString().Trim());
        }


        private void loadPolicyFile(Model model, Helper.loadPolicyLineHandler<string, Model> handler)
        {
            FileStream fis;
            try
            {
                fis = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("policy file not found");
            }
            StreamReader br = new StreamReader(fis);

            string line;
            try
            {
                while (!string.ReferenceEquals((line = br.ReadLine()), null))
                {
                    handler.accept(line, model);
                }

                fis.Close();
                br.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("IO error occurred");
            }
        }

        private void savePolicyFile(char[] text)
        {
            try
            {
                //Byte[] byteArray = Encoding.UTF8.GetBytes(text);
                FileStream fos = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fos.WriteByte(text);
                fos.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception("IO error occurred");
            }
        }

        public virtual void addPolicy(string sec, string ptype, IList<string> rule)
        {
            throw new Exception("not implemented");
        }

        public virtual void removePolicy(string sec, string ptype, IList<string> rule)
        {
            throw new Exception("not implemented");
        }

        public virtual void removeFilteredPolicy(string sec, string ptype, int fieldIndex, params string[] fieldValues)
        {
            throw new Exception("not implemented");
        }



    }
}
