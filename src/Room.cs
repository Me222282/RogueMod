using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public class Room
    {
        public Room(RectangleI bounds, bool dark, Door[] doors)
        {
            Bounds = bounds;
            Doors = doors;
        }
        
        public bool Dark { get; set; }
        public RectangleI Bounds { get; }
        
        public LinkedList<IEntity> Entities { get; } = new LinkedList<IEntity>();
        public Door[] Doors { get; }
        
        public void Render(bool inRoom, VirtualScreen scr)
        {
            scr.RenderBoxD(Bounds);
            
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].Draw(this, scr);
            }
            
            scr.Fill(Bounds.X + 1, Bounds.Y + 1,
                Bounds.Width - 2, Bounds.Height - 2,
                (char)Draw.Floor);
            
            if (inRoom)
            {
                foreach (IEntity e in Entities)
                {
                    e.Draw(scr);
                }
            }
        }
    }
}