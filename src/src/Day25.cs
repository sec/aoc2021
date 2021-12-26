namespace aoc2021.src;

internal class Day25 : BaseDay
{
    const int EMPTY = 0;
    const int DOWN = 1;
    const int RIGHT = 2;

    static bool MoveRight(ref int[,] sea)
    {
        var moved = false;
        var tmp = new int[sea.GetLength(0), sea.GetLength(1)];

        for (int y = 0; y < sea.GetLength(0); y++)
        {
            for (int x = 0; x < sea.GetLength(1); x++)
            {
                if (sea[y, x] == RIGHT)
                {
                    var nx = (x + 1) % sea.GetLength(1);
                    if (sea[y, nx] == EMPTY)
                    {
                        moved = true;
                        tmp[y, nx] = RIGHT;
                    }
                    else
                    {
                        tmp[y, x] = RIGHT;
                    }
                }
                else if (sea[y, x] == DOWN)
                {
                    tmp[y, x] = DOWN;
                }
            }
        }

        sea = tmp;

        return moved;
    }

    static bool MoveDown(ref int[,] sea)
    {
        var moved = false;
        var tmp = new int[sea.GetLength(0), sea.GetLength(1)];

        for (int y = 0; y < sea.GetLength(0); y++)
        {
            for (int x = 0; x < sea.GetLength(1); x++)
            {
                if (sea[y, x] == RIGHT)
                {
                    tmp[y, x] = RIGHT;
                }
                else if (sea[y, x] == DOWN)
                {
                    var ny = (y + 1) % sea.GetLength(0);
                    if (sea[ny, x] == EMPTY)
                    {
                        tmp[ny, x] = DOWN;
                        moved = true;
                    }
                    else
                    {
                        tmp[y, x] = DOWN;
                    }
                }
            }
        }

        sea = tmp;

        return moved;
    }

    protected override object Part1()
    {
        var sea = ReadAllLines(true).ToGrid(s => s == "v" ? DOWN : s == ">" ? RIGHT : EMPTY);
        var step = 0;

        while (true)
        {
            var f1 = MoveRight(ref sea);
            var f2 = MoveDown(ref sea);
            step++;

            if (!f1 && !f2)
            {
                return step;
            }
        }
    }

    protected override object Part2()
    {
        return 0;
    }
}