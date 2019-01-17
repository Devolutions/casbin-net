using System;
using System.Collections.Generic;
using System.Text;

namespace casbinet.Effect
{
    public class DefaultEffector : Effector
    {

        public DefaultEffector()
        {

        }

        public bool MergeEffects(string expr, Effect[] effects, float[] results)
        {
            bool result;

            if (expr.Equals("some(where (p_eft == allow))"))
            {
                result = false;
                foreach (Effect effect in effects)
                {
                    if (effect == Effect.Allow)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }

            if (expr.Equals("!some(where (p_eft == deny))"))
            {
                result = true;
                foreach (Effect effect in effects)
                {
                    if (effect == Effect.Deny)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }

            if (expr.Equals("some(where (p_eft == allow)) && !some(where (p_eft == deny))"))
            {
                result = false;
                foreach (Effect effect in effects)
                {
                    if (effect == Effect.Allow)
                    {
                        result = true;
                    }
                    else if (effect == Effect.Deny)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }

            if (expr.Equals("priority(p_eft) || deny"))
            {
                result = false;
                foreach (Effect effect in effects)
                {
                    if (effect == Effect.Indetermiante)
                    {
                        break;
                    }

                    if (effect == Effect.Allow)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }

                return result;
            }
            else
            {
                throw new Exception("Unsupported effect");
            }
        }
    }
}
