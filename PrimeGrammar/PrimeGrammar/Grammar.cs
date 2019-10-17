using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrimeGrammar
{
    public abstract class GrammarSymbol
    {
        public readonly string Name;

        public GrammarSymbol(string name)
        {
            Name = name;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is GrammarSymbol symbol)
            {
                return Name.Equals(symbol.Name);
            }
            else
            {
                return base.Equals(obj);
            }
        }
        
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    public class Terminal : GrammarSymbol
    {
        public Terminal(string symbol) : base(symbol) {}
    }
    
    public class Variable : GrammarSymbol
    {
        public Variable(string name) : base(name) {}
    }
    
    public class Production
    {
        public readonly List<GrammarSymbol> LeftPart;

        public readonly List<GrammarSymbol> RightPart;

        public Production(string leftPart, string rightPart)
        {
            LeftPart = new List<GrammarSymbol>();
            RightPart = new List<GrammarSymbol>();
            LeftPart.Add(new Variable(leftPart));
            RightPart.Add(new Variable(rightPart));
        }

        public Production(List<GrammarSymbol> leftPart, List<GrammarSymbol> rightPart)
        {
            LeftPart = leftPart;
            RightPart = rightPart;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var elem in LeftPart)
            {
                result.Append(elem.Name + "|");
            }

            result.Remove(result.Length - 1, 1);

            result.Append(" -> ");
            
            foreach (var elem in RightPart)
            {
                result.Append(elem.Name + "|");
            }

            result.Remove(result.Length - 1, 1);

            return result.ToString();
        }
    }
    
    public class Grammar
    {
        public readonly Variable StartVariable;

        public readonly List<Production> Productions;

        public readonly List<Variable> NonTerminals;

        public readonly List<Terminal> Terminals;

        public Grammar(Variable startVariable, List<Production> productions, List<Terminal> terminals, List<Variable> nonTerminals)
        {
            StartVariable = startVariable;
            NonTerminals = nonTerminals;
            Terminals = terminals;
            Productions = productions;
        }

        public void PrintToFile(string path)
        {
            using (StreamWriter output = new StreamWriter(path))
            {
                output.WriteLine($"Start nonterminal is {StartVariable.Name}");
                foreach (var production in Productions)
                {
                    output.WriteLine(production.ToString());
                }
            }
        }
    }
}