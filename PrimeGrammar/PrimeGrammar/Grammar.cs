using System.Collections.Generic;

namespace PrimeGrammar
{
    public interface IGrammarSymbol {}

    public enum Alphabet
    {
        Zero, One, Epsilon
    }

    public class Terminal : IGrammarSymbol
    {
        public readonly Alphabet Symbol;

        public Terminal(Alphabet symbol)
        {
            Symbol = symbol;
        }
    }
    
    public class Variable : IGrammarSymbol
    {
        public readonly string Name;

        public Variable(string name)
        {
            Name = name;
        }
    }
    
    public class Production
    {
        public readonly List<IGrammarSymbol> LeftPart;

        public readonly List<IGrammarSymbol> RightPart;

        public Production(List<IGrammarSymbol> leftPart, List<IGrammarSymbol> rightPart)
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