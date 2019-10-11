using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace PrimeGrammar
{
    public class Generator
    {
        private const int MaxLength = 30;
        
        private const int MaxIterations = 1_000_0000;

        private Variable Eps;
        
        private Grammar Grammar;

        private string FilePath;

        private string WritePath;

        private HashSet<string> Used;

        public Generator(Grammar grammar, string filePath)
        {
            Grammar = grammar;
            FilePath = filePath;
            Eps = new Variable("eps");
            Used = new HashSet<string>();
            
            string startupPath = Environment.CurrentDirectory;
            WritePath = startupPath + "\\" + "numbers.txt";
        }

        public void Execute()
        {
            for (int i = 0; i < Grammar.Productions.Count; i++)
            {
                Production production = Grammar.Productions[i];
                // start from all productions which left part = Start variable
                if (production.LeftPart.Count == 1 
                    && production.LeftPart[0].Equals(Grammar.StartVariable))
                {
                    // TODO: Right part not epsilon!!!!
                    GenerateNumber(production.RightPart, i);
                }
            }
        }

        private void GenerateNumber(List<GrammarSymbol> list, int pr)
        {
            int count = 0;
            Queue<State> queue = new Queue<State>();
            queue.Enqueue(new State(list, pr));
            while (count != MaxIterations && queue.Count != 0)
            {
                count++;
                State state = queue.Dequeue();
                List<GrammarSymbol> workList = state.Symbols;
                TryWrite(state);

                for (int k = 0; k < Grammar.Productions.Count; k++)
                {
                    Production production = Grammar.Productions[k];
                    
                    for (int i = 0; i < workList.Count - production.LeftPart.Count + 1; i++)
                    {
                        bool contain = true;
                        for (int j = 0; j < production.LeftPart.Count && contain; j++)
                        {
                            if (!workList[j + i].Equals(production.LeftPart[j]))
                            {
                                contain = false;
                            }
                        }

                        if (contain)
                        {
                            List<GrammarSymbol> newList = new List<GrammarSymbol>();
                            for (int j = 0; j < i; j++)
                            {
                                newList.Add(workList[j]);
                            }

                            for (int j = 0; j < production.RightPart.Count; j++)
                            {
                                if (!production.RightPart[j].Equals(Eps))
                                {
                                    newList.Add(production.RightPart[j]);
                                }
                            }

                            for (int j = i + production.LeftPart.Count; j < workList.Count; j++)
                            {
                                newList.Add(workList[j]);
                            }

                            if (MaxLength >= newList.Count && Check(newList))
                            {
                                List<int> position = new List<int>();
                                foreach (var pos in state.Position)
                                {
                                    position.Add(pos);
                                }
                                position.Add(k);
                                queue.Enqueue(new State(newList, position));
                            }
                        }
                    }
                }
            }

            /*if (list.Count == MaxLength)
            {
                return;
            }
            
            foreach (var production in Grammar.Productions)
            {
                for (int i = 0; i < list.Count - production.LeftPart.Count + 1; i++)
                {
                    bool contain = true;
                    for (int j = 0; j < production.LeftPart.Count && contain; j++)
                    {
                        if (!list[j + i].Equals(production.LeftPart[j]))
                        {
                            contain = false;
                        }
                    }

                    if (contain)
                    {
                        List<GrammarSymbol> newList = new List<GrammarSymbol>();
                        for (int j = 0; j < i; j++)
                        {
                            newList.Add(list[j]);
                        }

                        for (int j = 0; j < production.RightPart.Count; j++)
                        {
                            if (!production.RightPart[j].Equals(Eps))
                            {
                                newList.Add(production.RightPart[j]);
                            }
                        }

                        for (int j = i + production.LeftPart.Count; j < list.Count; j++)
                        {
                            newList.Add(list[j]);
                        }

                        if (!TryWrite(newList) && newList.Count > list.Count)
                        {
                            GenerateNumber(newList);
                        }
                    }
                }
            }*/
        }

        private bool Check(List<GrammarSymbol> list)
        {
            string s = "";
            foreach (var item in list)
            {
                s += item.Name;
            }
            
            if (!Used.Contains(s))
            {
                Used.Add(s);
                return true;
            }

            return false;
        }
        
        private bool TryWrite(State state)
        {
            List<GrammarSymbol> list = state.Symbols;
            bool contain = true;
            foreach (var symb in list)
            {
                if (!Grammar.Terminals.Contains(new Terminal(symb.Name)))
                {
                    contain = false;
                    break;
                }
            }

            if (contain)
            {
                using (StreamWriter sw = new StreamWriter(WritePath, true, System.Text.Encoding.Default))
                {
                    String s = "";
                    foreach (var symb in list)
                    {
                        s += symb.Name;
                    }

                    sw.WriteLine(s);
                    for (int i = 0; i < state.Position.Count - 1; i++)
                    {
                        sw.WriteLine("        {0}", Grammar.Productions[state.Position[i]]);
                    }

                    sw.WriteLine("        {0}", Grammar.Productions[state.Position[state.Position.Count - 1]]);
                    Console.WriteLine(s);
                }
                return true;
            }

            return false;
        }

        private class State
        {
            public List<int> Position { get; }
            
            public List<GrammarSymbol> Symbols { get; }

            public State(List<GrammarSymbol> symbols, List<int> position)
            {
                Position = position;
                Symbols = symbols;
            }
            
            public State(List<GrammarSymbol> symbols, int pos)
            {
                Position = new List<int>();
                Position.Add(pos);
                Symbols = symbols;
            }
        }
    }
}