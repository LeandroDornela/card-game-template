using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace CardGameTemplate
{
    public static class ListExtensions
    {   
        // Fisher‑Yates Shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            System.Random random = new System.Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void DebugLog<T>(this IList<T> list)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Logging list of: {typeof(T)}");
            foreach (var element in list)
                sb.AppendLine(element?.ToString() ?? "null");

            CardGameTemplate.Debug.Log(Debug.Category.Temp, sb.ToString());
        }
    }
}