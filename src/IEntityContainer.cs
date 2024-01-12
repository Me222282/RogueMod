using System.Collections.Generic;

namespace RogueMod
{
    public interface IEntityContainer
    {
        public bool Dark { get; }
        
        public void Render(VirtualScreen scr, bool viewEntities);
        public void RenderEntites(VirtualScreen scr, bool visible);
        public bool Enter(IEntity entity);
        public void PlaceItem(ItemEntity item);
        public void Leave(IEntity entity);
        
        public bool IsEntity(int x, int y);
        public bool SeeEntity(int x, int y);
        public ItemEntity GetItemEntity(int x, int y);
    }
}