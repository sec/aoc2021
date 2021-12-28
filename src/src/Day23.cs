namespace aoc2021.src;

internal class Day23 : BaseDay
{
    record struct Point(int X, int Y);

    class Amphipod
    {
        static readonly Dictionary<int, int> _roomX = new()
        {
            { 1, 3 },
            { 10, 5 },
            { 100, 7 },
            { 1000, 9 }
        };

        public int Index;
        public int Cost;
        public Point Pos;
        public bool Moved;

        public Amphipod()
        {

        }

        public Amphipod(Amphipod prev)
        {
            Index = prev.Index;
            Cost = prev.Cost;
            Pos = prev.Pos;
            Moved = prev.Moved;
        }

        public static bool InsideRoom(Point position) => (position.Y >= 2) && (position.X is 3 or 5 or 7 or 9);

        public bool InCorrectRoom(Point position) => (position.Y >= 2) && _roomX[Cost] == position.X;

        public bool OnFinalSpace => InCorrectRoom(Pos);

        public override string ToString() => $"({Cost},{Pos.X},{Pos.Y})";
    }

    class State
    {
        public int Cost = 0;
        public readonly List<Amphipod> Amphipods = new();
        public readonly HashSet<Point> Open = new();

        public State()
        {

        }

        public State(State prev)
        {
            Cost = prev.Cost;
            Open = prev.Open;
            Amphipods = prev.Amphipods.Select(i => new Amphipod(i)).ToList();
        }

        internal void SpawnAmphipods(char c, Point p)
        {
            var a = new Amphipod
            {
                Index = Amphipods.Count,
                Pos = p,
                Cost = c switch
                {
                    'A' => 1,
                    'B' => 10,
                    'C' => 100,
                    'D' => 1000,
                    _ => throw new NotImplementedException()
                },
                Moved = false
            };

            Amphipods.Add(a);
        }

        internal IEnumerable<State> GetNextStates()
        {
            foreach (var pod in Amphipods)
            {
                var others = Amphipods.Where(x => x.Cost == pod.Cost && x.Index != pod.Index);

                // don't move if on right place and below in right one or end fo the room
                if (pod.InCorrectRoom(pod.Pos) && (others.Any(i => i.Pos.Y == pod.Pos.Y + 1) || !Open.Contains(new Point(pod.Pos.X, pod.Pos.Y + 1))))
                {
                    continue;
                }

                var visited = new HashSet<Point>(new[] { pod.Pos });
                var fringe = new Queue<(int, Point)>(new[] { (0, pod.Pos) });

                while (fringe.Count > 0)
                {
                    var (depth, current) = fringe.Dequeue();

                    if (!pod.Moved)
                    {
                        if (current.Y == 1 && (current.X != 3 && current.X != 5 && current.X != 7 && current.X != 9))
                        {
                            yield return MoveToNextState(pod.Index, current, depth);
                        }
                    }
                    else
                    {
                        var correct_room_is_full = pod.InCorrectRoom(current) && others.All(i => i.OnFinalSpace);
                        var correct_room_almost_full = pod.InCorrectRoom(current) && (others.Any(i => i.Pos.Y == current.Y + 1) || !Open.Contains(new Point(current.X, current.Y + 1)));

                        if (correct_room_is_full || correct_room_almost_full)
                        {
                            yield return MoveToNextState(pod.Index, current, depth);

                            continue;
                        }
                    }

                    var nextMoves = Ext.UpDownLeftRightMoves.Select(m => new Point(current.X + m.X, current.Y + m.Y));
                    foreach (var move in nextMoves)
                    {
                        if (visited.Contains(move) || !Open.Contains(move) || Amphipods.Any(i => i.Pos == move))
                        {
                            continue;
                        }
                        if (current.Y == 1 && (Amphipod.InsideRoom(move) && !pod.InCorrectRoom(move)))
                        {
                            // don't step from hallway into wrong room
                            continue;
                        }

                        visited.Add(move);
                        fringe.Enqueue((depth + 1, move));
                    }
                }
            }
        }

        State MoveToNextState(int index, Point newPos, int depth)
        {
            var newState = new State(this);

            var pod = newState.Amphipods.Single(x => x.Index == index);
            pod.Pos = newPos;
            pod.Moved = true;

            newState.Cost += pod.Cost * depth;

            return newState;
        }

        internal bool Final => Amphipods.All(i => i.OnFinalSpace);

        public override int GetHashCode() => this.ToString().GetHashCode();

        public override string ToString() => $"{Cost},{string.Join(",", Amphipods.Select(i => i.ToString()))}";
    }

    static State ReadState(string[] input)
    {
        State state = new();
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                var p = new Point(x, y);
                switch (c)
                {
                    case '#':
                        break;
                    case '.':
                        state.Open.Add(p);
                        break;
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                        state.SpawnAmphipods(c, p);
                        state.Open.Add(p);
                        break;
                    case ' ':
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        return state;
    }

    static State MoveAround(State start)
    {
        var fringe = new PriorityQueue<State, int>();
        var visited = new HashSet<int>();

        fringe.Enqueue(start, start.Cost);
        visited.Add(start.GetHashCode());

        while (fringe.TryDequeue(out var state, out var dist))
        {
            Console.Write($"{dist}\r");
            if (state.Final)
            {
                return state;
            }

            foreach (var ns in state.GetNextStates())
            {
                var hc = ns.GetHashCode();
                if (!visited.Contains(hc))
                {
                    visited.Add(hc);
                    fringe.Enqueue(ns, ns.Cost);
                }
            }
        }

        return new();
    }

    protected override object Part1() => MoveAround(ReadState(ReadAllLines(true))).Cost;

    protected override object Part2()
    {
        var input = ReadAllLines(true).ToList();
        input.Insert(3, "  #D#C#B#A#");
        input.Insert(4, "  #D#B#A#C#");

        return MoveAround(ReadState(input.ToArray())).Cost;
    }
}