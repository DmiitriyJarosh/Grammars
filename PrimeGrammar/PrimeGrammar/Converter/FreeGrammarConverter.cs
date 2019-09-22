using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeGrammar.Converter
{
    public class FreeGrammarConverter : IConverter
    {
        public List<Production> GetProductions(TuringMachine turingMachine)
        {
            List<Production> productions = new List<Production>();
            List<string> alphabet = new List<string>(){"0", "1"};
            
            // production 1
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A1")},
                new List<GrammarSymbol>(){new Variable(turingMachine.InitialState.ID), new Variable("A2")}
            ));
            
            Console.WriteLine("1/8 step finished");

            // production 2
            foreach (var symbol in alphabet)
            {
                productions.Add(new Production(
                    new List<GrammarSymbol>(){new Variable("A2")},
                    new List<GrammarSymbol>(){new Variable($"[{symbol},{symbol}]"), new Variable("A2")}
                ));
            }
            
            Console.WriteLine("2/8 step finished");
            
            // production 3
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A2")},
                new List<GrammarSymbol>(){new Variable("A3")}
            ));
            
            Console.WriteLine("3/8 step finished");
            
            // production 4
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A3")},
                new List<GrammarSymbol>(){new Terminal("[eps,Blank]"), new Variable("A3")}
            ));
            
            Console.WriteLine("4/8 step finished");
            
            // production 5
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A3")},
                new List<GrammarSymbol>(){new Terminal("eps")}
            ));
            
            Console.WriteLine("5/8 step finished");
            
            alphabet.Add("eps");
            // production 6
            foreach (var a in alphabet)
            {
                foreach (var transition in turingMachine.Transitions)
                {
                    State startState = transition.From;
                    State finishState = transition.To;
                    String readSymbol = transition.Read;
                    String writeSymbol = transition.Write;
                    if (transition.Movement == Move.Left)
                    {
                        continue;
                    }
                    
                    productions.Add(new Production(
                        new List<GrammarSymbol>(){new Variable(startState.ID), new Variable($"[{a},{readSymbol}]")},
                        new List<GrammarSymbol>(){new Variable($"[{a},{writeSymbol}]"), new Variable(finishState.ID)}
                    ));
                }
            }
            
            Console.WriteLine("6/8 step finished");
            
            // production 7
            foreach (var a in alphabet)
            {
                foreach (var b in alphabet)
                {
                    foreach (var transition in turingMachine.Transitions)
                    {
                        State startState = transition.From;
                        State finishState = transition.To;
                        String readSymbol = transition.Read;
                        String writeSymbol = transition.Write;
                        if (transition.Movement == Move.Right)
                        {
                            continue;
                        }
                        
                        foreach (var E in turingMachine.Alphabet)
                        {
                            productions.Add(new Production(
                                new List<GrammarSymbol>()
                                    {new Variable($"[{b},{E}]"), new Variable(startState.ID), new Variable($"[{a},{readSymbol}]")},
                                new List<GrammarSymbol>()
                                    {new Variable(finishState.ID), new Variable($"[{b},{E}]"), new Variable($"[{a},{writeSymbol}]")}
                            ));
                        }
                    }
                }
            }
            
            Console.WriteLine("7/8 step finished");
            
            // production 8
            foreach (var q in turingMachine.RingStates)
            {
                foreach (var C in turingMachine.Alphabet)
                {
                    foreach (var a in alphabet)
                    {
                        productions.Add(new Production(
                            new List<GrammarSymbol>(){new Variable($"[{a},{C}]"), new Variable(q.ID)},
                            new List<GrammarSymbol>(){new Variable(q.ID), new Terminal(a), new Variable(q.ID)}
                        ));
                        
                        productions.Add(new Production(
                            new List<GrammarSymbol>(){new Variable(q.ID), new Variable($"[{a},{C}]")},
                            new List<GrammarSymbol>(){new Variable(q.ID), new Terminal(a), new Variable(q.ID)}
                        ));
                    }
                }
                
                productions.Add(new Production(
                    new List<GrammarSymbol>(){new Variable(q.ID)},
                    new List<GrammarSymbol>(){new Terminal("eps")}
                ));
            }
            
            Console.WriteLine("8/8 step finished");

            return productions;
        }


        public Grammar Convert(TuringMachine turingMachine)
        {
            List<Production> productions = GetProductions(turingMachine);
            List<Terminal> terminals = new List<Terminal>()
            {
                new Terminal("0"),
                new Terminal("1"),
                new Terminal("eps")
            };
            HashSet<Variable> variables = new HashSet<Variable>();
            foreach (var production in productions)
            {
                foreach (var elem in production.LeftPart)
                {
                    if (elem is Variable nonterminal)
                    {
                        variables.Add(nonterminal);
                    }
                }
                foreach (var elem in production.RightPart)
                {
                    if (elem is Variable nonterminal)
                    {
                        variables.Add(nonterminal);
                    }
                }
            }
            return new Grammar(new Variable("A1"), productions, terminals, variables.ToList());
        }
    }
}