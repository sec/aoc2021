namespace aoc2021.src;

internal class Day11 : BaseDay
{
    long Run(bool isPartTwo)
    {
        var grid = ReadAllLines(true).ToGrid(int.Parse);

        if (!isPartTwo)
        {
            return Enumerable.Range(0, 100).Select(x => Step(grid)).Sum();
        }
        else
        {
            // did extra step, but TakeWhile didn't took it, so +1
            return 1 + Enumerable.Range(0, int.MaxValue).TakeWhile(x => Step(grid) != 100).Count();
        }
    }

    private static long Step(int[,] grid)
    {
        var visited = new HashSet<(int X, int Y)>();

        grid.Iterate(i => grid[i.Y, i.X]++);

        while (true)
        {
            var keepGoing = false;

            grid.Iterate(i =>
            {
                if (i.Value > 9 && !visited.Contains((i.X, i.Y)))
                {
                    keepGoing = true;
                    visited.Add((i.X, i.Y));

                    grid.GetAllAdj(i.X, i.Y).ToList().ForEach(item => grid[item.Y, item.X]++);
                }
            });

            if (!keepGoing)
            {
                break;
            }
        }

        return grid.Iterate(i =>
        {
            if (i.Value > 9)
            {
                grid[i.Y, i.X] = 0;
            }
        }).Flat().Count(i => i.value == 0);
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}