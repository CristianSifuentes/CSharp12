using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static System.Console;
using Point = (int X, int Y);    // alias any type, even a tuple

namespace CSharp12FeaturesDemo;

class Program
{
    public static void Main(string[] args)
    {
        WriteLine("C# 12.0 Features Demo");

        PrimaryConstructors();
        CollectionExpressions();
        AliasesLambdasAndRefReadonly();
        InlineArrays();
        ExperimentalAttribute();
    }

    // 1. Primary constructors in classes and structs
    static void PrimaryConstructors()
    {
        WriteLine("\n-- primary constructors --");

        var counter = new Counter(1);
        counter.Increment();
        WriteLine(counter.Total);                 // 2: the captured parameter is mutable

        var greeter = new Greeter("Hello");
        WriteLine(greeter.Greet("C# 12"));        // Hello, C# 12
    }

    // 2. Collection expressions and the spread element
    static void CollectionExpressions()
    {
        WriteLine("\n-- collection expressions --");

        int[] a = [1, 2, 3];
        List<string> b = ["x", "y"];
        Span<int> c = [4, 5];
        HashSet<int> d = [1, 1, 2];
        IEnumerable<int> e = [.. a, .. c, 9];      // spread element

        WriteLine(string.Join(", ", b));          // x, y
        WriteLine(d.Count);                       // 2
        WriteLine(e.Sum());                       // 24

        int[][] table = [[1, 2], [3]];
        WriteLine(table[0][1]);                   // 2

        int[] empty = [];
        WriteLine(empty.Length);                  // 0
    }

    // 3. Alias any type, default lambda parameters and ref readonly parameters
    static void AliasesLambdasAndRefReadonly()
    {
        WriteLine("\n-- aliases, lambdas and ref readonly --");

        Point p = (1, 2);
        WriteLine(p.Y);                           // 2

        var greet = (string name = "world") => $"hello {name}";
        WriteLine(greet());                       // hello world

        int value = 5;
        WriteLine(Read(in value));                // 5
    }

    static int Read(ref readonly int x) => x;

    // 4. Inline arrays: a fixed-size buffer inside a struct, without unsafe
    static void InlineArrays()
    {
        WriteLine("\n-- inline arrays --");

        var buffer = new Buffer();
        for (int i = 0; i < 4; i++) buffer[i] = i * i;

        int total = 0;
        foreach (int v in buffer) total += v;
        WriteLine(total);                         // 14
    }

    // 5. [Experimental]: using the API is an error unless its id is suppressed
    static void ExperimentalAttribute()
    {
        WriteLine("\n-- Experimental --");

#pragma warning disable DEMO001
        WriteLine(Beta.Try());                    // experimental API called
#pragma warning restore DEMO001
    }
}

public class Counter(int total)
{
    public int Total => total;
    public void Increment() => total++;
}

public class Greeter(string greeting)
{
    public string Greet(string name) => $"{greeting}, {name}";
}

[InlineArray(4)]
public struct Buffer
{
    private int _element0;
}

public static class Beta
{
    [Experimental("DEMO001")]
    public static string Try() => "experimental API called";
}
