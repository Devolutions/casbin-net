namespace casbinet.effect
{
    using System;

    public class DefaultEffector : Effector
    {

        public virtual bool mergeEffects(string expr, Effect[] effects, float[] results)
        {
            bool result;
            if (expr.Equals("some(where (p_eft == allow))"))
            {
                result = false;
                foreach (Effect eft in effects)
                {
                    if (eft == Effect.Allow)
                    {
                        result = true;
                        break;
                    }
                }
            }
            else if (expr.Equals("!some(where (p_eft == deny))"))
            {
                result = true;
                foreach (Effect eft in effects)
                {
                    if (eft == Effect.Deny)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else if (expr.Equals("some(where (p_eft == allow)) && !some(where (p_eft == deny))"))
            {
                result = false;
                foreach (Effect eft in effects)
                {
                    if (eft == Effect.Allow)
                    {
                        result = true;
                    }
                    else if (eft == Effect.Deny)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else if (expr.Equals("priority(p_eft) || deny"))
            {
                result = false;
                foreach (Effect eft in effects)
                {
                    if (eft != Effect.Indeterminate)
                    {
                        if (eft == Effect.Allow)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("unsupported effect");
            }

            return result;
        }
    }
}
