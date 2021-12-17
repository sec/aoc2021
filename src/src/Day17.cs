namespace aoc2021.src;

internal class Day17 : BaseDay
{
    int Run(bool isPartTwo)
    {
        var input = Regex.Matches(ReadAllText(), @"-?\d+");
        var tx1 = int.Parse(input[0].Value);
        var tx2 = int.Parse(input[1].Value);
        var ty1 = int.Parse(input[2].Value);
        var ty2 = int.Parse(input[3].Value);

        var result = 0;

        for (var veloX = 0; veloX < 250; veloX++)
        {
            for (var veloY = -1000; veloY < 1000; veloY++)
            {
                if (Fire(veloX, veloY, tx1, tx2, ty1, ty2, out var max))
                {
                    result = isPartTwo ? result + 1 : Math.Max(result, max);
                }
            }
        }

        return result;
    }

    static bool Fire(int veloX, int veloY, int tx1, int tx2, int ty1, int ty2, out int maxY)
    {
        var px = 0;
        var py = 0;
        maxY = 0;

        while (true)
        {
            if (px >= tx1 && px <= tx2 && py >= ty1 && py <= ty2)
            {
                return true;
            }

            if (px > tx2 || py < ty1)
            {
                return false;
            }

            px += veloX;
            py += veloY;

            maxY = Math.Max(maxY, py);

            veloY--;

            if (veloX > 0)
            {
                veloX--;
            }
            else if (veloX < 0)
            {
                veloX++;
            }
        }
    }

    protected override object Part1() => Run(false);

    protected override object Part2() => Run(true);
}