using System.Collections.Generic;

namespace PrimeGrammar
{
    public interface GrammarSymbol {}

    public enum Alphabet
    {
        Zero, One, Epsilon
    }

    public class Terminal : GrammarSymbol
    {
        public readonly Alphabet Symbol;

        public Terminal(Alphabet symbol)
        {
            Symbol = symbol;
        }
    }
    
    public class Variable : GrammarSymbol
    {
        public readonly string Name;

        public Variable(string name)
        {
            Name = name;
        }
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

        public Grammar(Variable startVariable, List<Production> productions)
        {
            StartVariable = startVariable;
            Productions = productions;
        }
    }
}