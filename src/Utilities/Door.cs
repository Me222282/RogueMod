using Zene.Structs;

namespace RogueMod
{
    public struct Door
    {
        public Door(int pi)
        {
            PerimeterIndex = pi;
            
            Hidden = false;
            Locked = false;
        }
        public Door(int pi, bool h, bool lk)
        {
            PerimeterIndex = pi;
            
            Hidden = h;
            Locked = lk;
        }
        
        public int PerimeterIndex { get; set; }
        
        public bool Hidden { get; set; }
        public bool Locked { get; set; }
        
        public Vector2I GetLocation(IRoom room)
        {
            int lastStage = 0;
            int stage = room.Bounds.Width - 2;
            // Top
            if (PerimeterIndex < stage)
            {
                return (room.Bounds.X + PerimeterIndex - lastStage + 1,
                    room.Bounds.Y);
            }
            lastStage = stage;
            stage += room.Bounds.Height - 2;
            // Right
            if (PerimeterIndex < stage)
            {
                return (room.Bounds.X + room.Bounds.Width - 1,
                    room.Bounds.Y + PerimeterIndex - lastStage + 1);
            }
            lastStage = stage;
            stage += room.Bounds.Width - 2;
            // Bottom
            if (PerimeterIndex < stage)
            {
                return (room.Bounds.X + room.Bounds.Width - (PerimeterIndex - lastStage),
                    room.Bounds.Y + room.Bounds.Height - 1);
            }
            lastStage = stage;
            stage += room.Bounds.Height - 2;
            // Left
            if (PerimeterIndex < stage)
            {
                return (room.Bounds.X,
                    room.Bounds.Y + room.Bounds.Height - (PerimeterIndex - lastStage));
            }
            
            throw new System.Exception();
        }
    }
}