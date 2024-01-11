using System;
using Zene.Structs;
using CursesSharp;

namespace RogueMod
{
    public class Program
    {
        public static Random RNG = new Random();
        public static MessageManager Message = new MessageManager();
        public static Properties Properties;
        
        private static void Main(string[] args)
        {
            Curses.InitScr();
            
            Curses.Echo = false;
            Curses.CursorVisibility = 0;
            Stdscr.Keypad = true;
            Stdscr.Blocking = true;
            
            Properties = new Properties();
            
            Out.Init(Properties.Size);
            Out.InitColours();
            Stdscr.Clear();
            
            Splash();
            
            Rogue game = new Rogue(Properties.Size);
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Ring.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Ring.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Scroll.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Scroll.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Staff.Create(game)));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Staff.Create(game)));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Potion.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Potion.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Gold(100)));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Gold(30)));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Food.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Food.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Weapon.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Weapon.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Armour.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(Armour.Create()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Key()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Key()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Amulet()));
            game.RoomManager.Rooms[0].PlaceItem(new ItemEntity(new Amulet()));
            
            game.Render();
            
            Curtain(game);
            game.Out.PrintDirect = true;
            
            while (true)
            {
                int cki = Stdscr.GetChar();
                if (cki == 'Q')
                {
                    if (IsQuit()) { break; }
                    
                    continue;
                }
                
                Message.Clear();
                ManageKeyInput(cki, game);
                Message.Manage();
            }
            
            Curses.EndWin();
        }
        private static bool IsQuit()
        {
            Out.ColourNormal();
            Stdscr.Add(0, 0, "Do you wish to end your quest now (");
            Out.InvertColours();
            Stdscr.Add('Y');
            Out.InvertColours();
            Stdscr.Add("es/");
            Out.InvertColours();
            Stdscr.Add('N');
            Out.InvertColours();
            Stdscr.Add("o) ?");
            
            int cki = Stdscr.GetChar();
            Stdscr.Move(0, 0);
            Stdscr.ClearToEol();
            return char.ToLower((char)cki) == 'y';
        }
        
        public static string GetMod(int i)
        {
            if (i < 0)
            {
                return $"-{Math.Abs(i)}";
            }
            
            return $"+{i}";
        }
        
        private static int _lastAction;
        private static void ManageKeyInput(int ch, Rogue game)
        {
            if (ch != Controls.Repeat &&
                ch != Controls.RepeatB)
            {
                _lastAction = ch;
            }
            
            switch (ch)
            {
                case Controls.LastMessage:
                case Controls.LastMessageB:
                    Message.PushLastMessage();
                    return;
                case Controls.ControlVis:
                    Stdscr.Clear();
                    Out.PrintList(_controlVisual, (Out.Width / 2) < 37, Out.DefaultOnChar);
                    game.Out.Print();
                    return;
                case Controls.SymbolVis:
                    Stdscr.Clear();
                    Out.PrintList(_symbolVisual, (Out.Width / 2) < 37, Out.DefaultOnChar);
                    game.Out.Print();
                    return;
                case Controls.Inventory:
                    SelectItem(game, ItemType.Any);
                    return;
                case Controls.Wear:
                    if (game.Player.Backpack.IsWearing)
                    {
                        Message.Push("You are already wearing some. You'll have to take it off first.");
                        return;
                    }
                    char wear = SelectItemChar(game, ItemType.Armour);
                    try
                    {
                        game.Player.Backpack.Wear(wear);
                        IItem w = game.Player.Backpack.Wearing;
                        w.MakeKnown(game);
                        Message.Push($"You are now wearing {w.ToString(game, false)}");
                    }
                    catch (Exception) { }
                    return;
                case Controls.TakeOff:
                    if (!game.Player.Backpack.IsWearing)
                    {
                        Message.Push("You aren't wearing any amour");
                        return;
                    }
                    IItem oldWear = game.Player.Backpack.Wearing;
                    if (!game.Player.Backpack.Wear('\0'))
                    {
                        Message.Push("Your amour is cursed!");
                        return;
                    }
                    char c = game.Player.Backpack.GetChar(oldWear);
                    Message.Push($"You used to be wearing {c}) {oldWear.ToString(game, false)}");
                    return;
                case Controls.Drop:
                    IItem item = SelectItem(game, ItemType.Any);
                    IItem drop = item.Copy();
                    if (item is null) { return; }
                    if (item is Weapon && item.Stackable)
                    {
                        game.Player.Backpack.DropAll(item);
                    }
                    else
                    {
                        game.Player.Backpack.DropOne(item);
                    }
                    ItemEntity ie = new ItemEntity(drop, game.Player.Position);
                    ie.UnderChar = game.Player.UnderChar;
                    game.CurrentRoom.Enter(ie);
                    game.Player.UnderChar = ie.Graphic;
                    return;
                case Controls.RepeatB:
                case Controls.Repeat:
                    ManageKeyInput(_lastAction, game);
                    return;
                case (int)Direction.Up:
                case (int)Direction.Down:
                case (int)Direction.Left:
                case (int)Direction.Right:
                case (int)Direction.UpLeft:
                case (int)Direction.UpRight:
                case (int)Direction.DownLeft:
                case (int)Direction.DownRight:
                    game.TryMovePlayer((Direction)ch);
                    return;
                case 'l':
                    game.CurrentRoom.Dark = false;
                    game.Eluminate(game.CurrentRoom);
                    return;
            }
        }
        private static IItem SelectItem(Rogue game, ItemType type)
        {
            char c = SelectItemChar(game, type);
            if (c == '\0') { return null; }
            
            IItem item = null;
            try
            {
                item = game.Player.Backpack[c];
            }
            catch (Exception) { }
            
            if (item is null || (type != ItemType.Any && item.Type != type))
            {
                return null;
            }
            
            return item;
        }
        private static char SelectItemChar(Rogue game, ItemType type)
        {   
            if (game.Player.Backpack.IsEmpty)
            {
                Message.Push("You are empty handed");
                return '\0';
            }
            
            char c = '\0';
            
            Stdscr.Clear();
            Out.PrintList(game.Player.Backpack.GetItemList(game), true, ch =>
            {
                if (ch == Keys.ESC) { return Out.Return.Return; }
                if (ch == ' ') { return Out.Return.Break; }
                if (char.IsLetter((char)ch))
                {
                    c = char.ToLower((char)ch);
                    return Out.Return.Break;
                }
                
                return Out.Return.Continue;
            });
            game.Out.Print();
            
            return c;
        }
        
        private static readonly string[] _controlVisual = new string[]
        {
            "F1     list of commands",
            "F2     list of symbols",
            "F3     repeat command",
            "F4     repeat message",
            "F5     rename something",
            "F6     recall what's been discovered",
            "F7     inventory of your possessions",
            "F8     <dir> identify trap type",
            "F9     The Any Key (definable)",
            "Alt F9 defines the Any Key",
            "F10    Supervisor Key (fake dos)",
            "Space  Clear -More- message",
            "◄┘     the Enter Key",
            "←      left",
            "↓      down",
            "↑      up",
            "→      right",
            "Home   up & left",
            "PgUp   up & right",
            "End    down & left",
            "PgDn   down & right",
            "Scroll Fast Play mode",
            ".      rest",
            ">      go down a staircase",
            "<      go up a staircase",
            "Esc    cancel command",
            "d      drop object",
            "e      eat food",
            "f      <dir> find something",
            "q      quaff potion",
            "r      read paper",
            "s      search for trap/secret door",
            "t      <dir> throw something",
            "w      wield a weapon",
            "z      <dir> zap with a wand",
            "B      run down & left",
            "H      run left",
            "J      run down",
            "K      run up",
            "L      run right",
            "N      run down & right",
            "U      run up & right",
            "Y      run up & left",
            "W      wear armor",
            "T      take armor off",
            "P      put on ring",
            "Q      quit",
            "R      remove ring",
            "S      save game",
            "^      identify trap",
            "?      help",
            "/      key",
            "+      throw",
            "-      zap",
            "Ctrl t terse message format",
            "Ctrl r repeat message",
            "Del    search for something hidden",
            "Ins    <dir> find something",
            "a      repeat command",
            "c      rename something",
            "i      inventory",
            "v      version number",
            "!      Supervisor Key (fake DOS)",
            "D      list what has been discovered",
        };
        private static readonly string[] _symbolVisual = new string[]
        {
            $"{(char)Draw.Floor}: the floor",
            $"{(char)Draw.Player}: the hero",
            $"{(char)Draw.Food}: some food",
            $"{(char)Draw.Amulet}: the amulet of yendor",
            $"{(char)Draw.Key}: a key",
            $"{(char)Draw.Scroll}: a scroll",
            $"{(char)Draw.Weapon}: a weapon",
            $"{(char)Draw.Armour}: a piece of armor",
            $"{(char)Draw.Gold}: some gold",
            $"{(char)Draw.Staff}: a magic staff",
            $"{(char)Draw.Potion}: a potion",
            $"{(char)Draw.Ring}: a magic ring",
            $"{(char)Draw.PassageB}: a passage",
            $"{(char)Draw.Door}: a door",
            $"{(char)Draw.WallTL}: an upper left corner",
            $"{(char)Draw.Trap}: a trap",
            $"{(char)Draw.WallH}: a horizontal wall",
            $"{(char)Draw.WallBR}: a lower right corner",
            $"{(char)Draw.WallBL}: a lower left corner",
            $"{(char)Draw.WallV}: a vertical wall",
            $"{(char)Draw.WallTR}: an upper right corner",
            $"{(char)Draw.Stairs}: a stair case",
            $"{(char)Draw.Magic},{(char)Draw.MagicB}: safe and perilous magic",
            "A-Z: 26 different monsters"
        };
        
        private static string Splash()
        {   
            Out.RenderBoxD(0, 0, Out.Width, Out.Height);
            
            Out.SetColour(Colours.Normal, true);
            Out.Centre(2, "ROGUE:  The Adventure Game");
            Out.SetColour(Colours.Magenta);
            Out.Centre(4, "The game of Rogue was designed by:");
            Out.SetColour(Colours.White);
            Out.Centre(6, "Michael Toy and Glenn Wichman");
            Out.SetColour(Colours.Magenta);
            Out.Centre(9, "Various implementations by:");
            Out.SetColour(Colours.White);
            Out.Centre(11, "Ken Arnold, Jon Lane, Michael Toy and Owen Fraser");
            Out.SetColour(Colours.Magenta);
            Out.Centre(14, "Adapted for the IBM PC by:");
            Out.SetColour(Colours.White);
            Out.Centre(16, "A.I. Design");
            Out.SetColour(Colours.Yellow);
            Out.Centre(19, "(C)Copyright 1985");
            Out.SetColour(Colours.White);
            Out.Centre(20, "Epyx Incorporated");
            Out.SetColour(Colours.Yellow);
            Out.Centre(21, "All Rights Reserved");
            
            Out.SetColour(Colours.Brown);
            Stdscr.AddW(22, 0, '╠');
            Out.RenderLineH(1, 22, (char)Draw.WallH, Out.Width - 2);
            Stdscr.AddW('╣');
            Stdscr.Standend();
            
            Stdscr.Refresh();
            
            Stdscr.Add(23, 2, "Rogue's Name? ");
            Out.SetColour(Colours.White);
            Curses.Echo = true;
            Curses.CursorVisibility = 1;
            string name =  Stdscr.GetString(23);
            Curses.Echo = false;
            Curses.CursorVisibility = 0;
            return name;
        }
        private static void Curtain(Rogue game)
        {
            int delay = Properties.CurtainTime / Out.Height;
            
            Out.RenderBoxS(0, 0, Out.Width, Out.Height);
            Stdscr.Refresh();
            Curses.NapMs(delay);
            
            // Lower curtain
            Out.SetColour(Colours.Yellow);
            for (int l = 1; l < Out.Height - 1; l++)
            {
                Out.RenderLineH(1, l, (char)Draw.Passage, Out.Width - 2);
                Stdscr.Refresh();
                Curses.NapMs(delay);
            }
            Curses.NapMs(delay);
            
            // Raise curtain
            
            for (int l = Out.Height - 1; l >= 0; l--)
            {
                game.Out.PrintLine(l);
                Stdscr.Refresh();
                Curses.NapMs(delay);
            }
        }
    }
}
