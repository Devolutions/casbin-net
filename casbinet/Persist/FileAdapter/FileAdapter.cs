namespace casbinet.persist.FileAdapter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    //import org.casbin.jcasbin.model.Assertion;
    //import org.casbin.jcasbin.model.Model;
    //import org.casbin.jcasbin.persist.Adapter;
    //import org.casbin.jcasbin.persist.Helper;
    //import org.casbin.jcasbin.util.Util;

    public class FileAdapter : Adapter
    {
        private String filePath;

        public FileAdapter(String filePath)
        {
            this.filePath = filePath;
        }

        public virtual void loadPolicy(Model model)
        {
            if (filePath == "")
            {
                // throw new Error("invalid file path, file path cannot be empty");
                return;
            }

            loadPolicyFile(model, Helper.loadPolicyLine());
        }

        public virtual void savePolicy(Model model)
        {
            if (filePath == "")
            {
                throw new FileNotFoundException("invalid file path, file path cannot be empty"); 
            }

            StringBuilder tmp = new StringBuilder();

            for (Map.Entry<String, Assertion> entry : model.model.get("p").entrySet())
            {
                String ptype = entry.getKey();
                Assertion ast = entry.getValue();

                for (List<String> rule : ast.policy)
                {
                    tmp.append(ptype + ", ");
                    tmp.append(Util.arrayToString(rule));
                    tmp.append("\n");
                }
            }

            for (Map.Entry<String, Assertion> entry : model.model.get("g").entrySet())
            {
                String ptype = entry.getKey();
                Assertion ast = entry.getValue();

                for (List<String> rule : ast.policy)
                {
                    tmp.append(ptype + ", ");
                    tmp.append(Util.arrayToString(rule));
                    tmp.append("\n");
                }
            }
            savePolicyFile(tmp.toString().trim());
        }


        private void loadPolicyFile(Model model, Helper.loadPolicyLineHandler<String, Model> handler)
        {
            FileInputStream fis;
            try
            {
                fis = new FileInputStream(filePath);
            }
            catch (FileNotFoundException e)
            {
                e.printStackTrace();
                throw new Error("policy file not found");
            }
            BufferedReader br = new BufferedReader(new InputStreamReader(fis));

            String line;
            try
            {
                while ((line = br.readLine()) != null)
                {
                    handler.accept(line, model);
                }

                fis.close();
                br.close();
            }
            catch (IOException e)
            {
                e.printStackTrace();
                throw new Error("IO error occurred");
            }
        }

        private void savePolicyFile(String text)
        {
            try
            {
                FileOutputStream fos = new FileOutputStream(filePath);
                fos.write(text.getBytes());
                fos.close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                throw new FileNotFoundException("IO error occurred");
            }
        }

        public virtual void addPolicy(String sec, String ptype, List<String> rule)
        {
            throw new FileNotFoundException("not implemented");
        }

        public virtual void removePolicy(String sec, String ptype, List<String> rule)
        {
            throw new FileNotFoundException("not implemented");
        }

        public virtual void removeFilteredPolicy(String sec, String ptype, int fieldIndex, params object[] fieldValues)
        {
            throw new FileNotFoundException("not implemented");
        }



    }
}
