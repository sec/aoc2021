namespace aoc2021.src;

internal class Day22 : BaseDay
{
    record struct Point(int X, int Y, int Z);

    record struct Step(Point From, Point To, bool State)
    {
        public long SizeX => To.X - From.X + 1;
        public long SizeY => To.Y - From.Y + 1;
        public long SizeZ => To.Z - From.Z + 1;

        public long Volume => SizeX * SizeY * SizeX;

        public static bool Intersect(Step a, Step b)
        {
            return (a.From.X <= b.To.X && a.To.X >= b.From.X) && (a.From.Y <= b.To.Y && a.To.Y >= b.From.Y) && (a.From.Z <= b.To.Z && a.To.Z >= b.From.Z);
        }
    }

    record struct Cuboid(Point Start, Point End)
    {
    }

    IEnumerable<Step> ParseInput()
    {
        foreach (var step in ReadAllLinesSplit(" ").Select(i => new { State = i[0] == "on", Data = i[1].Split(',') }))
        {
            var x = step.Data[0][2..].Split("..").Select(int.Parse).ToArray();
            var y = step.Data[1][2..].Split("..").Select(int.Parse).ToArray();
            var z = step.Data[2][2..].Split("..").Select(int.Parse).ToArray();

            yield return new Step(new Point(x[0], y[0], z[0]), new Point(x[1], y[1], z[1]), step.State);
        }
    }

    protected override object Part1()
    {
        var cube = new Dictionary<Point, bool>();

        foreach (var step in ParseInput().ToList())
        {
            if (step.From.X < -50 || step.From.X > 50 || step.From.Y < -50 || step.From.Y > 50 || step.From.Z < -50 || step.From.Z > 50)
            {
                continue;
            }

            for (var x = step.From.X; x <= step.To.X; x++)
            {
                for (var y = step.From.Y; y <= step.To.Y; y++)
                {
                    for (var z = step.From.Z; z <= step.To.Z; z++)
                    {
                        cube[new Point(x, y, z)] = step.State;
                    }
                }
            }
        }

        return cube.Count(kv => kv.Value);
    }

    void PrintOpenScad()
    {
        var off = new StringBuilder();
        var on = new StringBuilder();
        foreach (var step in ParseInput().ToList())
        {
            var sb = step.State ? on : off;
            sb.AppendLine($"translate([{step.From.X}, {step.From.Y}, {step.From.Z}]) cube([{step.SizeX}, {step.SizeY}, {step.SizeZ}]);");
        }
        Console.WriteLine($"difference() {{");
        Console.WriteLine($"union() {{ {on} }};");
        Console.WriteLine($"union() {{ {off} }};");
        Console.WriteLine($"}}");
    }

    protected override object Part2()
    {
        var cubes = new List<Step>();
        var volumes = new List<long>();

        foreach (var step in ParseInput())
        {
            if (step.State)
            {
                // add new cube volume to list
                volumes.Add(step.Volume);
                // check for common parts with old cubes
                foreach (var old in cubes)
                {
                    if (Step.Intersect(old, step))
                    {
                        var common = GetCommonVolume(step, old);
                        volumes.Add(-common);
                    }
                }
                // add new cube to cubes
                cubes.Add(step);
            }
            else
            {
                foreach (var old in cubes)
                {
                    if (Step.Intersect(old, step))
                    {
                        var common = GetCommonVolume(step, old);
                        volumes.Add(-common);
                    }
                }
            }
        }

        return volumes.Sum();
    }

    static long GetCommonVolume(Step step, Step old)
    {
        //if (old.Volume < step.Volume)
        //{
        //    var tmp = step;
        //    step = old;
        //    old = tmp;
        //}

        var common = 0L;
        Parallel.For(step.From.X, step.To.X + 1, x =>
        //for (var x = step.From.X; x <= step.To.X; x++)
        {
              Parallel.For(step.From.Y, step.To.Y + 1, y =>
              //for (var y = step.From.Y; y <= step.To.Y; y++)
              {
                  Parallel.For(step.From.Z, step.To.Z + 1, z =>
                  //for (var z = step.From.Z; z <= step.To.Z; z++)
                  {
                      var cx = x >= old.From.X && x <= old.To.X;
                      var cy = y >= old.From.Y && y <= old.To.Y;
                      var cz = z >= old.From.Z && z <= old.To.Z;
                      if (cx && cy && cz)
                      {
                          Interlocked.Increment(ref common);
                          //common++;
                      }
                  });
              });

          });
        return common;
    }
}