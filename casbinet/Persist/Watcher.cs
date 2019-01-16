namespace casbinet.persist
{
    using System.Collections.Generic;
    using System.Threading;

    public interface Watcher 
    {
        ThreadStart UpdateCallback { set; }

        void update();

    }
}
