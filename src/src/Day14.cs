namespace aoc2021.src;

internal class Day14 : BaseDay
{
    long Run(int steps)
    {
        var input = ReadAllLines(true).ToList();
        var map = input.Skip(1).Select(i => i.Split(" -> ")).ToDictionary(k => k[0], v => v[1]);

        var counter = (input.First() + "§").GetConsecutive(2).GroupBy(i => string.Join(string.Empty, i)).ToDictionary(k => k.Key, v => v.LongCount());
        for (var s = 0; s < steps; s++)
        {
            var nextcounter = new Dictionary<string, long>();

            foreach (var (pair, count) in counter)
            {
                if (map.ContainsKey(pair))
                {
                    AddCount(nextcounter, pair[0] + map[pair], count);
                    AddCount(nextcounter, map[pair] + pair[1], count);
                }
                else
                {
                    AddCount(nextcounter, pair, 1);
                }
            }

            counter = nextcounter;
        }

        var final = counter.GroupBy(k => k.Key[0]).Select(i => i.Select(j => j.Value).Sum()).ToList();

        return final.Max() - final.Min();
    }

    static void AddCount(Dictionary<string, long> src, string what, long amount)
    {
        if (!src.ContainsKey(what))
        {
            src[what] = 0;
        }
        src[what] += amount;
    }

    protected override object Part1() => Run(10);

    protected override object Part2() => Run(40);
}