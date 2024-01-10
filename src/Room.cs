using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public class Room
    {
        public Room(RectangleI bounds, bool dark, Door[] doors)
        {
            Bounds = bounds;
            Dark = dark;
            Doors = doors;
        }
        
        public bool Dark { get; set; }
        public RectangleI Bounds { get; }
        
        public LinkedList<IEntity> Entities { get; } = new LinkedList<IEntity>();
        public Door[] Doors { get; }
        
        public void Render(VirtualScreen scr, bool viewEntities)
        {
            scr.RenderBoxD(Bounds);
            
            for (int i = 0; i < Doors.Length; i++)
            {
                Doors[i].Draw(this, scr);
            }
            
            scr.Fill(Bounds.X + 1, Bounds.Y + 1,
                Bounds.Width - 2, Bounds.Height - 2,
                (char)Draw.Floor);
            
            if (viewEntities)
            {
                RenderEntites(scr, true);
            }
        }
        public void RenderEntites(VirtualScreen scr, bool visible)
        {
            if (visible)
            {
                foreach (IEntity e in Entities)
                {
                    e.Draw(scr);
                }
                return;
            }
            
            foreach (IEntity e in Entities)
            {
                scr.Write(e.Position.X, e.Position.Y, Draw.Floor);
            }
        }
        public bool Enter(IEntity entity)
        {
            if (entity is ItemEntity)
            {
                ItemEntity ie = GetItemEntity(entity.Position.X, entity.Position.Y);
                if (ie is not null) { return false; }
            }
            Entities.Add(entity);
            return true;
        }
        public void PlaceItem(ItemEntity item)
        {
            item.Position = GetNextPosition();
            Entities.Add(item);
        }
        public void Leave(IEntity entity) => Entities.Remove(entity);
        
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
        public bool OnBoundary(int x, int y)
        {
            return x == Bounds.X || x == Bounds.Right ||
                y == Bounds.Y || y == (Bounds.Y + Bounds.Height);
        }
        public bool IsEntity(int x, int y)
            => Entities.Exists(e => e.Position.X == x && e.Position.Y == y);
        public bool SeeEntity(int x, int y)
        {
            foreach (IEntity e in Entities)
            {
                if (e.Position.X != x || e.Position.Y != y) { continue; }
                e.Seen = true;
                return true;
            }
            
            return false;
        }
        public ItemEntity GetItemEntity(int x, int y)
        {
            foreach (IEntity e in Entities)
            {
                if (e is not ItemEntity ie ||
                    e.Position.X != x || e.Position.Y != y) { continue; }
                return ie;
            }
            
            return null;
        }
        
        public Vector2I GetNextPosition()
        {
            RectangleI innerBounds = new RectangleI(
                Bounds.X + 1,
                Bounds.Y + 1,
                Bounds.Width - 2,
                Bounds.Height - 2);
            int x = 0, y = 0;
            do
            {
                x = Program.RNG.Next(innerBounds.Left, innerBounds.Right);
                y = Program.RNG.Next(innerBounds.Top, innerBounds.Y + innerBounds.Height);
            }
            while (IsEntity(x, y));
            
            return (x, y);
        }
    }
}