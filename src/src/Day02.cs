namespace aoc2021.src;

internal class Day02 : BaseDay
{
    record Move(string Dir, int Amount);

    List<Move> ParseInput() => ReadAllLinesSplit(" ").Select(x => new Move(x[0], int.Parse(x[1]))).ToList();

    const string _up = "up";
    const string _down = "down";
    const string _forward = "forward";

    protected override object Part1()
    {
        var input = ParseInput();
        var x = input.Where(x => x.Dir == _forward).Sum(x => x.Amount);
        var up = input.Where(x => x.Dir == _up).Sum(x => x.Amount);
        var down = input.Where(x => x.Dir == _down).Sum(x => x.Amount);

        return (x * (down - up));
    }

    protected override object Part2()
    {
        var input = ParseInput();
        int x = 0, y = 0, aim = 0;

        foreach (var i in input)
        {
            switch (i.Dir)
            {
                case _forward:
                    x += i.Amount;
                    y += aim * i.Amount;
                    break;
                case _down:
                    aim += i.Amount;
                    break;
                case _up:
                    aim -= i.Amount;
                    break;
            }
        }

        return (x * y).ToString();
    }
}