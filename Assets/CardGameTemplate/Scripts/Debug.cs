using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGameTemplate
{
    public class Debug
    {
        public enum Category
        {
            Temp,
            Data,
            Networking,
            GameLogic,
            GameState,
            Events,
            UI
        }

        private static readonly bool[] _enabled =
        {
            true, true, true, true, true, true
        };

        private static readonly string[] _categoryHex = new string[]
        {
            ColorUtility.ToHtmlStringRGB(new Color(0.7f, 0.7f, 0.7f)), // Temp
            ColorUtility.ToHtmlStringRGB(new Color(0.0f, 0.6f, 1.0f)), // Data
            ColorUtility.ToHtmlStringRGB(new Color(0.2f, 0.9f, 0.9f)), // Networking
            ColorUtility.ToHtmlStringRGB(new Color(0.1f, 0.8f, 0.1f)), // GameLogic
            ColorUtility.ToHtmlStringRGB(new Color(1.0f, 0.6f, 0.0f)), // GameState
            ColorUtility.ToHtmlStringRGB(new Color(1.0f, 0.6f, 1.0f)), // Game Events/Signals
            ColorUtility.ToHtmlStringRGB(new Color(1.0f, 0.3f, 0.8f)), // UI
        };

        private static string Wrap(Category category, string text)
        {
            return $"<color=#{_categoryHex[(int)category]}>[{category}]</color> {text}";
        }

        public static void SetEnabled(Category category, bool enabled)
        {
            _enabled[(int)category] = enabled;
        }

        public static void Log(Category category, string text)
        {
            if (!_enabled[(int)category]) return;

            UnityEngine.Debug.Log(Wrap(category, text));
        }

        public static void LogWarning(Category category, string text)
        {
            if (!_enabled[(int)category]) return;

            UnityEngine.Debug.LogWarning(Wrap(category, text));
        }

        public static void LogError(Category category, string text)
        {
            if (!_enabled[(int)category]) return;

            UnityEngine.Debug.LogError(Wrap(category, text));
        }
    }
}
