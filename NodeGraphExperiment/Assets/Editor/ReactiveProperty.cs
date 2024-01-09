using System;

namespace Editor
{
    public class ReactiveProperty<TValue>
    {
        private TValue _value;

        public ReactiveProperty() : this(default) { }
        
        public ReactiveProperty(TValue value) =>
            _value = value;
        
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