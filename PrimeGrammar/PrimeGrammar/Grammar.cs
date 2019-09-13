using System.Collections.Generic;

namespace PrimeGrammar
{
    public abstract class GrammarSymbol
    {
        public readonly string Name;

        public GrammarSymbol(string name)
        {
            Name = name;
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

        public Production(List<GrammarSymbol> leftPart, List<GrammarSymbol> rightPart)
        {
            LeftPart = leftPart;
            RightPart = rightPart;
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
    }
}