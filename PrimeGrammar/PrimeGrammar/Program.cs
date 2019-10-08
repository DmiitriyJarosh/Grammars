using System.Collections.Generic;
using PrimeGrammar.Converter;

namespace PrimeGrammar
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TuringMachine lba = Parser.ParseFromTuple("C:\\Users\\senior\\Desktop\\Git\\Grammars\\PrimeGrammar\\UnarLba.txt");
            ContextSensitiveConverter contextSensitiveConverter = new ContextSensitiveConverter();
            Grammar contextSensitiveGrammar = contextSensitiveConverter.Convert(lba);
            TuringMachine tm = Parser.ParseFromTuple("C:\\Users\\senior\\Desktop\\Git\\Grammars\\PrimeGrammar\\UnarTM.txt");
            FreeGrammarConverter freeGrammarConverter = new FreeGrammarConverter();
            Grammar freeGrammar = freeGrammarConverter.Convert(tm);
            freeGrammar.PrintToFile("C:\\Users\\senior\\Desktop\\Git\\Grammars\\PrimeGrammar\\UnarGrammar.txt");
            Generator generator = new Generator(freeGrammar, "");
            generator.Execute();
        }

        /// <summary>
        /// WW^R
        /// </summary>
        /// <returns></returns>
        public static Grammar GenerateTest()
        {
            Variable start = new Variable("S");
            
            List<Production> productions = new List<Production>();
            List<GrammarSymbol> leftPart = new List<GrammarSymbol>(); leftPart.Add(new Variable("S"));
            List<GrammarSymbol> rightPart = new List<GrammarSymbol>(); rightPart.Add(new Variable("0")); rightPart.Add(new Variable("S")); rightPart.Add(new Variable("0"));
            productions.Add(new Production(leftPart, rightPart));
            
            leftPart = new List<GrammarSymbol>(); leftPart.Add(new Variable("S"));
            rightPart = new List<GrammarSymbol>(); rightPart.Add(new Variable("1")); rightPart.Add(new Variable("S")); rightPart.Add(new Variable("1"));
            productions.Add(new Production(leftPart, rightPart));
            
            productions.Add(new Production("S", "eps"));
            
            List<Terminal> terminals = new List<Terminal>();
            terminals.Add(new Terminal("0"));
            terminals.Add(new Terminal("1"));
            
            List<Variable> nonTerminals = new List<Variable>();
            nonTerminals.Add(new Variable("S"));

            return new Grammar(start, productions, terminals, nonTerminals);
        }
    }
}