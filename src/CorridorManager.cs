using System;
using System.Collections;
using System.Collections.Generic;

namespace RogueMod
{
    public class CorridorManager : ICorridorManager
    {
        public CorridorManager(Corridor[] corridors)
        {
            Corridors = corridors;
        }
        
        public Corridor[] Corridors { get; }
        
        public void Render(IOutput scr)
        {
            for (int i = 0; i < Corridors.Length; i++)
            {
                Corridor c = Corridors[i];
                if (c.Vertical)
                {
                    scr.RenderLineV(c.Position.X, c.Position.Y, (char)Draw.Passage, c.Length);
                    continue;
                }
                
                scr.RenderLineH(c.Position.X, c.Position.Y, (char)Draw.Passage, c.Length);
            }
        }
        
        public IEnumerator<Corridor> GetEnumerator() => (IEnumerator<Corridor>)Corridors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Corridors.GetEnumerator();
    }
}