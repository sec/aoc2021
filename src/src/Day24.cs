using System.Collections.Concurrent;

namespace aoc2021.src;

internal class Day24 : BaseDay
{
    long _min = 0;
    long _max = 0;

    record struct State(long[] Regs, long Min, long Max)
    {
        public (long, long, long, long) GetRegs() => (Regs[0], Regs[1], Regs[2], Regs[3]);
    }

    static int Index(string r)
    {
        return r switch
        {
            "x" => 0,
            "y" => 1,
            "w" => 2,
            "z" => 3,
            _ => throw new NotImplementedException()
        };
    }

    static long Value(string r, long[] regs)
    {
        return r switch
        {
            "x" or "y" or "w" or "z" => regs[Index(r)],
            _ => long.Parse(r)
        };
    }

    protected override object Part1()
    {
        var state = new List<State>() { new State(new[] { 0L, 0, 0, 0 }, 0, 0) };

        foreach (var line in ReadAllLines(true))
        {
            var data = line.Split(' ');
            var op = data[0];
            var left = Index(data[1]);
            var right = data.Length == 3 ? data[2] : "";

            switch (op)
            {
                case "inp":
                    {
                        // remove duplicate states
                        var hash = new Dictionary<(long X, long Y, long W, long Z), (long Min, long Max)>();
                        foreach (var s in state)
                        {
                            s.Regs[left] = 0;

                            var regs = s.GetRegs();                            

                            if (hash.TryGetValue(regs, out var val))
                            {
                                var min = Math.Min(s.Min, val.Min);
                                var max = Math.Max(s.Max, val.Max);

                                hash[regs] = (min, max);
                            }
                            else
                            {
                                hash[regs] = (s.Min, s.Max);
                            }
                        }

                        // populate state without duplicates
                        state.Clear();
                        foreach (var kv in hash)
                        {
                            var (X, Y, W, Z) = kv.Key;
                            var (Min, Max) = kv.Value;

                            var item = new State(new[] { X, Y, W, Z }, Min, Max);

                            state.Add(item);
                        }

                        Console.WriteLine($"State count: {state.Count}");

                        // generate new states
                        var next = new ConcurrentBag<State>();
                        Parallel.ForEach(state, s =>
                        {
                            for (int i = 1; i <= 9; i++)
                            {
                                var regs = new[] { s.Regs[0], s.Regs[1], s.Regs[2], s.Regs[3] };
                                regs[left] = i;

                                var min = s.Min * 10 + i;
                                var max = s.Max * 10 + i;

                                next.Add(new(regs, min, max));
                            }
                        });
                        state.Clear();
                        state = next.ToList();
                    }
                    break;

                case "add":
                    {
                        Parallel.ForEach(state, s =>
                        {
                            var a = s.Regs[left];
                            var b = Value(right, s.Regs);

                            s.Regs[left] = a + b;
                        });
                    }
                    break;

                case "mul":
                    {
                        Parallel.ForEach(state, s =>
                        {
                            var a = s.Regs[left];
                            var b = Value(right, s.Regs);

                            s.Regs[left] = a * b;
                        });
                    }
                    break;

                case "div":
                    {
                        Parallel.ForEach(state, s =>
                        {
                            var a = s.Regs[left];
                            var b = Value(right, s.Regs);

                            s.Regs[left] = a / b;
                        });
                    }
                    break;

                case "mod":
                    {
                        Parallel.ForEach(state, s =>
                        {
                            var a = s.Regs[left];
                            var b = Value(right, s.Regs);

                            s.Regs[left] = a % b;
                        });
                    }
                    break;

                case "eql":
                    {
                        Parallel.ForEach(state, s =>
                        {
                            var a = s.Regs[left];
                            var b = Value(right, s.Regs);

                            s.Regs[left] = (a == b) ? 1 : 0;
                        });
                    }
                    break;
            }
        }

        var zIndex = Index("z");
        var final = state.Where(x => x.Regs[zIndex] == 0).ToList();

        _max = final.MaxBy(x => x.Max).Max;
        _min = final.MinBy(x => x.Min).Min;

        return _max;
    }

    protected override object Part2() => _min;
}