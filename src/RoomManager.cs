using Zene.Structs;

namespace RogueMod
{
    public sealed class RoomManager
    {
        public const int Corridor = -1;
        public const int NoEntry = 0;
        
        public RoomManager(Vector2I playingSize)
        {
            _size = playingSize;
            _roomMap = new int[playingSize.Y, playingSize.X];
        }
        
        private Vector2I _size;
        private int[,] _roomMap;
        
        public Room[] Rooms { get; private set; }
        
        public void GenRooms(int level)
        {
            Rooms = new Room[]
            {
                new Room(new RectangleI(2, 2, 10, 10), false,
                    new Door[] { new Door(4, false), new Door(3, true) })
            };
            FillMap(0);
        }
        private void FillMap(int roomIndex)
        {
            Room r = Rooms[roomIndex];
            
            roomIndex++;
            for (int y = r.Bounds.Y; y < (r.Bounds.Y + r.Bounds.Height); y++)
            {
                for (int x = r.Bounds.X; x < r.Bounds.Right; x++)
                {
                    _roomMap[y, x] = roomIndex;
                }
            }
            for (int i = 0; i < r.Doors.Length; i++)
            {
                Vector2I pos = r.GetDoor(i);
                _roomMap[pos.Y, pos.X] = roomIndex;
            }
        }
        public Room GetRoom(int x, int y)
        {
            int m = _roomMap[y, x];
            return m < 1 ? null : Rooms[m - 1];
        }
        public bool TryMoveEntity(int x, int y, IEntity entity, out Room room)
        {
            if (_roomMap[y, x] == NoEntry)
            {
                room = GetRoom(entity.Position.X, entity.Position.Y);
                return false;
            }
            
            Room oldRoom = GetRoom(entity.Position.X, entity.Position.Y);
            
            entity.Position = (x, y);
            room = GetRoom(x, y);
            
            if (oldRoom != room)
            {
                oldRoom?.Leave(entity);
                room?.Enter(entity);
            }
            
            return true;
        }
        
        public bool Eluminatable(int x, int y)
        {
            int v = _roomMap[y, x];
            if (v > 0)
            {
                Room r = Rooms[v - 1];
                return r.Dark && !r.OnBoundary(x, y);
            }
            return false;
        }
        
        public Room GetRandomRoom() => Rooms[Program.RNG.Next(Rooms.Length)];
    }
}