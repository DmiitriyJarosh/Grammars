using System.Collections.Generic;

namespace PrimeGrammar
{
    public enum Move
    {
        Left, Right
    }
    
    public class State
    {
        public readonly int ID;

        public State(int id)
        {
            ID = id;
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