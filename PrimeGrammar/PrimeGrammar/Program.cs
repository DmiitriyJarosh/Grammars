using System;
using PrimeGrammar.Converter;

namespace PrimeGrammar
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TuringMachine tm = Parser.ParseFromTuple("C:\\Users\\Sharik\\Documents\\GitHub\\Grammars\\PrimeGrammar\\TM.txt");
            FreeGrammarConverter freeGrammarConverter = new FreeGrammarConverter();
            Grammar freeGrammar = freeGrammarConverter.Convert(tm);
            freeGrammar.PrintToFile("C:\\Users\\Sharik\\Documents\\GitHub\\Grammars\\PrimeGrammar\\output.txt");
        }
    }
}