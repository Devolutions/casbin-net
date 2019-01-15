namespace casbinet.persist
{
    using System.Collections.Generic;
    using System.Threading;

    //import org.casbin.jcasbin.model.Model;

    public interface Watcher
    {
        void setUpdateCallback(Thread runnable); 

        void update();

    }

    
}
