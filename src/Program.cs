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
            game.RoomManager.Rooms[0].Enter(new ItemEntity(Ring.Create()));
            
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
            if (ch != 'a')
            {
                _lastAction = ch;
            }
            
            switch (ch)
            {
                case 'm':
                    Message.Push("This is the first message");
                    Message.Push("The next message");
                    Message.Push("Last but not least!");
                    return;
                case (Keys.F0 + 4):
                    Message.PushLastMessage();
                    return;
                case (Keys.F0 + 1):
                    Out.ColourNormal();
                    Stdscr.Clear();
                    PrintList(_controlVisual, 37);
                    game.Out.Print();
                    return;
                case (Keys.F0 + 2):
                    Out.ColourNormal();
                    Stdscr.Clear();
                    PrintList(_symbolVisual, 37);
                    game.Out.Print();
                    return;
                case (Keys.F0 + 3):
                case 'a':
                    ManageKeyInput(_lastAction, game);
                    return;
                case Keys.UP:
                case Keys.DOWN:
                case Keys.LEFT:
                case Keys.RIGHT:
                case Keys.PPAGE:
                case Keys.NPAGE:
                case Keys.END:
                case Keys.HOME:
                    game.TryMovePlayer((Direction)ch);
                    return;
            }
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
        private static void PrintList(string[] list, int minX)
        {
            int hw = Out.Width / 2;
            bool vertical = hw < minX;
            for (int i = 0, j = 0; j < list.Length; i++, j++)
            {
                int x = 0;
                int y = i;
                if (!vertical)
                {
                    x = i % 2 == 0 ? 0 : hw;
                    y = i / 2;
                }
                
                if (y >= (Out.Height - 2))
                {
                    Stdscr.Add(Out.Height - 1, 0, "--Press space for more, Esc to continue--");
                    while (true)
                    {
                        int ch = Stdscr.GetChar();
                        if (ch == Keys.ESC) { return; }
                        if (ch != ' ') { continue; }
                        break;
                    }
                    Stdscr.Clear();
                    i = -1;
                    j--;
                    continue;
                }
                
                Out.Write(x, y, list[j], true);
            }
            
            Stdscr.Add(Out.Height - 1, 0, "--Press space to continue--");
            int sp;
            do
            {
                sp = Stdscr.GetChar();
            } while (sp != ' ' && sp != Keys.ESC);
        }
        
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
