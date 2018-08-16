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
    }
}
