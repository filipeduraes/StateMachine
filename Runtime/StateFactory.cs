using System;
using System.Collections.Generic;

namespace IdeaToGame.HFSM
{
    public class StateFactory<T> where T : StateMachine<T>
    {
        private readonly Dictionary<Type, State<T>> states = new();

        public StateFactory(T stateMachine)
        {
            Type[] stateTypes = typeof(T).Assembly.GetTypes();

            foreach (Type stateType in stateTypes)
            {
                if (stateType.IsSubclassOf(typeof(State<T>)) && !stateType.IsAbstract)
                {
                    State<T> state = Activator.CreateInstance(stateType, stateMachine as object) as State<T>;
                    states[stateType] = state;
                }
            }
        }

        public State<T> GetState<TState>() where TState : State<T>
        {
            Type stateType = typeof(TState);
            return states[stateType];
        }

        #region State Path

        private State<T> GetState(Type stateType)
        {
            return states[stateType];
        }

        public List<State<T>> GetStatePath(State<T> state)
        {
            List<State<T>> result = new();
            FindStatePath(state.GetType(), result);
            return result;
        }

        private void FindStatePath(Type stateType, ICollection<State<T>> result)
        {
            State<T> state = GetState(stateType);
            result.Add(state);
            
            if(state.ParentState != null)
                FindStatePath(state.ParentState, result);
        }
        
        #endregion
    }
}