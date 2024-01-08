using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public class Rogue
    {
        public Rogue(Vector2I size)
        {
            PlayingSize = size - (0, 3);
            Out = new VirtualScreen(size);
        }
        
        public VirtualScreen Out { get; }
        
        public Vector2I PlayingSize { get; }
        public string PlayerName { get; set; } = "";
        
        public Discoveries Discoveries { get; } = new Discoveries();
        public Mapping NameMaps { get; } = new Mapping();
        
        public Room[] Rooms { get; private set; }
        
        public void Render()
        {
            Stdscr.Clear();
            for (int i = 0; i < Rooms.Length; i++)
            {
                Rooms[i].Render(true, Out);
            }
        }
        
        public void GenRooms(int level)
        {
            Rooms = new Room[]
            {
                new Room(new RectangleI(2, 2, 10, 10), false,
                    new Door[] { new Door(4, false), new Door(3, true) })
            };
        }
    }
}