using System.Collections.Generic;

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

            // production 2
            foreach (var symbol in alphabet)
            {
                productions.Add(new Production(
                    new List<GrammarSymbol>(){new Variable("A2")},
                    new List<GrammarSymbol>(){new Variable($"[{symbol},{symbol}]"), new Variable("A2")}
                ));
            }
            
            // production 3
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A2")},
                new List<GrammarSymbol>(){new Variable("A3")}
            ));
            
            // production 4
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A3")},
                new List<GrammarSymbol>(){new Terminal("[eps,Blank]"), new Variable("A3")}
            ));
            
            // production 5
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A3")},
                new List<GrammarSymbol>(){new Terminal("eps")}
            ));
            
            alphabet.Add("eps");
            // production 6
            foreach (var a in alphabet)
            {
                foreach (var startState in turingMachine.States)
                {
                    foreach (var finishState in turingMachine.States)
                    {
                        foreach (var readSymbol in turingMachine.Alphabet)
                        {
                            foreach (var writeSymbol in turingMachine.Alphabet)
                            {
                                Transition transition = new Transition(
                                    startState,
                                    readSymbol,
                                    finishState,
                                    writeSymbol,
                                    Move.Right
                                );

                                if (turingMachine.Transitions.Contains(transition))
                                {
                                    productions.Add(new Production(
                                        new List<GrammarSymbol>(){new Variable(startState.ID), new Variable($"[{a},{readSymbol}]")},
                                        new List<GrammarSymbol>(){new Variable($"[{a},{writeSymbol}]"), new Variable(finishState.ID)}
                                    ));
                                }
                            }
                        }
                    }
                }
            }
            
            // production 7
            foreach (var a in alphabet)
            {
                foreach (var b in alphabet)
                {
                    foreach (var startState in turingMachine.States)
                    {
                        foreach (var finishState in turingMachine.States)
                        {
                            foreach (var readSymbol in turingMachine.Alphabet)
                            {
                                foreach (var writeSymbol in turingMachine.Alphabet)
                                {
                                    foreach (var E in turingMachine.Alphabet)
                                    {
                                        Transition transition = new Transition(
                                            startState,
                                            readSymbol,
                                            finishState,
                                            writeSymbol,
                                            Move.Left
                                        );

                                        if (turingMachine.Transitions.Contains(transition))
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
                        }
                    }
                }
            }
            
            // production 8
            foreach (var a in alphabet)
            {
                foreach (var C in turingMachine.Alphabet)
                {
                    foreach (var q in turingMachine.RingStates)
                    {
                        productions.Add(new Production(
                            new List<GrammarSymbol>(){new Variable($"[{a},{C}]"), new Variable(q.ID)},
                            new List<GrammarSymbol>(){new Variable(q.ID), new Terminal(a), new Variable(q.ID)}
                        ));
                        
                        productions.Add(new Production(
                            new List<GrammarSymbol>(){new Variable(q.ID), new Variable($"[{a},{C}]")},
                            new List<GrammarSymbol>(){new Variable(q.ID), new Terminal(a), new Variable(q.ID)}
                        ));
                        
                        productions.Add(new Production(
                            new List<GrammarSymbol>(){new Variable(q.ID)},
                            new List<GrammarSymbol>(){new Terminal("eps")}
                        ));
                    }
                }
            }

            return productions;
        }


        public Grammar Convert(TuringMachine turingMachine)
        {
            List<Production> productions = GetProductions(turingMachine);
            return new Grammar(new Variable("A1"), productions, null, null);
        }
    }
}