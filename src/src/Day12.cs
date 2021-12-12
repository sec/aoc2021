namespace aoc2021.src;

internal class Day12 : BaseDay
{
    const string START = "start";
    const string END = "end";

    int Run(bool isPartTwo)
    {
        var result = 0;
        var map = new Dictionary<string, List<string>>();

        foreach (var i in ReadAllLinesSplit("-"))
        {
            foreach (var (k, v) in new[] { (i[0], i[1]), (i[1], i[0]) })
            {
                if (!map.ContainsKey(k))
                {
                    map[k] = new List<string>();
                }
                map[k].Add(v);
            }
        }

        var start = new List<string> { START };
        var visited = new HashSet<string> { START };
        var fringe = new Queue<IEnumerable<string>>(new[] { start });

        while (fringe.Count > 0)
        {
            var path = fringe.Dequeue();

            if (path.Last() == END)
            {
                result++;
                continue;
            }

            foreach (var next in map[path.Last()])
            {
                if (next == START)
                {
                    continue;
                }

                if (next.All(char.IsLower) && path.Contains(next))
                {
                    // don't visit small cave more than once
                    if (!isPartTwo)
                    {
                        continue;
                    }

                    // don't visit one small cave more than twice
                    if (path.Where(x => x.All(char.IsLower)).GroupBy(x => x).Any(x => x.Count() > 1))
                    {
                        continue;
                    }
                }

                var new_path = path.Concat(new[] { next });
                var hashed = string.Join(",", new_path);

                if (visited.Contains(hashed))
                {
                    continue;
                }

                visited.Add(hashed);
                fringe.Enqueue(new_path);
            }
        }

        return result;
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}