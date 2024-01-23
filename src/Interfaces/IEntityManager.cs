using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public interface IEntityManager : IEnumerable<IEntity>
    {
        public void Render(RectangleI bounds, IOutput scr);
        public void Show(int x, int y, IOutput scr);
        public void Hide(int x, int y, IOutput scr);
        
        public ICharacter GetCharacter(int x, int y);
        public IItem GetItem(int x, int y);
        public IEntity GetTopEntity(int x, int y);
        
        public bool Place(Vector2I pos, IEntity entity);
        public void Delete(int x, int y, bool top = false);
        
        public bool Shift(Vector2I start, Vector2I end);
        
        public ICharacter GetHitCast(Vector2I start, Direction dir, Vector2I end);
        
        public Vector2I GetRandomPosition(RectangleI bounds);
    }
}