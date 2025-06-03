using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    public static List<T> Shuffle<T>(this List<T> input)
    {
        for (int i = input.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (input[i], input[j]) = (input[j], input[i]);
        }

        return input;
    }
}
