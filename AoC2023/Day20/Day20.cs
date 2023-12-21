namespace AoC2023.Day20;

public class Day20
{
    [TestCase("Day20/example.txt", 1, 32)]
    [TestCase("Day20/example.txt", 1_000, 32_000_000)]
    [TestCase("Day20/example2.txt", 1_000, 11_687_500)]
    [TestCase("Day20/input.txt", 1_000, 703315117)]
    public void Part1(string path, int pushCount, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var config = input
            .Select(l => l.Halve(" -> "))
            .ToDictionary(t => t.Item1, t => t.Item2.Split(", "));

        var bus = new Bus();
        bus.Modules = config.Keys.Select(k => (Module)(k[0] switch
            {
                '%' => new FlipFlopModule(k[1..], config[k], bus),
                '&' => new ConjunctionModule(k[1..], config.Count(kvp => kvp.Value.Contains(k[1..])), config[k], bus),
                _ => new BroadcastModule(k, config[k], bus),
            }))
            .ToDictionary(m => m.Name, m => m);

        for (var i = 0; i < pushCount; i++)
        {
            bus.Pulse("button", new[] { "broadcaster" }, false);
            while (bus.MoveNext())
            {
            }
        }

        (bus.HighCount * bus.LowCount).Should().Be(expected);
    }

    //[TestCase("Day20/input.txt", 703315117)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var config = input
            .Select(l => l.Halve(" -> "))
            .ToDictionary(t => t.Item1, t => t.Item2.Split(", "));

        var bus = new Bus();
        bus.Modules = config.Keys.Select(k => (Module)(k[0] switch
            {
                '%' => new FlipFlopModule(k[1..], config[k], bus),
                '&' => new ConjunctionModule(k[1..], config.Count(kvp => kvp.Value.Contains(k[1..])), config[k], bus),
                _ => new BroadcastModule(k, config[k], bus),
            }))
            .ToDictionary(m => m.Name, m => m);

        bus.Modules["rx"] = new BroadcastModule("rx", Array.Empty<string>(), bus);
        var pushCount = 0;
        while (bus.Modules["rx"].LowCount < 1)
        {
            pushCount++;
            bus.Pulse("button", new[] { "broadcaster" }, false);
            while (bus.MoveNext())
            {
            }
        }

        pushCount.Should().Be(expected);
    }

    private class Bus
    {
        private Queue<(string, string[], bool)> pulseQueue = new();
        public Dictionary<string, Module> Modules { get; set; }

        public int HighCount;
        public int LowCount;

        public void Pulse(string from, string[] to, bool value)
        {
            if (value)
                HighCount += to.Length;
            else
                LowCount += to.Length;

            pulseQueue.Enqueue((from, to, value));
        }

        public bool MoveNext()
        {
            if (!pulseQueue.Any())
                return false;
            var (from, to, value) = pulseQueue.Dequeue();
            foreach (var module in to)
                if (Modules.TryGetValue(module, out var m))
                    m.Pulse(from, value);
            return true;
        }
    }

    private abstract class Module
    {
        protected Module(string name, string[] outputs, Bus bus)
        {
            Name = name;
            Outputs = outputs;
            Bus = bus;
        }

        public string Name { get; }
        private Bus Bus { get; }
        private string[] Outputs { get; }

        public abstract void Pulse(
            string inputModuleName,
            bool value);

        public int LowCount { get; private set; }

        protected void SendPulse(bool value)
        {
            if (!value)
                LowCount++;

            Bus.Pulse(Name, Outputs, value);
        }
    }

    private class FlipFlopModule : Module
    {
        public FlipFlopModule(string name, string[] outputs, Bus bus)
            : base(name, outputs, bus)
        {
        }

        private bool State { get; set; }

        public override void Pulse(string inputModuleName, bool value)
        {
            if (value)
                return;

            State = State != true;
            SendPulse(State);
        }
    }

    private class ConjunctionModule : Module
    {
        private Dictionary<string, bool> Inputs { get; } = new();

        public ConjunctionModule(string name, int inputsCount, string[] outputs, Bus bus)
            : base(name, outputs, bus)
        {
            InputsCount = inputsCount;
        }

        private int InputsCount { get; }

        public override void Pulse(string inputModuleName, bool value)
        {
            Inputs[inputModuleName] = value;
            var nextValue = Inputs.Count(kvp => kvp.Value) != InputsCount;
            SendPulse(nextValue);
        }
    }

    private class BroadcastModule : Module
    {
        public BroadcastModule(string name, string[] outputs, Bus bus)
            : base(name, outputs, bus)
        {
        }

        public override void Pulse(string inputModuleName, bool value)
        {
            SendPulse(value);
        }
    }
}