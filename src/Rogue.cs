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
    
    public class Rogue : IRogue
    {
        public Rogue(Vector2I size, IMessageManager messages)
        {
            PlayingSize = size - (0, 3);
            Out = new GameOutput(size - (0, 1), 2);
            Message = messages;
            
            RoomManager = new RoomManager(PlayingSize);
            EntityManager = new EntityManager(PlayingSize);
            
            Player = new Player();
            RoomManager.GenerateRooms(1, this);
            Out.FillCorridorMap(RoomManager.CorridorManager, 1);
            
            CurrentRoom = RoomManager.GetRandomRoom();
            Vector2I pos = this.GetRandomPosition(CurrentRoom);
            EntityManager.Place(pos, Player);
            Eluminate(Player.Position.X, Player.Position.Y, true);
            Eluminate(CurrentRoom);
        }
        
        private IMessageManager Message { get; }
        public IGameOutput Out { get; }
        
        public Vector2I PlayingSize { get; }
        
        public Discoveries Discoveries { get; } = new Discoveries();
        
        public IRoomManager RoomManager { get; }
        public IEntityManager EntityManager { get; }
        public IPlayer Player { get; }
        public IRoom CurrentRoom { get; private set; }
        
        public void Render()
        {
            // Clear layers without vis
            Out[0].Clear();
            Out[1].Clear();
            // Readd corridoor map
            Out.FillCorridorMap(RoomManager.CorridorManager, 1);
            
            foreach (IRoom r in RoomManager)
            {
                r.Render(Out[0]);
            }
            
            RoomManager.CorridorManager.Render(Out[0]);
            EntityManager.Render(new RectangleI((0, 0), PlayingSize), Out[1]);
        }
        public bool TryMoveCharacter(int x, int y, ICharacter character)
        {
            Vector2I oldPos = character.Position;
            LocationProperties lp = RoomManager.GetProperties(x, y);
            bool success =  lp.CanEnter;
            ICharacter hit = EntityManager.GetCharacter(x, y);
            success |= hit is not null && hit.Character;
            if (!success) { return false; }
            
            EntityManager.Shift(oldPos, (x, y));
            
            if (character.PickupItems && !character.Backpack.IsFull)
            {
                IItem item = EntityManager.GetItem(x, y);
                if (item is not null)
                {
                    if (character is Player)
                    {
                        Message.Push("you gained an item");
                    }
                    
                    character.Backpack.Add(item);
                    EntityManager.Delete(x, y, false);
                }
            }
            
            Out[1].Write(oldPos.X, oldPos.Y, ' ');
            Out[1].Write(x, y, character.Graphic);
            
            return true;
        }
        public bool TryMovePlayer(Direction dir)
        {
            Vector2I offset = dir.GetOffset();
            
            Vector2I oldPos = Player.Position;
            int x = oldPos.X + offset.X;
            int y = oldPos.Y + offset.Y;
            
            if (x < 0 || x >= PlayingSize.X ||
                y < 0 || y >= PlayingSize.Y)
            {
                return false;
            }
            
            bool success = TryMoveCharacter(x, y, Player);
            LocationProperties lp = RoomManager.GetProperties(x, y);
            if (!success)
            {
                if (!lp.IsLocked) { return false; }
                if (!Player.Backpack.DropOne(new Key()))
                {
                    Message.Push(Messages.DoorLocked);
                    return false;
                }
                
                RoomManager.UnlockDoor(x, y, Out[0]);
                
                Message.Push(Messages.UnlockedDoor);
                return false;
            }
            
            if (CurrentRoom != null && CurrentRoom.Dark)
            {
                Eluminate(oldPos.X, oldPos.Y, false);
            }
            // Eluminate new position - always incase standing in doorway
            Eluminate(x, y, true);
            
            IRoom newRoom = lp.Room;
            
            if (CurrentRoom != newRoom)
            {
                // Hide entities in old room
                if (CurrentRoom is not null)
                {
                    Out[1].Fill(CurrentRoom.Bounds, ' ');
                }
                
                CurrentRoom = newRoom;
                
                if (newRoom is not null)
                {
                    Eluminate(newRoom);
                    
                    // Render entities in new room
                    EntityManager.Render(newRoom.Bounds, Out[1]);
                }
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
                    LocationProperties lp = RoomManager.GetProperties(i2, i1);
                    
                    if (value && lp.ShouldEluminate)
                    {
                        Out[i2, i1] = true;
                        continue;
                    }
                    if (!value && lp.ShouldHide)
                    {
                        Out[i2, i1] = false;
                        continue;
                    }
                }
            }
        }
        public void Eluminate(IRoom room)
        {
            if (room is null) { return; }
            
            if (room.Dark) { return; }
            
            // Eluminate room
            for (int y = room.Bounds.Y; y < (room.Bounds.Y + room.Bounds.Height); y++)
            {
                for (int x = room.Bounds.X; x < room.Bounds.Right; x++)
                {
                    Out[x, y] = true;
                }
            }
        }   
        
#if DEBUG
        public static Rogue GetTest(IRoomManager rm, IEntityManager em, IGameOutput o, IMessageManager m, IPlayer p)
        {
            return new Rogue(rm, em, o, m, p);
        }
        private Rogue(IRoomManager rm, IEntityManager em, IGameOutput o, IMessageManager m, IPlayer p)
        {
            PlayingSize = o.Size;
            Message = m;
            Out = o;
            RoomManager = rm;
            EntityManager = em;
            CurrentRoom = p is not null ? rm.GetProperties(p.Position.X, p.Position.Y).Room : null;
            Player = p;
        }
#endif
    }
}