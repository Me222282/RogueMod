using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public enum Direction
    {
        Up = Keys.UP,
        Down = Keys.DOWN,
        Left = Keys.LEFT,
        Right = Keys.RIGHT,
        UpRight = Keys.PPAGE,
        DownRight = Keys.NPAGE,
        UpLeft = Keys.HOME,
        DownLeft = Keys.END
    }
    
    public class Rogue
    {
        public Rogue(Vector2I size)
        {
            PlayingSize = size - (0, 3);
            Out = new VirtualScreen(size);
            
            RoomManager = new RoomManager(PlayingSize);
            
            Player = new Player();
            RoomManager.GenRooms(1);
            
            Room playerRoom = RoomManager.GetRandomRoom();
            Player.Position = playerRoom.GetNextPosition();
            playerRoom.Entities.Add(Player);
            Eluminate(Player.Position.X, Player.Position.Y, true);
            CurrentRoom = playerRoom;
            Eluminate(playerRoom);
        }
        
        public VirtualScreen Out { get; }
        
        public Vector2I PlayingSize { get; }
        public string PlayerName { get; set; } = "";
        
        public Discoveries Discoveries { get; } = new Discoveries();
        public Mapping NameMaps { get; } = new Mapping();
        
        public RoomManager RoomManager { get; }
        public Player Player { get; }
        public Room CurrentRoom { get; private set; }
        
        public void Render()
        {
            Stdscr.Clear();
            for (int i = 0; i < RoomManager.Rooms.Length; i++)
            {
                Room r = RoomManager.Rooms[i];
                r.Render(Out, r == CurrentRoom);
            }
        }
        public bool TryMoveEntity(int x, int y, IEntity entity, out Room room)
        {
            Vector2I oldPos = entity.Position;
            bool success = RoomManager.TryMoveEntity(x, y, entity, out room);
            if (!success) { return false; }
            
            if (entity is ICharacter c && c.PickupItems && !c.Backpack.IsFull)
            {
                ItemEntity item = room.GetItemEntity(x, y);
                if (item != null)
                {
                    if (c is Player)
                    {
                        Program.Message.Push("you gained an item");
                    }
                    
                    c.Backpack.Add(item.Item);
                    room.Leave(item);
                    entity.UnDraw(Out);
                }
            }
            
            Out.Write(oldPos.X, oldPos.Y, entity.UnderChar);
            entity.UnderChar = Out.Read(x, y);
            Out.Write(x, y, entity.Graphic);
            
            return true;
        }
        public bool TryMovePlayer(Direction dir)
        {
            Vector2I offset = dir switch
            {
                Direction.Up => (0, -1),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                Direction.Right => (1, 0),
                Direction.UpRight => (1, -1),
                Direction.DownRight => (1, 1),
                Direction.UpLeft => (-1, -1),
                Direction.DownLeft => (-1, 1),
                _ => throw new Exception()
            };
            
            Vector2I oldPos = Player.Position;
            int x = oldPos.X + offset.X;
            int y = oldPos.Y + offset.Y;
            
            bool success = TryMoveEntity(x, y, Player, out Room newRoom);
            if (!success) { return false; }
            
            if (CurrentRoom != null && CurrentRoom.Dark)
            {
                Eluminate(oldPos.X, oldPos.Y, false);
            }
            // Eluminate new position - always incase standing in doorway
            Eluminate(x, y, true);
            
            if (CurrentRoom != newRoom)
            {
                // Render old room without entities
                CurrentRoom.RenderEntites(Out, false);
                
                CurrentRoom = newRoom;
                Eluminate(newRoom);
                
                // Render new room with entites
                newRoom.RenderEntites(Out, true);
            }
            
            return true;
        }
        
        private void Eluminate(int x, int y, bool value)
        {
            int max1 = Math.Min(PlayingSize.Y - 1, y + 1);
            int max2 = Math.Min(PlayingSize.X - 1, x + 1);
            for (int i1 = Math.Max(0, y - 1); i1 <= max1; i1++)
            {
                for (int i2 = Math.Max(0, x - 1); i2 <= max2; i2++)
                {
                    Out[i2, i1] = !(!value && RoomManager.Eluminatable(i2, i1));
                }
            }
        }
        public void Eluminate(Room room)
        {
            if (room.Dark)
            {
                // Show previously seen entities
                foreach (IEntity e in room.Entities)
                {
                    if (!e.Seen) { continue; }
                    Out[e.Position.X, e.Position.Y] = true;
                }
                return;
            }
            
            // All entities have now been seen
            foreach (IEntity e in room.Entities)
            {
                e.Seen = true;
            }
            
            // Eluminate room
            for (int y = room.Bounds.Y; y < (room.Bounds.Y + room.Bounds.Height); y++)
            {
                for (int x = room.Bounds.X; x < room.Bounds.Right; x++)
                {
                    Out[x, y] = true;
                }
            }
        }
    }
}