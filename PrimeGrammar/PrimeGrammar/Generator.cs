using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace PrimeGrammar
{
    public class Generator
    {
        // Length of number in binary <= 10
        private const int MaxLength = 10;

        private Variable Eps;
        
        private Grammar Grammar;

        private string FilePath;

        public Generator(Grammar grammar, string filePath)
        {
            Grammar = grammar;
            FilePath = filePath;
            Eps = new Variable("eps");
        }

        public void Execute()
        {
            foreach (var production in Grammar.Productions)
            {
                // start from all productions which left part = Start variable
                if (production.LeftPart.Count == 1 
                    && production.LeftPart[0].Equals(Grammar.StartVariable))
                {
                    // TODO: Right part not epsilon!!!!
                    GenerateNumber(0, production.RightPart);
                }
            }
        }
        
        private void GenerateNumber(int length, List<GrammarSymbol> list)
        {
            TryWrite(list);
            if (length == MaxLength)
                return;
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

                        GenerateNumber(length + 1, newList);
                    }
                }
            }
        }

        private void TryWrite(List<GrammarSymbol> list)
        {
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
                foreach (var symb in list)
                {
                    Console.Write(symb.Name);
                }
                Console.WriteLine();
            }
        }
    }
}