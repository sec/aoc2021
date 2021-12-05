namespace aoc2021.src;

internal class Day05 : BaseDay
{
    const int SIZE = 1000;

    protected override object Part1() => CountOverlap(false);

    protected override object Part2() => CountOverlap(true);

    int CountOverlap(bool includeDiagonal)
    {
        var input = ReadAllLines();
        var map = new int[SIZE, SIZE];

        foreach (var line in input)
        {
            var data = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
            var left = data[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            var right = data[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

            var (x1, y1) = (int.Parse(left[0]), int.Parse(left[1]));
            var (x2, y2) = (int.Parse(right[0]), int.Parse(right[1]));

            if (includeDiagonal || x1 == x2 || y1 == y2)
            {
                map[y2, x2]++;

                while (x1 != x2 || y1 != y2)
                {
                    map[y1, x1]++;

                    if (x1 != x2)
                    {
                        x1 += x1 < x2 ? 1 : -1;
                    }

                    if (y1 != y2)
                    {
                        y1 += y1 < y2 ? 1 : -1;
                    }
                }
            }
        }

        return map.Flat().Count(item => item.value >= 2);
    }
}