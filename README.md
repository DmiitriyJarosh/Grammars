# Grammars
Implementation of converters from turing machine and LBA to 0-type and 1-type Homsky grammars respectively. This task was done on the base of the machines which accept the language of prime numbers in unary aplhabet.
Main algorithms of convertion can be found in `Converter/FreeGrammarConverter.cs` for 0-type grammar and in `Converter/ContextSensitiveConverter.cs` for 1-type grammar. This algorithms also include machine based optimizations to reduce amount of productions in the final grammars.

Turing machine can be found in `UnarTM.txt`, LBA in `UnarLba.txt`.
The format for machines: first line contains `<initial state> <blank symbol> <final state separated by space>`, other lines contains machine transitions in format `<from state>,<read symbol>,<to state>,<write symbol>,<shift>`.

