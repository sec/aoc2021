namespace aoc2021.src;

internal class Day20 : BaseDay
{
    const int SIZE = 300;
    const int OFFSET = SIZE / 2;

    static void Enhance(ref bool[][] grid, ref bool[][] output, ref bool[] matrix, bool outerDefault)
    {
        var sb = new StringBuilder("AoC  2021");

        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                var index = 0;
                foreach (var (X, Y) in Ext.AroundMoves)
                {
                    sb[index++] = GetValue(ref grid, y + Y, x + X, outerDefault, ref matrix) ? '1' : '0';
                }

                var number = Convert.ToInt32(sb.ToString(), 2);

                output[y][x] = matrix[number];
            }
        }

        var swap = grid;
        grid = output;
        output = swap;
    }

    static bool GetValue(ref bool[][] grid, int y, int x, bool outerDefault, ref bool[] matrix)
    {
        if (x >= 0 && x < SIZE && y >= 0 && y < SIZE)
        {
            return grid[y][x];
        }

        return matrix[0] && outerDefault;
    }

    int Run(int count)
    {
        var input = ReadAllLines(true);

        var matrix = input.First().Select(i => i == '#').ToArray();

        // init big grid
        var grid = new bool[SIZE][];
        var temp = new bool[SIZE][];
        for (int i = 0; i < SIZE; i++)
        {
            grid[i] = new bool[SIZE];
            temp[i] = new bool[SIZE];
        }

        // load grid
        var map = input.Skip(1).ToArray();
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    grid[y + OFFSET][x + OFFSET] = true;
                }
            }
        }

        // do the magic
        for (int i = 0; i < count / 2; i++)
        {
            Enhance(ref grid, ref temp, ref matrix, false);
            Enhance(ref grid, ref temp, ref matrix, true);
        }

        // count true values
        return grid.Select(i => i.Count(j => j)).Sum();
    }

    protected override object Part1() => Run(2);

    protected override object Part2() => Run(50);
}