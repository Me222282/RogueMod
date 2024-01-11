using System;
using Zene.Structs;

namespace RogueMod
{
    public struct Corridor
    {
        public Corridor(Vector2I i, int l, bool v)
        {
            Position = i;
            Length = l;
            Vertical = v;
        }
        public Corridor(Vector2I a, Vector2I b)
        {
            if (a.X == b.X)
            {
                Position = Math.Min(a.Y, b.Y) == a.Y ? a : b;
                Vertical = false;
            }
            else
            {
                Position = Math.Min(a.X, b.X) == a.X ? a : b;
                Vertical = true;
            }
            
            Length = Math.Abs((a.X - b.X) + (a.Y - b.Y));
        }
        
        public Vector2I Position { get; }
        public int Length { get; }
        public bool Vertical { get; }
    }
}