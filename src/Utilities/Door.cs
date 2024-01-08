namespace RogueMod
{
    public struct Door
    {
        public Door(int l, bool b)
        {
            Location = l;
            Vertical = b;
        }
        
        public int Location { get; set; }
        public bool Vertical { get; set; }
        
        public void Draw(Room room, VirtualScreen scr)
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
            scr.Write(x + room.Bounds.X, y + room.Bounds.Y, (char)RogueMod.Draw.Door);
        }
    }
}