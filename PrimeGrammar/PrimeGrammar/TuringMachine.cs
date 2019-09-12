using System.Collections.Generic;

namespace PrimeGrammar
{
    public enum Move
    {
        Left, Right
    }
    
    public class State
    {
        public readonly string ID;

        public State(string id)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is State state)
            {
                return ID.Equals(state.ID);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }

    public class Transition
    {
        public readonly State From;

        public readonly State To;

        public readonly string Read;

        public readonly string Write;

        public readonly Move Movement;

        public Transition(State from, string read, State to, string write, Move movement)
        {
            From = from;
            Read = read;
            Write = write;
            To = to;
            Movement = movement;
        }
    }
    
    public class TuringMachine
    {
        public readonly State InitialState;

        public readonly List<State> States;

        public readonly List<Transition> Transitions;

        public readonly List<State> RingStates;

        public TuringMachine(State initialState, List<State> states, List<Transition> transitions, List<State> ringStates)
        {
            InitialState = initialState;
            States = states;
            Transitions = transitions;
            RingStates = ringStates;
        }
    }
}