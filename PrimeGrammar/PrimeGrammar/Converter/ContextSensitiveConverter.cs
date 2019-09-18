using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace PrimeGrammar.Converter
{
    // непорождение, недостижимые
    //http://trpl7.ru/t-books/Martin/Martinenko_11_Ch-08.pdf
    public class ContextSensitiveConverter: IConverter
    {
        private const string Axiom1 = "S1";
        
        private const string Axiom2 = "S2";

        private const string LeftMarker = "&";
        
        private const string RightMarker = "$";
        
        private string[] AlphabetInput = new[] {"0", "1"};

        private List<Terminal> Terminals;

        private List<Production> Productions;

        private Grammar Grammar;

        public ContextSensitiveConverter()
        {
            Terminals = new List<Terminal>();
            Productions = new List<Production>();
            foreach (var symb in AlphabetInput)
            {
                Terminals.Add(new Terminal(symb));
            }
        }

        public Grammar Convert(TuringMachine turingMachine)
        {
            String arg;
            String val;
            
            foreach (var transition in turingMachine.Transitions)
            {
                // q ∈ Q\F
                if (!turingMachine.RingStates.Contains(transition.From))
                {
                    foreach (var a in Terminals)
                    {
                        if (transition.Movement == Move.Right)
                        {
                            // (p, ¢, R)∈δ(q,¢)
                            if (transition.Read.Equals(LeftMarker)
                                && transition.Write.Equals(LeftMarker))
                            {
                                foreach (var X in turingMachine.Alphabet)
                                {
                                    /////////////
                                    //2.1     [q, ¢, X, a, $]→[¢, p,X,a, $]
                                    arg = $"[{transition.From.ID},{LeftMarker},{X},{a.Name},{RightMarker}]";
                                    val = $"[{LeftMarker},{transition.To.ID},{X},{a.Name},{RightMarker}]";
                                    Add(arg, val);
                                    /////////////
                                    
                                    
                                    /////////////
                                    //5.1     [q,¢, X,a] → [¢, p, X,a], если (p, ¢, R)∈δ(q,¢)
                                    arg = $"[{transition.From.ID},{LeftMarker},{X},{a.Name}]";
                                    val = $"[{LeftMarker},{transition.To.ID},{X},{a.Name}]";
                                    Add(arg, val);
                                    /////////////
                                }
                            }
                            else
                            {
                                /////////////
                                // 2.3     [¢, q, X, a, $] → [¢, Y, a, p, $], если (p, Y, R)∈δ(q,X);
                                arg = $"[{LeftMarker},{transition.From.ID},{transition.Read},{a.Name},{RightMarker}]";
                                val = $"[{LeftMarker},{transition.Write},{a.Name},{transition.To.ID},{RightMarker}]";
                                Add(arg, val);
                                /////////////

                                /////////////
                                // 7.1     [q,X, a, $] → [Y, a, p,$], если (p, Y, R)∈δ(q,X);
                                arg = $"[{transition.From.ID},{transition.Read},{a.Name},{RightMarker}]";
                                val = $"[{transition.Write},{a.Name},{transition.To.ID},{RightMarker}]";
                                Add(arg, val);
                                /////////////
                                
                                foreach (var b in Terminals)
                                {
                                    foreach (var Z in turingMachine.Alphabet)
                                    {
                                        /////////////
                                        // 5.3     [¢, q, X,a] [Z, b]→ [¢, Y, a] [p, Z,b], если (p, Y, R)∈δ(q,X);
                                        arg =
                                            $"[{LeftMarker},{transition.From.ID},{transition.Read},{a.Name}][{Z}, {b.Name}]";
                                        val =
                                            $"[{LeftMarker},{transition.Write},{a.Name}][{transition.To.ID},{Z},{b.Name}]";
                                        Add(arg, val);
                                        /////////////
                                        
                                        /////////////
                                        // 6.1     [q,X,a] [Z, b] → [Y, a][p, Z,b], если (p, Y, R)∈δ(q,X); 
                                        arg =
                                            $"[{transition.From.ID},{transition.Read},{a.Name}][{Z}, {b.Name}]";
                                        val =
                                            $"[{transition.Write},{a.Name}][{transition.To.ID},{Z},{b.Name}]";
                                        Add(arg, val);
                                        /////////////
                                        
                                        /////////////
                                        // 6.3     [q, X,a] [Z, b,$]→ [Y, a] [p, Z,b,$] ,  если (p, Y, R)∈δ(q,X);
                                        arg =
                                            $"[{transition.From.ID},{transition.Read},{a.Name}][{Z}, {b.Name}, {RightMarker}]";
                                        val =
                                            $"[{transition.Write},{a.Name}][{transition.To.ID},{Z},{b.Name}, {RightMarker}]";
                                        Add(arg, val);
                                        /////////////
                                    }
                                }
                            }
                        }
                        else
                        {
                            // (p, $, L)∈δ(q,$)
                            if (transition.Read.Equals(RightMarker)
                                && transition.Write.Equals(RightMarker))
                            {
                                foreach (var X in turingMachine.Alphabet)
                                {
                                    /////////////
                                    // 2.4    [¢, X, a, q, $] → [¢, p, X, a, $], если (p, $, L)∈δ(q,$)
                                    arg = $"[{LeftMarker},{X},{a.Name},{transition.From.ID},{RightMarker}]";
                                    val = $"[{LeftMarker},{transition.To.ID},{X},{a.Name},{RightMarker}]";
                                    Add(arg, val);
                                    /////////////
                                    
                                    /////////////
                                    // 7.2    [X, a, q,$] → [p,X, a, $], если (p, $, L)∈δ(q,$);
                                    arg = $"[{X},{a.Name},{transition.From.ID},{RightMarker}]";
                                    val = $"[{transition.To.ID},{X},{a.Name},{RightMarker}]";
                                    Add(arg, val);
                                    /////////////
                                }
                            }
                            else
                            {
                                /////////////
                                // 2.2     [¢, q, X, a, $] → [p,¢, Y, a, $], если (p, Y, L)∈δ(q,X)
                                arg = $"[{LeftMarker},{transition.From.ID},{transition.Read},{a.Name},{RightMarker}]";
                                val = $"[{transition.To.ID},{LeftMarker},{transition.Write},{a.Name},{RightMarker}]";
                                Add(arg, val);
                                /////////////
                                
                                /////////////
                                // 5.2     [¢, q, X,a] → [p,¢, Y,a],  если (p, Y, L)∈δ(q,X)
                                arg = $"[{LeftMarker},{transition.From.ID},{transition.Read},{a.Name}]";
                                val = $"[{transition.To.ID},{LeftMarker},{transition.Write},{a.Name}]";
                                Add(arg, val);
                                /////////////
                                
                                foreach (var b in Terminals)
                                {
                                    foreach (var Z in turingMachine.Alphabet)
                                    {
                                        /////////////
                                        // 6.2     [Z, b] [q, X,a] → [p,Z,b] [Y, a], если (p, Y, L)∈δ(q,X)
                                        arg = $"[{Z},{b.Name}][{transition.From.ID},{transition.Read},{a.Name}]";
                                        val = $"[{transition.To.ID},{Z},{b}][{transition.Write},{a.Name}]";
                                        Add(arg, val);
                                        /////////////
                                        
                                        /////////////
                                        // 7.3     [Z, b] [q,X, a,$] → [p, Z, b] [Y, a,$],  если (p, Y, L)∈δ(q,X);
                                        arg = $"[{Z},{b.Name}][{transition.From.ID},{transition.Read},{a.Name}, {LeftMarker}]";
                                        val = $"[{transition.To.ID},{Z},{b}][{transition.Write},{a.Name}, {LeftMarker}]";
                                        Add(arg, val);
                                        /////////////
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            foreach (var a in Terminals)
            {
                //1     S1→[q0,¢,a, a, $]
                val = String.Format("[{0},{2},{1},{1},{3}]", turingMachine.InitialState.ID, a.Name,LeftMarker, RightMarker);
                Add(Axiom1, val);
                
                //4.1     S1→[q0, ¢, a, a]S2
                val = String.Format("[{0},{1},{2},{2}]{3}", turingMachine.InitialState.ID, LeftMarker, a.Name, Axiom2);
                Add(Axiom1, val);
                
                //4.2     S2→[a,a]S2;
                val = String.Format("[{0},{0}]{1}", a.Name, Axiom2);
                Add(Axiom2, val);
                
                //4.3     S2→[a, a,$]; 
                val = String.Format("[{0},{0},{1}]", a.Name, RightMarker);
                Add(Axiom2, val);
                
                foreach (var X in turingMachine.Alphabet)
                {
                    foreach (var q in turingMachine.RingStates)
                    {
                        /////////////
                        //3.1     [q, ¢, X, a, $] →a;
                        arg = $"[{q.ID},{LeftMarker},{X},{a.Name},{RightMarker}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //3.2     [¢, q, X, a, $] →a;
                        arg = $"[{LeftMarker},{q.ID},{X},{a.Name},{RightMarker}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //3.3     [¢, X, a, q, $] →a; 
                        arg = $"[{LeftMarker},{X},{a.Name},{q.ID},{RightMarker}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //8.1     [q,¢, X, a]→a; 
                        arg = $"[{q.ID},{LeftMarker},{X},{a.Name}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //8.2     [¢, q, X,a]→a;
                        arg = $"[{LeftMarker},{q.ID},{X},{a.Name}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //8.3     [q, X,a]→a; 
                        arg = $"[{q.ID},{X},{a.Name}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //8.4     [q, X, a,$]→a; 
                        arg = $"[{q.ID},{X},{a.Name}, {RightMarker}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //8.5     [X,a,q,$]→a;
                        arg = $"[{X},{a.Name},{q.ID}, {RightMarker}]";
                        val = a.Name;
                        Add(arg, val);
                        /////////////
                    }

                    foreach (var b in Terminals)
                    {
                        /////////////
                        //9.1     a[X,b]→ab;
                        arg = $"{a.Name}[{X},{b.Name}]";
                        val = $"{a.Name}{b.Name}";
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //9.2     a[X,b,$]→ab;
                        arg = $"{a.Name}[{X},{b.Name},{RightMarker}]";
                        val = $"{a.Name}{b.Name}";
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //9.3     [X, a]b→ab;
                        arg = $"[{X},{a.Name}]{b.Name}";
                        val = $"{a.Name}{b.Name}";
                        Add(arg, val);
                        /////////////
                        
                        /////////////
                        //9.4     [¢, X, a]b→ab;
                        arg = $"[{LeftMarker},{X},{a.Name}]{b.Name}";
                        val = $"{a.Name}{b.Name}";
                        Add(arg, val);
                        /////////////
                    }
                }
            }
            
            return null;
        }

        private void Add(string arg, string val)
        {
            Productions.Add(new Production(arg, val));
        }
    }
}