namespace casbinet.persist
{
    using casbinet.Model;

    public class Watcher
    {
        public event Helper.LoadPolicyLineHandler<string, Model> OnUpdate;

        public void Update(string line, Model model)
        {
            this.OnUpdate?.Invoke(line, model);
        }
    }
}
