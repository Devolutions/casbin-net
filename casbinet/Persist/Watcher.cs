namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public interface Watcher
    {
        void update();
    }
}
