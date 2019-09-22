using PrimeGrammar.Converter;

namespace PrimeGrammar
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TuringMachine lba = Parser.ParseFromArrow("D:\\GitHub\\Grammars\\PrimeGrammar\\lba.txt");
            ContextSensitiveConverter contextSensitiveConverter = new ContextSensitiveConverter();
            Grammar contextSensitiveGrammar = contextSensitiveConverter.Convert(lba);
            TuringMachine tm = Parser.ParseFromArrow("D:\\GitHub\\Grammars\\PrimeGrammar\\mt.txt");
            FreeGrammarConverter freeGrammarConverter = new FreeGrammarConverter();
            Grammar freeGrammar = freeGrammarConverter.Convert(tm);
            freeGrammar.PrintToFile("D:\\GitHub\\Grammars\\PrimeGrammar\\FreeGrammar.txt");
            Generator generator = new Generator(freeGrammar, "");
            generator.Execute();
        }
    }
}