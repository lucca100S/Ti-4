using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMud.Utils
{
    public class Chance
    {
        public static int WeightedChance(params float[] chances)
        {
            float maxChance = 1;
            foreach (float chance in chances)
            {
                if (chance > maxChance)
                {
                    maxChance = chance;
                }
            }

            float randomValue = Random.Range(0f, 1f);
            int index = 0;
            for (int i = 1; i < chances.Length; i++)
            {
                if (randomValue <= (chances[i] / maxChance))
                {
                    float chance1 = (chances[i] / maxChance);
                    float chance2 = (chances[index] / maxChance);
                    if (chance1 > chance2)
                    {
                        index = i;
                    }
                    else if (chance1 == chance2)
                    {
                        index = randomValue > 0.5f ? i : index;
                    }
                }
            }

            return index;
        }
    }
}
