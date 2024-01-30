using System.Collections.Generic;
using Zene.Structs;

namespace RogueMod
{
    public interface IRoomManager : IEnumerable<IRoom>
    {
        public int Level { get; }
        
        public ICorridorManager CorridorManager { get; }
        
        public LocationProperties GetProperties(int x, int y);
        
        public void UnlockDoor(int x, int y, IOutput scr);
        public void ShowDoor(int x, int y, IOutput scr);
        
        public void GenerateRooms(int level, IRogue game);
        
        public IRoom GetRandomRoom();
        
        public Vector2I GetHitCast(Vector2I pos, Direction dir);
    }
    
    public struct LocationProperties
    {
        public LocationProperties(bool w, bool ce, bool l, IRoom r)
        {
            Wall = w;
            CanEnter = ce;
            IsLocked = l;
            Room = r;
        }
        
        public bool Wall { get; }
        public bool CanEnter { get; }
        public bool IsCorridor => !Wall && Room is null;
        public bool IsLocked { get; }
        public IRoom Room { get; }
        
        public bool ShouldEluminate => Wall || IsCorridor || (Room is not null && Room.Dark);
        public bool ShouldHide => !Wall && Room is not null && Room.Dark;
    }
}