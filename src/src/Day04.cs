namespace aoc2021.src;

internal class Day04 : BaseDay
{
    const int BOARD_SIZE = 5;

    (List<Board>, List<int>) ParseInput()
    {
        var lines = ReadAllLines(true).ToList();

        var numbers = lines.First().Split(',').Select(int.Parse).ToList();
        var boards = new List<Board>();

        foreach (var batch in lines.Skip(1).Chunk(BOARD_SIZE))
        {
            boards.Add(new Board(batch));
        }

        return (boards, numbers);
    }

    class Board
    {
        public int[][] Numbers { get; private set; } = new int[BOARD_SIZE][];

        public bool[][] Marked { get; private set; } = new bool[BOARD_SIZE][];

        public Board(string[] batch)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                Numbers[i] = batch[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                Marked[i] = new bool[BOARD_SIZE];
            }
        }

        internal void Mark(int number) => Numbers.Iterate(item => Marked[item.X][item.Y] = Marked[item.X][item.Y] | item.Value == number);

        internal bool Won() => Enumerable.Range(0, BOARD_SIZE).Any(i => Marked[i].All(x => x) || Marked.GetColumn(i).All(x => x));

        internal int SumUnmarked() => Numbers.Flat().Where(item => !Marked[item.row][item.column]).Sum(item => item.value);
    }

    protected override object Part1() => Play(false);

    protected override object Part2() => Play(true);

    int Play(bool untilLast)
    {
        var (boards, numbers) = ParseInput();

        foreach (var number in numbers)
        {
            foreach (var b in boards)
            {
                b.Mark(number);

                if ((untilLast && boards.All(x => x.Won())) || (!untilLast && b.Won()))
                {
                    return b.SumUnmarked() * number;
                }
            }
        }

        return -1;
    }
}