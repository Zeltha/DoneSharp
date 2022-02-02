using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DoneSharp.AI.State
{
    public class StateMachine <T> where T : Enum
    {
        public delegate IEnumerable StateMachineHandler(StateMachine<T> stateMachine);
        
        private T _state;

        private readonly Dictionary<T, StateMachineHandler> _stateHandlers;

        private IEnumerator _currentEnumerable;

        public T State
        {
            get => _state;

            set
            {
                _state = value;

                _currentEnumerable = _stateHandlers[value](this).GetEnumerator();
            } 
        }

        public StateMachine()
        {
            var values = GetEnumValues();

            _stateHandlers = new Dictionary<T, StateMachineHandler>();

            foreach (T value in values)
            {
                _stateHandlers.Add(value, null);
            }
        }

        public object Next()
        {
            _currentEnumerable?.MoveNext();

            return _currentEnumerable?.Current;
        }

        public void SetHandler(T state, StateMachineHandler handler)
        {
            _stateHandlers[state] = handler;
        }
        
        private static IEnumerable<T> GetEnumValues() {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}