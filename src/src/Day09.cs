namespace aoc2021.src;

internal class Day09 : BaseDay
{
    int[,] ParseInput()
    {
        var input = ReadAllLines().ToList();
        var output = new int[input.Count, input[0].Length];

        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                output[y, x] = int.Parse(input[y][x].ToString());
            }
        }

        return output;
    }

    int Run(bool isPartTwo)
    {
        var map = ParseInput();

        var part1 = new List<int>();
        var part2 = new List<int>();

        for (var y = 0; y < map.GetLength(0); y++)
        {
            for (var x = 0; x < map.GetLength(1); x++)
            {
                var p = map[y, x];
                if (map.GetAdj(x, y).All(a => p < a.Value))
                {
                    part1.Add(p);
                    part2.Add(CountBasin((x, y), map));
                }
            }
        }

        return !isPartTwo ? part1.Select(x => x + 1).Sum() : part2.OrderByDescending(x => x).Take(3).Aggregate(1, (a, b) => a * b);
    }

    static int CountBasin((int X, int Y) start, int[,] map)
    {
        var result = 1;
        var visited = new HashSet<(int X, int Y)>();
        var fringe = new Queue<(int X, int Y)>();

        visited.Add(start);
        fringe.Enqueue(start);

        while (fringe.Count > 0)
        {
            var (cx, cy) = fringe.Dequeue();

            foreach (var (x, y, v) in map.GetAdj(cx, cy))
            {
                if (visited.Contains((x, y)))
                {
                    continue;
                }
                visited.Add((x, y));

                if (v < 9)
                {
                    result++;
                    fringe.Enqueue((x, y));
                }
            }
        }

        return result;
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}