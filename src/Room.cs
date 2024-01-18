using System.Collections;
using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public class Room : IRoom
    {
        public Room(RectangleI bounds, bool dark, Door[] doors)
        {
            Bounds = bounds;
            InnerBounds = new RectangleI(Bounds.X + 1, Bounds.Y + 1, Bounds.Width - 2, Bounds.Height - 2);
            Dark = dark;
            Doors = doors;
        }
        
        public bool Dark { get; set; }
        public RectangleI Bounds { get; }
        public RectangleI InnerBounds { get; }
        
        public Door[] Doors { get; }
        public int DoorCount => Doors.Length;
        
        public Door this[int index] => Doors[index];
        
        public void Render(IOutput scr)
        {
            scr.RenderBoxD(Bounds);
            
            for (int i = 0; i < Doors.Length; i++)
            {
                Door r = Doors[i];
                if (r.Hidden) { continue; }
                Vector2I pos = r.GetLocation(this);
                scr.Write(pos.X, pos.Y, Draw.Door);
            }
            
            scr.Fill(InnerBounds, (char)Draw.Floor);
        }
        
        public Vector2I GetDoor(int index)
        {
            Door d = Doors[index];
            int x = 0;
            int y = 0;
            if (d.Vertical)
            {
                y = d.Location;
            }
            else
            {
                x = d.Location;
            }
            return Bounds.Location + (x, y);
        }
        public int GetDoor(int x, int y)
        {
            Vector2I v = (x, y);
            for (int i = 0; i < Doors.Length; i++)
            {
                if (GetDoor(i) != v) { continue; }
                return i;
            }
            
            return -1;
        }
        public bool OnBoundary(int x, int y)
        {
            return x == Bounds.X || x == Bounds.Right ||
                y == Bounds.Y || y == (Bounds.Y + Bounds.Height);
        }
        
        /*
        public Vector2I GetRandomPosition()
        {
            RectangleI innerBounds = InnerBounds;
            int x = 0, y = 0;
            do
            {
                x = Program.RNG.Next(innerBounds.Left, innerBounds.Right);
                y = Program.RNG.Next(innerBounds.Top, innerBounds.Y + innerBounds.Height);
            }
            while (IsEntity(x, y));
            
            return (x, y);
        }*/
        
        public IEnumerator<Door> GetEnumerator() => (IEnumerator<Door>)Doors.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Doors.GetEnumerator();
    }
}