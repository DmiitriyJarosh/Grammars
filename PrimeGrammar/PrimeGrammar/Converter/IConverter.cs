namespace PrimeGrammar.Converter
{
    public interface IConverter
    {
        Grammar Convert(TuringMachine turingMachine);
    }
}