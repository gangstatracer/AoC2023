namespace AoC2023.Day20;

public class Day20
{
    // [TestCase("Day20/example.txt", 1, 32)]
    // [TestCase("Day20/example.txt", 1_000, 32_000_000)]
    // [TestCase("Day20/example2.txt", 1_000, 11_687_500)]
    // [TestCase("Day20/input.txt", 1_000, 432434)]
    public void Part1(string path, int pushCount, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var config = input
            .Select(l => l.Halve(" -> "))
            .ToDictionary(t => t.Item1, t => t.Item2.Split(", "));

        var modules = config.Keys.Select(k => ((string, Module))(k[0] switch
            {
                '%' => (k, new FlipFlopModule(k[1..])),
                '&' => (k, new ConjunctionModule(k[1..], config.Count(kvp => kvp.Value.Contains(k[1..])))),
                _ => (k, new BroadcastModule(k)),
            }))
            .ToDictionary(m => m.Item1, m => m.Item2);

        foreach (var c in config)
        {
            modules[c.Key].Outputs = modules
                .Where(cc => c.Value.Contains(cc.Value.Name))
                .Select(cc => cc.Value)
                .ToArray();
        }

        modules["broadcaster"].Pulse("button", false);
    }

    private abstract class Module
    {
        protected Module(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public Module[] Outputs { get; set; }
        public int HighCount;
        public int LowCount;

        public abstract void Pulse(
            string inputModuleName,
            bool value);

        protected void SendPulse(bool value)
        {
            foreach (var output in Outputs)
            {
                output.Pulse(Name, value);
            }

            if (value)
                HighCount += Outputs.Length;
            else
                LowCount += Outputs.Length;
        }
    }

    private class FlipFlopModule : Module
    {
        public FlipFlopModule(string name)
            : base(name)
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

        public ConjunctionModule(string name, int inputsCount)
            : base(name)
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
        public BroadcastModule(string name)
            : base(name)
        {
        }

        public override void Pulse(string inputModuleName, bool value)
        {
            SendPulse(value);
        }
    }
}