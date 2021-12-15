namespace aoc2021.src;

internal class Day15 : BaseDay
{
    record Point(int X, int Y);

    static int FindPath(int[,] map)
    {
        // Dijkstra's algorithm
        var finish = new Point(map.GetLength(1) - 1, map.GetLength(0) - 1);
        var start = new Point(0, 0);

        var fringe = new PriorityQueue<Point, int>(new[] { (start, 0) });
        var dist = new Dictionary<Point, int>();
        var prev = new Dictionary<Point, Point>();

        map.Iterate(item => dist[new Point(item.X, item.Y)] = int.MaxValue);
        dist[start] = 0;

        while (fringe.Count > 0)
        {
            var u = fringe.Dequeue();

            if (u == finish)
            {
                var path = new List<Point>();
                while (finish != start)
                {
                    path.Add(finish);
                    finish = prev[finish];
                }
                return path.Select(i => map[i.Y, i.X]).Sum();
            }

            foreach (var (X, Y, Value) in map.GetAdj(u.X, u.Y))
            {
                var v = new Point(X, Y);
                var alt = dist[u] + map[Y, X];

                if (dist[v] > alt)
                {
                    dist[v] = alt;
                    prev[v] = u;

                    fringe.Enqueue(v, alt);
                }
            }
        }

        throw new InvalidDataException();
    }

    object Run(bool isPartTwo)
    {
        var map = ReadAllLines(true).ToGrid(int.Parse);

        if (!isPartTwo)
        {
            return FindPath(map);
        }
        else
        {
            #region Expand Map
            const int REPEAT = 5;
            var rx = map.GetLength(1);
            var ry = map.GetLength(0);

            var final_map = new int[ry * REPEAT, rx * REPEAT];

            // copy old values
            map.Iterate(item => final_map[item.Y, item.X] = item.Value);

            // expand right
            for (var y = 0; y < ry; y++)
            {
                for (var i = 1; i < REPEAT; i++)
                {
                    for (var x = 0; x < rx; x++)
                    {
                        final_map[y, (i * rx) + x] = Flip(1 + final_map[y, (i - 1) * rx + x]);
                    }
                }
            }
            // expand down
            for (var x = 0; x < rx * REPEAT; x++)
            {
                for (var i = 1; i < REPEAT; i++)
                {
                    for (var y = 0; y < ry; y++)
                    {
                        final_map[i * ry + y, x] = Flip(1 + final_map[(i - 1) * ry + y, x]);
                    }
                }
            }
            #endregion

            return FindPath(final_map);
        }
    }

    static int Flip(int v) => v > 9 ? 1 : v;

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}