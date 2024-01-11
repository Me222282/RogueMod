using System;
using Zene.Structs;

namespace RogueMod
{
    public sealed class RoomManager
    {
        public const int Corridor = -1;
        public const int LockedDoor = -2;
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
                new Room(new RectangleI(2, 2, 60, 20), true,
                    new Door[] { new Door(4, false, false, true), new Door(3, true) })
            };
            FillMap(0);
        }
        private void FillMap(int roomIndex)
        {
            Room r = Rooms[roomIndex];
            RectangleI innerBounds = new RectangleI(
                r.Bounds.X + 1,
                r.Bounds.Y + 1,
                r.Bounds.Width - 2,
                r.Bounds.Height - 2);
            
            roomIndex++;
            // Fill room
            for (int y = innerBounds.Y; y < (innerBounds.Y + innerBounds.Height); y++)
            {
                for (int x = innerBounds.X; x < innerBounds.Right; x++)
                {
                    _roomMap[y, x] = roomIndex;
                }
            }
            // Doors
            for (int i = 0; i < r.Doors.Length; i++)
            {
                if (r.Doors[i].Hidden) { continue; }
                int num = r.Doors[i].Locked ? LockedDoor : roomIndex;
                Vector2I pos = r.GetDoor(i);
                _roomMap[pos.Y, pos.X] = num;
            }
        }
        public Room GetRoom(int x, int y)
        {
            int m = _roomMap[y, x];
            return m < 1 ? null : Rooms[m - 1];
        }
        public bool TryMoveEntity(int x, int y, IEntity entity, out Room room)
        {
            if (_roomMap[y, x] == NoEntry || _roomMap[y, x] == LockedDoor)
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
                return r.Dark && !r.OnBoundary(x, y) && !r.SeeEntity(x, y);
            }
            return false;
        }
        
        public void UnlockDoor(Room r, int dr)
        {
            Door d = r.Doors[dr];
            
            d.Locked = false;
            if (d.Hidden) { return; }
            
            int i = Array.IndexOf(Rooms, r);
            Vector2I pos = r.GetDoor(dr);
            _roomMap[pos.Y, pos.X] = i;
        }
        public void ShowDoor(Room r, int dr, VirtualScreen scr)
        {
            Door d = r.Doors[dr];
            
            d.Hidden = false;
            
            int i = d.Locked ? LockedDoor : Array.IndexOf(Rooms, r);
            Vector2I pos = r.GetDoor(dr);
            _roomMap[pos.Y, pos.X] = i;
            
            d.Draw(r, scr);
        }
        
        public Room GetRandomRoom() => Rooms[Program.RNG.Next(Rooms.Length)];
    }
}