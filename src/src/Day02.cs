namespace aoc2021.src;

internal class Day02 : BaseDay
{
    protected override string Part1()
    {
        var input = ReadAllLinesSplit(" ");

        var x = 0;
        var y = 0;

        foreach (var i in input)
        {
            var n = int.Parse(i[1]);

            switch (i[0])
            {
                case "forward":
                    x += n;
                    break;
                case "down":
                    y += n;
                    break;
                case "up":
                    y -= n;
                    break;
            }
        }

        return (x * y).ToString();
    }

    protected override string Part2()
    {
        var input = ReadAllLinesSplit(" ");

        var x = 0;
        var y = 0;
        var aim = 0;

        foreach (var i in input)
        {
            var n = int.Parse(i[1]);

            switch (i[0])
            {
                case "forward":
                    x += n;
                    y += aim * n;
                    break;
                case "down":
                    aim += n;
                    break;
                case "up":
                    aim -= n;
                    break;
            }
        }

        return (x * y).ToString();
    }
}