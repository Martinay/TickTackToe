using System.Collections.Generic;
using TickTackToe.Game;

namespace TickTackToe.Agent.Team2
{
    public class StatusComparer : IEqualityComparer<Status>
    {
        public bool Equals(Status x, Status y)
        {
            for (int i = 0; i < x.Field.Count; i++)
            {
                for (int j = 0; j < x.Field.Count; j++)
                {
                    if (x.Field[i][j] != y.Field[i][j])
                        return false;
                }
            }

            return true;
        }

        public int GetHashCode(Status obj)
        {
            var hash = 0;
            for (int i = 0; i < obj.Field.Count; i++)
            {
                for (int j = 0; j < obj.Field.Count; j++)
                {
                    hash += (int)obj.Field[i][j];
                }
            }

            return hash;
        }
    }
}
