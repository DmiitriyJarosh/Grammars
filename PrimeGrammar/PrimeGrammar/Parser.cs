using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PrimeGrammar.Exceptions;

namespace PrimeGrammar
{
    public static class Parser
    {
        public static Move MoveFromString(string input)
        {
            switch (input)
            {
                case "l":
                case "L":
                    return Move.Left;
                case "r":
                case "R":
                    return Move.Right;
                default:
                    throw new MoveParseException(input);
            }
        }
        
        // Remember, file start with string: startMachineState ringStates
        // ringStates should be divided by spaces
        // Parsing format: startState,readSymbol,toState,writeSymbol,movement
        // in every line
        public static TuringMachine ParseFromTuple(string path)
        {
            using (StreamReader input = new StreamReader(path))
            {
                HashSet<State> states = new HashSet<State>();
                HashSet<string> alphabet = new HashSet<string>();
                List<Transition> transitions = new List<Transition>();
                List<State> ringStates = new List<State>();
                State startState;
                
                string line = input.ReadLine();
                if (line == null)
                {
                    throw new ParseException("No lines in input file");
                }
                else
                {
                    if (line.EndsWith("\n"))
                    {
                        line = line.Substring(0, line.Length - 1);
                    }

                    string[] elems = line.Split(' ');
                    startState = new State(elems[0]);
                    for (int i = 1; i < elems.Length; i++)
                    {
                        ringStates.Add(new State(elems[i]));
                    }
                }
                
                line = input.ReadLine();
                
                while (line != null)
                {
                    if (line.EndsWith("\n"))
                    {
                        line = line.Substring(0, line.Length - 1);
                    }

                    string[] elems = line.Split(',');
                    
                    State from = new State(elems[0]);
                    State to = new State(elems[2]);

                    string read = elems[1];
                    string write = elems[3];

                    Move movement = MoveFromString(elems[4]);
                    
                    Transition transition = new Transition(from, read, to, write, movement);
                    
                    transitions.Add(transition);
                    states.Add(from);
                    states.Add(to);
                    alphabet.Add(read);
                    alphabet.Add(write);
                    
                    line = input.ReadLine();
                }
                
                return new TuringMachine(startState, states.ToList(), transitions, ringStates);
            }
        }

        // Remember, file start with string: startMachineState ringStates
        // ringStates should be divided by spaces
        // Parsing format: readSymbol,startState -> writeSymbol,toState,movement
        // in every line
        // Commas (,) are not included in file!!!!!
        
        public static TuringMachine ParseFromArrow(string path)
        {
            using (StreamReader input = new StreamReader(path))
            {
                HashSet<State> states = new HashSet<State>();
                List<Transition> transitions = new List<Transition>();
                List<State> ringStates = new List<State>();
                HashSet<string> alphabet = new HashSet<string>();
                State startState;
                
                string line = input.ReadLine();
                if (line == null)
                {
                    throw new ParseException("No lines in input file");
                }
                else
                {
                    if (line.EndsWith("\n"))
                    {
                        line = line.Substring(0, line.Length - 1);
                    }

                    string[] elems = line.Split(' ');
                    startState = new State(elems[0]);
                    for (int i = 1; i < elems.Length; i++)
                    {
                        ringStates.Add(new State(elems[i]));
                    }
                }

                line = input.ReadLine();
                
                while (line != null)
                {
                    if (line.StartsWith("//") || line.StartsWith(" ") || line.Equals(""))
                    {
                        line = input.ReadLine();
                        continue;
                    }
                    
                    if (line.EndsWith("\n"))
                    {
                        line = line.Substring(0, line.Length - 1);
                    }

                    string[] elems = line.Split("->".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    
                    State from = new State(elems[0].Substring(1));
                    State to = new State(elems[1].Substring(1, elems[1].Length - 2));

                    string read = elems[0][0].ToString();
                    string write = elems[1][0].ToString();

                    Move movement = MoveFromString(elems[1][elems[1].Length - 1].ToString());
                    
                    Transition transition = new Transition(from, read, to, write, movement);
                    
                    transitions.Add(transition);
                    states.Add(from);
                    states.Add(to);
                    alphabet.Add(read);
                    alphabet.Add(write);
                    
                    line = input.ReadLine();
                }
                
                return new TuringMachine(startState, states.ToList(), transitions, ringStates);
            }
        }
    }
}