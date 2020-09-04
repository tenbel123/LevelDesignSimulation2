﻿using System.Collections.Generic;

public static class DictExtensions
{
    //https://kanchi0914.hatenablog.com/entry/2019/09/30/054419
    //確率の計算。行動のパターン
    public static T GetByRouletteSelection<T>(this Dictionary<T, int> dict)
    {
        var sum = 0;
        T selected = default;

        foreach (var key in dict.Keys)
        {
            sum += dict[key];
        }

        int tempSum = UnityEngine.Random.Range(0, sum);

        foreach (var key in dict.Keys)
        {
            tempSum -= dict[key];
            selected = key;
            if (tempSum < 0)
            {
                break;
            }
        }

        return selected;
    }
}

