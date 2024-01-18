using Zene.Structs;

namespace RogueMod
{
    public interface IRogue
    {
        public IGameOutput Out { get; }
        
        public Vector2I PlayingSize { get; }
        public string PlayerName { get; set; }
        
        public Discoveries Discoveries { get; }
        public Mapping NameMaps { get; }
        public Player Player { get; }
        public IRoom CurrentRoom { get; }
        
        public IRoomManager RoomManager { get; }
        public ICorridorManager CorridorManager { get; }
        public IEntityManager EntityManager { get; }
        
        public bool TryMoveCharacter(int x, int y, ICharacter character);
    }
}