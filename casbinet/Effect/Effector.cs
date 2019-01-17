using System;
using System.Collections.Generic;
using System.Text;

namespace casbinet.Effect
{
    public interface Effector
    {
        bool MergeEffects(string expr, Effect[] effects, float[] results);
    }
}
