namespace casbinet.Effect
{
    public interface IEffector
    {
        bool MergeEffects(string expr, Effect[] effects, float[] results);
    }
}
