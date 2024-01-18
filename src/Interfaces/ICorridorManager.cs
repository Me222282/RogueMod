using System.Collections.Generic;

namespace RogueMod
{
    public interface ICorridorManager : IEnumerable<Corridor>
    {
        public void Render(IOutput scr);
    }
}