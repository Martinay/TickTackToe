namespace TickTackToe.Agent
{
    public class Move
    {
        public Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString()
        {
            return $"X: {X} Y: {Y}";
        }
        public string GetIdentifier() => $"{X}_{Y}";
    }
}
