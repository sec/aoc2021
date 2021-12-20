namespace aoc2021.src;

internal class Day19 : BaseDay
{
    record Point(int X, int Y, int Z)
    {
        public static Point FromString(string data)
        {
            var i = data.Split(",").Select(int.Parse).ToArray();

            return new Point(i[0], i[1], i[2]);
        }

        public Point Rotate(int index)
        {
            return index switch
            {
                0 => new Point(+X, +Y, +Z),
                1 => new Point(+X, +Z, -Y),
                2 => new Point(+X, -Y, -Z),
                3 => new Point(+X, -Z, +Y),

                4 => new Point(-X, +Y, -Z),
                5 => new Point(-X, -Z, -Y),
                6 => new Point(-X, +Z, +Y),
                7 => new Point(-X, -Y, +Z),

                8 => new Point(+Y, +Z, +X),
                9 => new Point(+Y, -Z, -X),
                10 => new Point(+Y, +X, -Z),
                11 => new Point(+Y, -X, +Z),

                12 => new Point(-Y, +Z, -X),
                13 => new Point(-Y, +X, +Z),
                14 => new Point(-Y, -X, -Z),
                15 => new Point(-Y, -Z, +X),

                16 => new Point(+Z, +X, +Y),
                17 => new Point(+Z, +Y, -X),
                18 => new Point(+Z, -X, -Y),
                19 => new Point(+Z, -Y, +X),

                20 => new Point(-Z, +X, -Y),
                21 => new Point(-Z, +Y, +X),
                22 => new Point(-Z, -X, +Y),
                23 => new Point(-Z, -Y, -X),

                _ => throw new NotImplementedException(),
            };
        }

        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    record Scanner(int Index, List<Point> Beacons)
    {
        public Point Position = new(0, 0, 0);

        public bool Located = false;

        public Lazy<List<List<Point>>> Rotated = new(() => Enumerable.Range(0, 24).Select(i => Beacons.Select(p => p.Rotate(i)).ToList()).ToList());
    }

    private List<Scanner>? _scanners;

    static bool DoTheyOverlap(Scanner one, Scanner two, out Point Where)
    {
        foreach (var beaconA in one.Beacons)
        {
            foreach (var twoBeacons in two.Rotated.Value)
            {
                foreach (var beaconB in twoBeacons)
                {
                    var diff = beaconA - beaconB;
                    var movedB = twoBeacons.Select(p => p + diff).ToList();
                    var common = one.Beacons.Where(p => movedB.Contains(p));

                    if (common.Count() >= 12)
                    {
                        // add reoriented beacons to newly located scanner
                        two.Beacons.Clear();
                        two.Beacons.AddRange(movedB);

                        Where = diff;

                        return true;
                    }
                }
            }
        }

        Where = new(0, 0, 0);
        return false;
    }

    void LoadUp()
    {
        // Read
        _scanners = new List<Scanner>();
        foreach (var line in ReadAllLines(true))
        {
            if (line.StartsWith("---"))
            {
                _scanners.Add(new Scanner(int.Parse(line.Split(" ")[2]), new()));
            }
            else
            {
                _scanners.Last().Beacons.Add(Point.FromString(line));
            }
        }

        // Mark 0 as Located
        _scanners[0].Located = true;

        // Locate others
        while (_scanners.Any(s => !s.Located))
        {
            foreach (var unlocated in _scanners.Where(x => !x.Located))
            {
                foreach (var located in _scanners.Where(x => x.Located))
                {
                    if (DoTheyOverlap(located, unlocated, out var delta))
                    {
                        unlocated.Located = true;
                        unlocated.Position = delta;

                        Console.WriteLine($"Located {unlocated.Index} at {unlocated.Position} with {located.Index}");

                        break;
                    }
                }
            }
        }
    }

    protected override object Part1()
    {
        LoadUp();

        return _scanners!.SelectMany(x => x.Beacons).Distinct().Count();
    }

    protected override object Part2()
    {
        return _scanners!.GetPermutations(2)
            .Select(i => new { a = i.First(), b = i.Skip(1).First() })
            .Select(pair => Ext.ManhattanDistance(pair.a.Position.X, pair.a.Position.Y, pair.a.Position.Z, pair.b.Position.X, pair.b.Position.Y, pair.b.Position.Z))
            .Max();
    }
}