using System;

namespace Editor
{
    public class ReactiveProperty<TValue>
    {
        private TValue _value;
        
        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }

        public event Action Changed;
    }
}