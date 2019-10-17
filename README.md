# Grammars
Implementation of converters from turing machine and LBA to 0-type and 1-type Homsky grammars respectively. This task was done on the base of the machines which accept the language of prime numbers in unary aplhabet.
Main algorithms of convertion can be found in `Converter/FreeGrammarConverter.cs` for 0-type grammar and in `Converter/ContextSensitiveConverter.cs` for 1-type grammar. This algorithms also include machine based optimizations to reduce amount of productions in the final grammars.

Turing machine can be found in `UnarTM.txt`, LBA in `UnarLba.txt`.
The format for machines: first line contains `<initial state> <blank symbol> <final state separated by space>`, other lines contains machine transitions in format `<from state>,<read symbol>,<to state>,<write symbol>,<shift>`.

## Run
To run our application clone repository and download [.Net framework (version 4.6.1)](https://www.microsoft.com/ru-ru/download/details.aspx?id=48130), [build tools from Microsoft](https://www.microsoft.com/en-us/download/details.aspx?id=48159) and [target pack](https://www.microsoft.com/ru-ru/download/details.aspx?id=48136).
Then in cmd in directory with project `.sln` file run: <your path to installed build tools>\MSBuild\14.0\Bin\MsBuild.exe PrimeGrammar.sln
This command will build solution and now to run it just enter directory bin/Debug and run `.exe` from console.

## Result
The result of the program can be found in bin/release dir.
Files with grammars contain the first string with starting non-terminal and then lines with productions where elements are separated by `|`.
Other two files contains result of generating grammar usage: a few numbers with the order of grammar productions which have to be applied to generate this word.
