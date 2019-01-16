namespace casbinet.effect
{

    public interface Effector
    {
        bool mergeEffects(string expr, Effect[] effects, float[] results);
    }

}
