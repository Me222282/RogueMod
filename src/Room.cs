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
        
        public bool PlayerInRoom { get; private set; }
        
        public LinkedList<IEntity> Entities { get; } = new LinkedList<IEntity>();
        public Door[] Doors { get; }
        
        public void Render(VirtualScreen scr)
        {
            scr.RenderBoxD(Bounds);
            
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].Draw(this, scr);
            }
            
            scr.Fill(Bounds.X + 1, Bounds.Y + 1,
                Bounds.Width - 2, Bounds.Height - 2,
                (char)Draw.Floor);
            
            if (PlayerInRoom)
            {
                foreach (IEntity e in Entities)
                {
                    e.Draw(scr);
                }
            }
        }
        public void Enter(IEntity entity)
        {
            if (entity is Player)
            {
                PlayerInRoom = true;
            }
            else if (entity is ItemEntity)
            {
                entity.Position = GetNextPosition();
            }
            Entities.Add(entity);
        }
        public void Leave(IEntity entity)
        {
            Entities.Remove(entity);
            if (entity is Player)
            {
                PlayerInRoom = false;
            }
        }
        
        public Vector2I GetNextPosition()
        {
            RectangleI innerBounds = new RectangleI(
                Bounds.X + 1,
                Bounds.Y + 1,
                Bounds.Width - 2,
                Bounds.Height - 2);
            int x = 0, y = 0;
            while (Entities.Exists(e => e.Position.X == x && e.Position.Y == y))
            {
                x = Program.RNG.Next(innerBounds.Left, innerBounds.Right);
                y = Program.RNG.Next(innerBounds.Top, innerBounds.Y + innerBounds.Height);
            }
            
            return (x, y);
        }
    }
}