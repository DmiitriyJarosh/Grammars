using System;

namespace PrimeGrammar.Exceptions
{
    public class ParseException : ApplicationException
    {
        public ParseException(string message) : base(message) {}
    }
}