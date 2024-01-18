using Zene.Structs;

namespace RogueMod
{
    public struct Door
    {
        public Door(int l, bool v)
        {
            Location = l;
            Vertical = v;
            
            Hidden = false;
            Locked = false;
        }
        public Door(int l, bool v, bool h, bool lk)
        {
            Location = l;
            Vertical = v;
            
            Hidden = h;
            Locked = lk;
        }
        
        public int Location { get; set; }
        public bool Vertical { get; set; }
        
        public bool Hidden { get; set; }
        public bool Locked { get; set; }
        
        public Vector2I GetLocation(IRoom room)
        {
            int x = 0;
            int y = 0;
            if (Vertical)
            {
                y = Location;
            }
            else
            {
                x = Location;
            }
            return (x + room.Bounds.X, y + room.Bounds.Y);
        }
    }
}