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
        public CorridorManager CorridorManager { get; private set; }
        
        public void GenRooms(int level)
        {
            Rooms = new Room[]
            {
                new Room(new RectangleI(2, 2, 60, 20), true,
                    new Door[] { new Door(4, false, false, true), new Door(3, true) })
            };
            CorridorManager = new CorridorManager(new Corridor[]
            {
                new Corridor((6, 0), (6, 1)),
                new Corridor((6, 0), (0, 0)),
                new Corridor((0, 0), (0, 5)),
                new Corridor((0, 5), (1, 5))
            });
            
            FillMapC();
            
            for (int i = 0; i < Rooms.Length; i++)
            {
                FillMap(i);
            }
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
        private void FillMapC()
        {
            for (int i = 0; i < CorridorManager.Corridors.Length; i++)
            {
                Corridor c = CorridorManager.Corridors[i];
                Vector2I pos = c.Position;
                
                for (int j = 0; j < c.Length; j++)
                {
                    _roomMap[pos.Y, pos.X] = Corridor;
                    if (c.Vertical)
                    {
                        pos.Y++;
                    }
                    else
                    {
                        pos.X++;
                    }
                }
            }
        }
        public IEntityContainer GetEntityContainer(int x, int y)
        {
            int m = _roomMap[y, x];
            if (m == Corridor)
            {
                return CorridorManager;
            }
            return m < 1 ? null : Rooms[m - 1];
        }
        public bool TryMoveEntity(int x, int y, IEntity entity, out IEntityContainer room)
        {
            if (x < 0 || x >= _size.X ||
                y < 0 || y >= _size.Y)
            {
                room = null;
                return false;
            }
            
            if (_roomMap[y, x] == NoEntry || _roomMap[y, x] == LockedDoor)
            {
                room = GetEntityContainer(entity.Position.X, entity.Position.Y);
                return false;
            }
            
            IEntityContainer oldRoom = GetEntityContainer(entity.Position.X, entity.Position.Y);
            
            entity.Position = (x, y);
            room = GetEntityContainer(x, y);
            
            if (oldRoom != room)
            {
                oldRoom.Leave(entity);
                room.Enter(entity);
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
        
        private int MapIfCan(int x, int y)
        {
            if (x < 0 || x >= _size.X ||
                y < 0 || y >= _size.Y)
            {
                return 0;
            }
            
            return _roomMap[y, x];
        }
        private int GetDoorRoom(int x, int y)
        {
            int a = MapIfCan(x + 1, y);
            int b = MapIfCan(x, y + 1);
            int c = MapIfCan(x - 1, y);
            int d = MapIfCan(x, y - 1);
            
            return Math.Max(Math.Max(a, b), Math.Max(c, d)) - 1;
        }
        public IEntityContainer GetRoom(Vector2I pos)
        {
            int v = _roomMap[pos.Y, pos.X];
            if (v == Corridor)
            {
                return CorridorManager;
            }
            if (v == NoEntry || v == LockedDoor) { return null; }
            
            return Rooms[v - 1];
        }
        public void UnlockDoor(int x, int y)
        {
            int i = GetDoorRoom(x, y);
            Room r = Rooms[i];
            Door d = r.Doors[r.GetDoor(x, y)];
            
            d.Locked = false;
            if (d.Hidden) { return; }
            
            _roomMap[y, x] = i + 1;
        }
        public void ShowDoor(int x, int y, VirtualScreen scr)
        {
            int i = GetDoorRoom(x, y);
            Room r = Rooms[i];
            Door d = r.Doors[r.GetDoor(x, y)];
            
            d.Hidden = false;
            
            i = d.Locked ? LockedDoor : i + 1;
            _roomMap[y, x] = i;
            
            d.Draw(r, scr);
        }
        
        public bool IsLocked(int x, int y) => _roomMap[y, x] == LockedDoor;
        
        public IEntity GetEntity(IEntityContainer ec, Func<IEntity, int> compare)
        {
            int best = int.MaxValue;
            IEntity b = null;
            
            foreach (IEntity e in ec)
            {
                int l = compare(e);
                if (l < 0 || l >= best) { continue; }
                
                best = l;
                b = e;
            }
            
            return b;
        }
        public Vector2I GetHit(Vector2I pos, Direction dir)
        {
            int start = _roomMap[pos.Y, pos.X];
            int i = 0;
            
            Vector2I offset = dir.GetOffset();
            
            do
            {
                pos += offset;
                i = MapIfCan(pos.X, pos.Y);
            } while (i == start);
            
            return pos;
        }
        
        public Room GetRandomRoom() => Rooms[Program.RNG.Next(Rooms.Length)];
    }
}