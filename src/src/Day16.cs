namespace aoc2021.src;

internal class Day16 : BaseDay
{
    enum PacketType { sum, product, minimum, maximum, literal, gt, lt, equal }

    record Packet(int Version, PacketType Kind, long Value, List<Packet> SubPackets)
    {
        public long SumVersion() => Version + SubPackets.Sum(x => x.SumVersion());

        public long GetValue()
        {
            return Kind switch
            {
                PacketType.sum => SubPackets.Sum(x => x.GetValue()),
                PacketType.product => SubPackets.Aggregate(1L, (a, b) => a * b.GetValue()),
                PacketType.minimum => SubPackets.Min(x => x.GetValue()),
                PacketType.maximum => SubPackets.Max(x => x.GetValue()),
                PacketType.literal => Value,
                PacketType.gt => SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0,
                PacketType.lt => SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0,
                PacketType.equal => SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0,
                _ => throw new NotImplementedException(),
            };
        }
    }

    (Packet Packet, string Input) ParsePacket(string input)
    {
        var nodes = new List<Packet>();

        input = Extract(input, 3, out var version);
        input = Extract(input, 3, out var packettype);

        if (packettype == (int) PacketType.literal)
        {
            var sb = new StringBuilder();

            while (true)
            {
                sb.Append(input.AsSpan(1, 4));
                input = Extract(input, 1, out var groupType);
                input = Extract(input, 4, out var _);

                if (groupType == 0)
                {
                    return (new Packet(version, (PacketType) packettype, Convert.ToInt64(sb.ToString(), 2), nodes), input);
                }
            }
        }
        else
        {
            input = Extract(input, 1, out var mode);

            if (mode == 0)
            {
                input = Extract(input, 15, out var totalLength);
                var processed = 0;

                while (processed < totalLength)
                {
                    var (child, newinput) = ParsePacket(input);
                    nodes.Add(child);

                    processed += input.Length - newinput.Length;
                    input = newinput;
                }
            }
            else if (mode == 1)
            {
                input = Extract(input, 11, out var totalPackets);
                for (var i = 0; i < totalPackets; i++)
                {
                    var (child, newinput) = ParsePacket(input);
                    nodes.Add(child);

                    input = newinput;
                }
            }
        }

        return (new Packet(version, (PacketType) packettype, 0, nodes), input);
    }

    string Extract(string src, int count, out int value)
    {
        value = Convert.ToInt32(src[..count], 2);

        return src[count..];
    }

    Packet GetRootPacket()
    {
        var input = string.Join(string.Empty, ReadAllText().Select(i => Convert.ToString(Convert.ToInt32(i.ToString(), 16), 2).PadLeft(4, '0')));

        return ParsePacket(input).Packet;
    }

    protected override object Part1() => GetRootPacket().SumVersion();

    protected override object Part2() => GetRootPacket().GetValue();
}