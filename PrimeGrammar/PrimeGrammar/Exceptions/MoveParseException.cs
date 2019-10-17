using System;

namespace PrimeGrammar.Exceptions
{
    public class MoveParseException : ApplicationException
    {
        public MoveParseException(string wrongInput) : base(wrongInput) {}
    }
}