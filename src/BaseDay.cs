﻿using System.Diagnostics;

namespace aoc2021;

internal abstract class BaseDay
{
    private string _input = string.Empty;

    public void Solve(bool test)
    {
        string day = GetType().Name[^2..];
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Environment.CurrentDirectory;
        var inputpath = Path.Combine(test ? "input_test" : "input", $"day{day}.txt");
        var inputfile = Path.Combine(path, inputpath);

        if (File.Exists(inputfile))
        {
            _input = File.ReadAllText(inputfile);
            if (!string.IsNullOrWhiteSpace(_input))
            {               
                Console.WriteLine($"{(test ? "Test" : "Real")} #{day}");

                var time1 = TimeIt(Part1, out var part1);
                Console.WriteLine($"Part 1 [{time1}ms]");
                Console.WriteLine($"\t{part1}");

                var time2 = TimeIt(Part2, out var part2);
                Console.WriteLine($"Part 2 [{time2}ms]");
                Console.WriteLine($"\t{part2}");

                Console.WriteLine();
            }
        }

        static long TimeIt(Func<object> func, out object result)
        {
            var sw = Stopwatch.StartNew();
            result = func();
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }

    protected string ReadAllText() => _input;
    protected string[] ReadAllLines(bool removeEmpty = false) => _input.Split(Environment.NewLine, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
    protected string[] ReadAllTextSplit(string chars) => ReadAllText().Split(chars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    protected IEnumerable<string[]> ReadAllLinesSplit(string chars) => ReadAllLines().Select(x => x.Split(chars.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

    protected abstract object Part1();
    protected abstract object Part2();
}