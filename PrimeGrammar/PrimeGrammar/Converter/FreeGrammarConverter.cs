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
            List<string> alphabet = new List<string>(){"1"};
            
            // production 1
            // change the algo as we need to add blank before input
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A1")},
                new List<GrammarSymbol>()
                {
                    new Variable("[eps,Blank]"),
                    new Variable(turingMachine.InitialState.ID),
                    new Variable("A2"),
                    new Variable("[eps,Blank]")
                }
            ));
            
            Console.WriteLine("1/6 step finished");

            // production 2
            foreach (var symbol in alphabet)
            {
                productions.Add(new Production(
                    new List<GrammarSymbol>(){new Variable("A2")},
                    new List<GrammarSymbol>(){new Variable($"[{symbol},{symbol}]"), new Variable("A2")}
                ));
            }
            
            Console.WriteLine("2/6 step finished");

            // All blanks were added in 1st production so don't need A3 at all
            // production 5
            productions.Add(new Production(
                new List<GrammarSymbol>(){new Variable("A2")},
                new List<GrammarSymbol>(){new Terminal("eps")}
            ));
            
            Console.WriteLine("3/6 step finished");
            
            // states where blanks can appear
            List<State> statesWithBlanks = new List<State>()
            {
                new State("q100"),
                new State("q101"),
                new State("q102"),
                new State("q1")
            };
            
            alphabet.Add("eps");
            Console.WriteLine(productions.Count);
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

                    // filter productions with eps cells because we can use them only with blanks and #,$ symbols
                    if (a.Equals("eps") && !readSymbol.Equals("$") && !readSymbol.Equals("Blank")) 
                    {
                        continue;
                    }
                    
                    // we can't write these symbols in places of input!!
                    if (a.Equals("1") && (
                            readSymbol.Equals("#")
                            || readSymbol.Equals("$")
                            || readSymbol.Equals("Blank")
                            || writeSymbol.Equals("Blank")
                            || writeSymbol.Equals("#")
                            || writeSymbol.Equals("$")
                            ))
                    {
                        continue;
                    }

                    productions.Add(new Production(
                        new List<GrammarSymbol>(){new Variable(startState.ID), new Variable($"[{a},{readSymbol}]")},
                        new List<GrammarSymbol>(){new Variable($"[{a},{writeSymbol}]"), new Variable(finishState.ID)}
                    ));
                }
            }

            Console.WriteLine("4/6 step finished");

            FreeGrammarFilter filter = new FreeGrammarFilter();
            //rules for filtering
            filter.AddLeftContextFilter("q1", "q2", new List<string>(){"1", "$"});
            filter.AddLeftContextFilter("q2", "q3", new List<string>(){"$", "*", "1"});
            filter.AddLeftContextFilter("q5", "q6", new List<string>(){"$", "*", "1"});
            filter.AddLeftContextFilter("q6", "q6", new List<string>(){"$", "*", "1"});
            filter.AddLeftContextFilter("q8", "q9", new List<string>(){"*", "1"});
            filter.AddLeftContextFilter("q9", "q6", new List<string>(){"$", "*", "1"});
            filter.AddLeftContextFilter("q8", "q10", new List<string>(){"*", "1"});
            filter.AddLeftContextFilter("q10", "q3", new List<string>(){"$", "*", "1"});
            filter.AddLeftContextFilter("q13", "q14", new List<string>(){"$", "@", "1"});
            filter.AddLeftContextFilter("q14", "q14", new List<string>(){"$", "@", "1"});
            filter.AddLeftContextFilter("q15", "q16", new List<string>(){"1"});
            filter.AddLeftContextFilter("q16", "q14", new List<string>(){"$", "@", "1"});
            filter.AddLeftContextFilter("q13", "q14", new List<string>(){"$", "@", "1"});
            filter.AddLeftContextFilter("q17", "q3", new List<string>(){"$", "*", "1"});

            bool qEflag = true;
            bool qSflag = true;
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
                            // filter productions with eps cells because we can use them only with blanks and #,$ symbols
                            if (b.Equals("eps") && !E.Equals("Blank") && !E.Equals("$"))
                            {
                                continue;
                            }
                            
                            // filter productions with eps cells because we can use them only with blanks and #,$ symbols
                            if (a.Equals("eps") && !readSymbol.Equals("Blank") && !readSymbol.Equals("#"))
                            {
                                continue;
                            }
                            
                            // we can't write these symbols in places of input!!
                            if (a.Equals("1") && (
                                    readSymbol.Equals("#")
                                    || readSymbol.Equals("$")
                                    || readSymbol.Equals("Blank")
                                    || writeSymbol.Equals("Blank")
                                    || writeSymbol.Equals("#")
                                    || writeSymbol.Equals("$")
                                ))
                            {
                                continue;
                            }
                            
                            // we can't write these symbols in places of input!!
                            if (b.Equals("1") && (E.Equals("#") || E.Equals("$") || E.Equals("Blank")))
                            {
                                continue;
                            }

                            if (!statesWithBlanks.Contains(startState) && E.Equals("Blank"))
                            {
                                continue;
                            }
                            
                            // not important to move when finishing
                            if (finishState.ID.Equals("qE") || finishState.ID.Equals("qS"))
                            {
                                Production production = new Production(
                                    new List<GrammarSymbol>()
                                        {new Variable(startState.ID), new Variable($"[{a},{readSymbol}]")},
                                    new List<GrammarSymbol>()
                                        {new Variable(finishState.ID), new Variable($"[{a},{writeSymbol}]")}
                                );
                                
                                if (finishState.ID.Equals("qE") && qEflag)
                                { 
                                    qEflag = false;
                                    productions.Add(production);
                                }
                                if (finishState.ID.Equals("qS") && qSflag)
                                {
                                    qSflag = false;
                                    productions.Add(production);
                                }

                                continue;
                            }
                            
                            // clever filtering according rules which can be found before
                            if (filter.FilterLeftContext(startState, finishState, E))
                            {
                                continue;
                            }
                            
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
            
            Console.WriteLine("5/6 step finished");

            // production 8
            foreach (var q in turingMachine.RingStates)
            {
                foreach (var C in turingMachine.Alphabet)
                {
                    foreach (var a in alphabet)
                    {
                        // no blanks in result
                        if (C.Equals("Blank"))
                        {
                            continue;
                        }
                        
                        // eps cell can contain only #,$
                        if (a.Equals("eps") && !C.Equals("$") && !C.Equals("#"))
                        {
                            continue;
                        }

                        // not eps cell can't contain #.$'
                        if (a.Equals("1") && (C.Equals("$") || C.Equals("#")))
                        {
                            continue;
                        }
                        
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
            
            Console.WriteLine("6/6 step finished");

            return productions;
        }


        public Grammar Convert(TuringMachine turingMachine)
        {
            List<Production> productions = GetProductions(turingMachine);
            List<Terminal> terminals = new List<Terminal>()
            {
                //new Terminal("0"),
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