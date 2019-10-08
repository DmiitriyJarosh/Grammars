using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeGrammar
{
    // it works only for [1,..] entities so hardcode :)
    public class FreeGrammarFilter
    {
        private readonly List<FilterEntity> leftContextFilters = new List<FilterEntity>();

        public void AddLeftContextFilter(string stateFrom, string stateTo, List<string> reachableSymbols)
        {
            leftContextFilters.Add(new FilterEntity(stateFrom, stateTo, reachableSymbols));
        }

        public bool FilterLeftContext(State from, State to, string leftContextSymbol)
        {
            return leftContextFilters.Any(it =>
                new State(it.StateTo).Equals(to)
                && new State(it.StateFrom).Equals(@from)
                && !it.ReachableSymbols.Contains(leftContextSymbol)
            );
        }
    }

    class FilterEntity
    {
        public readonly string StateFrom;
        public readonly string StateTo;
        public readonly List<string> ReachableSymbols;

        public FilterEntity(string stateFrom, string stateTo, List<string> reachableSymbols)
        {
            StateFrom = stateFrom;
            StateTo = stateTo;
            ReachableSymbols = reachableSymbols;
        }
    }
}