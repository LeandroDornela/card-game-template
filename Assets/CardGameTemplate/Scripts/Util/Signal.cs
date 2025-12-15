using System;

namespace CardGameTemplate
{
    // Signals wrappers with multiple number of parameters.
    public class Signal
    {
        // NOTE: One possible declaration is "private event Action" event prevents outside manipulation of the
        // Delegate/Action is its public, since in that case is private, there is no need for "event"
        private Action _listeners;

        public void AddListener(Action listener) => _listeners += listener;
        public void RemoveListener(Action listener) => _listeners -= listener;

        public void Trigger()
        {
            Debug.Log(Debug.Category.Events, $"{nameof(Signal)} triggered.");
            _listeners?.Invoke(); // If no listeners(_listeners == null) don't invoke.
        } 
    }

    public class Signal<T>
    {
        private Action<T> _listeners;

        public void AddListener(Action<T> listener) => _listeners += listener;
        public void RemoveListener(Action<T> listener) => _listeners -= listener;

        public void Trigger(T value)
        {
            Debug.Log(Debug.Category.Events, $"{nameof(Signal)},{typeof(T)} triggered.");
            _listeners?.Invoke(value);
        }
    }

    public class Signal<T1, T2>
    {
        private Action<T1, T2> _listeners;

        public void AddListener(Action<T1, T2> listener) => _listeners += listener;
        public void RemoveListener(Action<T1, T2> listener) => _listeners -= listener;

        public void Trigger(T1 value1, T2 value2)
        {
            Debug.Log(Debug.Category.Events, $"{nameof(Signal)},{typeof(T1)},{typeof(T2)} triggered.");
            _listeners?.Invoke(value1, value2);
        }
    }

    public class Signal<T1, T2, T3>
    {
        private Action<T1, T2, T3> _listeners;

        public void AddListener(Action<T1, T2, T3> listener) => _listeners += listener;
        public void RemoveListener(Action<T1, T2, T3> listener) => _listeners -= listener;

        public void Trigger(T1 value1, T2 value2, T3 value3)
        {
            Debug.Log(Debug.Category.Events, $"{nameof(Signal)},{typeof(T1)},{typeof(T2)},{typeof(T3)} triggered.");
            _listeners?.Invoke(value1, value2, value3);
        }
    }
}