using System;
using System.Collections;
using System.Collections.Generic;

namespace RogueMod
{
    public class CorridorManager : IEntityContainer
    {
        public CorridorManager(Corridor[] corridors)
        {
            Corridors = corridors;
        }
        
        public bool Dark => true;
        
        public LinkedList<IEntity> Entities { get; } = new LinkedList<IEntity>();
        public Corridor[] Corridors { get; }
        
        public void Render(VirtualScreen scr, bool viewEntities)
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
            
            if (viewEntities)
            {
                RenderEntites(scr, true);
            }
        }
        
        public bool Enter(IEntity entity)
        {
            Entities.Add(entity);
            return true;
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
        public bool IsEntity(int x, int y)
            => Entities.Exists(e => e.Position.X == x && e.Position.Y == y);
        public void Leave(IEntity entity) => Entities.Remove(entity);
        public void PlaceItem(ItemEntity item) => throw new NotSupportedException();
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
                scr.Write(e.Position.X, e.Position.Y, Draw.Passage);
            }
        }
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

        public IEnumerator<IEntity> GetEnumerator() => Entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
    }
}