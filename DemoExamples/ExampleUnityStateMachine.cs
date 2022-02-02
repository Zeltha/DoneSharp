using System.Collections;
using DoneSharp.AI.State;
using UnityEngine;
using Random = System.Random;

namespace DemoExamples
{
    internal class ExampleUnityStateMachine : MonoBehaviour
    {
        private StateMachine<MyStates> _stateMachine;
        
        private readonly Random _randomNumberGenerator = new Random();
        
        private void Start()
        {
            _stateMachine = new StateMachine<MyStates>(); // You can optionally set a default state.
            
            // WARNING: using the following will not make you able to yield, meaning everything must happen that method call.
            
            _stateMachine.SetHandler(MyStates.None, (machine) =>
            {
                machine.State = MyStates.Idle;

                return null;
            });
            
            _stateMachine.SetHandler(MyStates.Idle, (machine) =>
            {
                if (_randomNumberGenerator.Next(0, 10) == 0)
                {
                    _stateMachine.State = MyStates.Wondering;
                }

                return null;
            });
            
            // Example of method based. This is the best usage.
            _stateMachine.SetHandler(MyStates.Wondering, MyWonderingMethod);
        }

        private void Update()
        {
            _stateMachine.Next();
        }
        
        private static IEnumerable MyWonderingMethod(StateMachine<MyStates> stateMachine)
        {
            // Do some movement

            yield return null; // Let the main thread continue.
                
            // Check if character is at destination

            yield return null;

            // Return the state machine to the 'none' state.
            stateMachine.State = MyStates.None;
        }

        internal enum MyStates
        {
            None,
            Idle,
            Wondering,
            Attacking
        }
    }
}