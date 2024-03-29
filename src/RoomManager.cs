using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zene.Structs;

namespace RogueMod
{
    public sealed class RoomManager : IRoomManager
    {
        private struct MapUnit
        {
            public short Room;
            public byte Door;
            public byte Args;
            
            public bool Corridor => Room == RoomManager.Corridor;
            public bool CanEnter => Room != RoomManager.NoEntry && Args == 0;
            
            public bool Locked
            {
                get => (Args & RoomManager.Locked) == RoomManager.Locked;
                set
                {
                    if (value)
                    {
                        Args |= RoomManager.Locked;
                        return;
                    }
                    
                    Args &= (byte)(~RoomManager.Locked & 0xFF);
                }
            }
            public bool Hidden
            {
                get => (Args & RoomManager.Hidden) == RoomManager.Hidden;
                set
                {
                    if (value)
                    {
                        Args |= RoomManager.Hidden;
                        return;
                    }
                    
                    Args &= (byte)(~RoomManager.Hidden & 0xFF);
                }
            }
            
            public Door GetDoor(RoomManager rm)
            {
                IRoom r = rm.Rooms[Room - 1];
                return r[Door];
            }
            public bool IsLocked(RoomManager rm) => GetDoor(rm).Locked;
        }
        
        public const short Corridor = -1;
        public const byte Hidden = 0x01;
        public const byte Locked = 0x02;
        public const short NoEntry = 0;
        
        public RoomManager(Vector2I playingSize)
        {
            _size = playingSize;
            _roomMap = new MapUnit[playingSize.Y, playingSize.X];
        }
        
        private Vector2I _size;
        private MapUnit[,] _roomMap;
        
        public IRoom[] Rooms { get; private set; }
        public ICorridorManager CorridorManager { get; private set; }

        public int Level { get; private set; }
        
        private int _sqareRoot;
        private int _hiddenDoors;
        private int _lockedDoors;
        public void GenerateRooms(int level, IRogue game)
        {
            Level = level;
            _sqareRoot = (int)Math.Sqrt(Program.Properties.MaxRooms);
            _lockedDoors = 0;
            _hiddenDoors = 0;
            
            Rooms = new Room[
                Program.RNG.Next(
                    Program.Properties.MinRooms,
                    Program.Properties.MaxRooms)];
            
            int[] offsets = new int[Rooms.Length];
            int count = Program.Properties.MaxRooms - Rooms.Length;
            int current = 0;
            for (int i = 0; i < offsets.Length; i++)
            {
                bool should = (Program.RNG.Next(3) == 0 || i + count >= offsets.Length) && count > 0;
                if (should)
                {
                    count--;
                    current++;
                }
                offsets[i] = current;
            }
            for (int i = 0; i < Rooms.Length; i++)
            {
                Rooms[i] = GenerateRoom(i + offsets[i], level, game);
            }
            
            CorridorManager = new CorridorManager(new Corridor[0]);
            
            FillMapC();
            
            for (int i = 0; i < Rooms.Length; i++)
            {
                FillMap(i);
            }
        }
        public Room GenerateRoom(int index, int level, IRogue game)
        {            
            bool dark = Program.RNG.Next(10) < level;
            Vector2I ms = game.PlayingSize / _sqareRoot;
            RectangleI bounds = new RectangleI();
            Vector2I top = (index % _sqareRoot, index / _sqareRoot) * ms;
            top.X += 1;
            
            do
            {
                bounds.Width = Program.RNG.Next(ms.X - 4) + 4;
                bounds.Height = Program.RNG.Next(ms.Y - 4) + 4;
                bounds.X = top.X + Program.RNG.Next(ms.X - bounds.Width);
                bounds.Y = top.Y + Program.RNG.Next(ms.Y - bounds.Height);
            } while (bounds.Y == 0);
            
            int perimeter = bounds.Width * 2 + bounds.Height * 2 - 8;
            
            Door[] doors = new Door[Program.RNG.Next(perimeter / 8) + 1];
            for (int i = 0; i < doors.Length; i++)
            {
                int p;
                ReadOnlySpan<Door> previous = new ReadOnlySpan<Door>(doors, 0, i);
                
                do
                {
                    p = Program.RNG.Next(perimeter);
                } while (previous.Exists(d => d.PerimeterIndex == p));
                bool locked = false;
                bool hidden = false;
                if (Program.RNG.Next(Program.Properties.DoorLockedProb) == 0)
                {
                    locked = true;
                    _lockedDoors++;
                }
                if (Program.RNG.Next(Program.Properties.DoorHiddenProb) == 0)
                {
                    hidden = true;
                    _hiddenDoors++;
                }
                
                doors[i] = new Door(p, hidden, locked);
            }
            
            return new Room(bounds, dark, doors);
        }
        private void FillMap(int roomIndex)
        {
            IRoom r = Rooms[roomIndex];
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
                    _roomMap[y, x].Room = (short)roomIndex;
                }
            }
            // Doors
            for (int i = 0; i < r.DoorCount; i++)
            {
                Vector2I pos = r[i].GetLocation(r);
                _roomMap[pos.Y, pos.X].Room = (short)roomIndex;
                _roomMap[pos.Y, pos.X].Door = (byte)i;
                _roomMap[pos.Y, pos.X].Args = (byte)
                    ((r[i].Hidden ? Hidden : 0) |
                    (r[i].Locked ? Locked : 0));
            }
        }
        private void FillMapC()
        {
            foreach (Corridor c in CorridorManager)
            {
                Vector2I pos = c.Position;
                
                for (int j = 0; j < c.Length; j++)
                {
                    _roomMap[pos.Y, pos.X].Room = Corridor;
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
        private MapUnit MapIfCan(int x, int y)
        {
            if (x < 0 || x >= _size.X ||
                y < 0 || y >= _size.Y)
            {
                return default;
            }
            
            return _roomMap[y, x];
        }
        public void UnlockDoor(int x, int y, IOutput scr)
        {
            int i = _roomMap[y, x].Room - 1;
            IRoom r = Rooms[i];
            Door d = _roomMap[y, x].GetDoor(this);
            
            d.Locked = false;
            _roomMap[y, x].Locked = false;
        }
        public void ShowDoor(int x, int y, IOutput scr)
        {
            int i = _roomMap[y, x].Room - 1;
            IRoom r = Rooms[i];
            Door d = _roomMap[y, x].GetDoor(this);
            
            d.Hidden = false;
            _roomMap[y, x].Hidden = false;
            
            Vector2I pos = d.GetLocation(r);
            scr.Write(pos.X, pos.Y, Draw.Door);
        }
        
        public bool IsLocked(int x, int y) => (_roomMap[y, x].Args & Locked) == Locked;
        public Vector2I GetHitCast(Vector2I pos, Direction dir)
        {
            int start = _roomMap[pos.Y, pos.X].Room;
            int i = 0;
            
            Vector2I offset = dir.GetOffset();
            
            do
            {
                pos += offset;
                i = MapIfCan(pos.X, pos.Y).Room;
            } while (i == start);
            
            return pos;
        }
        public IRoom GetRandomRoom() => Rooms[Program.RNG.Next(Rooms.Length)];

        public LocationProperties GetProperties(int x, int y)
        {
            MapUnit mu = _roomMap[y, x];
            IRoom r = mu.Room > 0 ? Rooms[mu.Room - 1] : null;
            return new LocationProperties(!mu.CanEnter || mu.Door > 0, mu.CanEnter, mu.Locked, r);
        }

        public IEnumerator<IRoom> GetEnumerator() => Rooms.Cast<IRoom>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Rooms.GetEnumerator();
    }
}