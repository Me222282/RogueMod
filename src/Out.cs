using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public enum Colours
    {
        Normal,
        Red,
        Green,
        Brown,
        Blue,
        Magenta,
        Cyan,
        LightGrey,
        Grey,
        LightRed,
        LightGreen,
        Yellow,
        LightBlue,
        LightMagenta,
        LightCyan,
        White
    }
    public enum Draw
    {
        Door = '╬',
        WallV = '║',
        WallH = '═',
        WallTL = '╔',
        WallTR = '╗',
        WallBL = '╚',
        WallBR = '╝',
        Floor = '·',
        Player = '☺',
        Food = '♣',
        Amulet = '♀',
        Key = '♂',
        Scroll = '♪',
        Weapon = '↑',
        Armour = '◘',
        Gold = '☼',
        Staff = 'τ',
        Potion = '¡',
        Ring = '○',
        Passage = '▒',
        Trap = '♦',
        Magic = '$',
        MagicB = '~',
        PassageB = '▓',
        Stairs = '≡'
    }
    
    public static class Out
    {
        public static void Init(Vector2I size)
        {
            Curses.ResizeTerm(size.Y + 1, size.X);
            Curses.FlushInput();
            
            Width = size.X;
            Height = size.Y;
            Size = size;
        }
        
        public static Vector2I Size { get; private set; }
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        
        private static ColourF GetColour(int i)
        {
            return new ColourF(
                (2f/3f * (i & 1)/1f) + (1/3f * (i & 8)/8f),
                ((2f/3f * (i & 2)/2f) + (1/3f * (i & 8)/8f)) / ((i) == 3 ? 2 : 1),
                (2f/3f * (i & 4)/4f) + (1/3f * (i & 8)/8f)
            );
        }
        internal static void InitColours()
        {
            if (!Curses.HasColors) { return; }
            
            Curses.StartColor();
            
            if (Curses.CanChangeColor)
            {
                for (short i = 0; i < 8; i++)
                {
                    ColourF t = GetColour(i);
                    Curses.InitColor(i,
                        (short)(1000 * t.R),
                        (short)(1000 * t.G),
                        (short)(1000 * t.B));
                }
            }
            
            for (short i = 1; i < 8; ++i)
            {
                Curses.InitPair(i, i, 0);
            }
            // On grey
            for (short i = 1; i < 8; ++i)
            {
                Curses.InitPair((short)(i + 8), i, 7);
            }
        }
        
        public static void SetColour(Colours colour, bool reverse = false)
        {
            Stdscr.Attr = GetAttribute(colour, reverse);
        }
        public static void InvertColours()
        {
            if ((Stdscr.Attr & Attrs.REVERSE) == Attrs.REVERSE)
            {
                Stdscr.Attr &= ~Attrs.REVERSE;
                return;
            }
            Stdscr.Attr |= Attrs.REVERSE;
        }
        public static void ColourNormal() => Stdscr.Attr = Attrs.NORMAL;
        public static uint GetAttribute(Colours colour, bool reverse = false, bool onGrey = false)
        {
            uint s = reverse ? Attrs.REVERSE : Attrs.NORMAL;
            uint b = colour > Colours.LightGrey ? Attrs.BOLD : Attrs.NORMAL;
            int a = onGrey ? 8 : 0;
            return Curses.COLOR_PAIR((int)colour & 0x7 + a) | b | s;
        }
        public static uint GetAttribute(Draw c, bool onGrey = false)
        {
            switch (c)
            {
                case Draw.Door:
                case Draw.WallTL:
                case Draw.WallH:
                case Draw.WallBR:
                case Draw.WallBL:
                case Draw.WallV:
                case Draw.WallTR:
                    return GetAttribute(Colours.Brown, false, onGrey);
                
                case Draw.Floor:
                    return GetAttribute(Colours.Green, false, onGrey);
                    
                case Draw.Player:
                case Draw.Gold:
                case Draw.Key:
                    return GetAttribute(Colours.Yellow, false, onGrey);
                
                case Draw.Staff:
                case Draw.Weapon:
                case Draw.Armour:
                case Draw.Amulet:
                case Draw.Ring:
                case Draw.Scroll:
                case Draw.Potion:
                    return GetAttribute(Colours.Blue, false, onGrey);
                    
                case Draw.Food:
                    return GetAttribute(Colours.Red, false, onGrey);
                    
                case Draw.Trap:
                    return GetAttribute(Colours.Magenta, false, onGrey);
                    
                case Draw.Stairs:
                    return GetAttribute(Colours.Green, true, onGrey);
                    
                case Draw.Passage:
                case Draw.PassageB:
                case Draw.Magic:
                case Draw.MagicB:
                    return GetAttribute(Colours.Grey);
            }
            
            return Attrs.NORMAL;
        }
        
        public static void Write(int x, int y, char c, bool colour = false, uint attr = Attrs.NORMAL)
        {
            if (colour)
            {
                attr = GetAttribute((Draw)c);
            }
            
            Stdscr.Attr = attr;
            Stdscr.AddW(y, x, c);
        }
        public static void WriteB(int x, int y, char c)
        {
            Stdscr.Attr = GetAttribute((Draw)c, true);
            Stdscr.AddW(y, x, c);
        }
        public static void Write(int x, int y, string str, bool colour = false, uint attr = Attrs.NORMAL)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Write(x + i, y, str[i], colour, attr);
            }
        }
        
        public static void RenderBoxD(int x, int y, int w, int h)
        {
            SetColour(Colours.Brown);
            Stdscr.AddW(y, x, (char)Draw.WallTL);
            RenderLineH(x + 1, y, (char)Draw.WallH, w - 2);
            Stdscr.AddW((char)Draw.WallTR);
            RenderLineV(x, y + 1, (char)Draw.WallV, h - 2);
            RenderLineV(x + w - 1, y + 1, (char)Draw.WallV, h - 2);
            Stdscr.AddW(y + h - 1, x, (char)Draw.WallBL);
            RenderLineH(x + 1, y + h - 1, (char)Draw.WallH, w - 2);
            Stdscr.AddW((char)Draw.WallBR);
        }
        public static void RenderBoxD(RectangleI bounds)
            => RenderBoxD(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        public static void RenderBoxS(int x, int y, int w, int h)
        {
            SetColour(Colours.Green);
            Stdscr.AddW(y, x, '┌');
            RenderLineH(x + 1, y, '─', w - 2);
            Stdscr.AddW('┐');
            RenderLineV(x, y + 1, '│', h - 2);
            RenderLineV(x + w - 1, y + 1, '│', h - 2);
            Stdscr.AddW(y + h - 1, x, '└');
            RenderLineH(x + 1, y + h - 1, '─', w - 2);
            Stdscr.AddW('┘');
        }
        public static void RenderLineV(int x, int y, char c, int n, bool colour = false)
        {
            for (int i = 0; i < n; i++)
            {
                if (colour)
                {
                    Stdscr.Attr = GetAttribute((Draw)c);
                }
                Stdscr.AddW(y + i, x, c);
            }
        }
        public static void RenderLineH(int x, int y, char c, int n, bool colour = false)
        {
            Stdscr.Move(y, x);
            for (int i = 0; i < n; i++)
            {
                if (colour)
                {
                    Stdscr.Attr = GetAttribute((Draw)c);
                }
                Stdscr.AddW(c);
            }
        }
        public static void Centre(int y, string str)
        {
            Stdscr.Add(y, (Curses.Cols - str.Length) / 2, str);
        }
        
        public static void Fill(int x, int y, int w, int h, char ch, bool colour = false)
        {
            for (int i = 0; i < h; i++)
            {
                RenderLineH(x, y + i, ch, w, colour);
            }
        }
        
        public enum Return
        {
            Continue,
            Break,
            Return
        }
        public delegate Return OnChar(int ch);
        public static OnChar DefaultOnChar =>
            ch => ch switch
            {
                Keys.ESC => Return.Return,
                ' ' => Return.Break,
                _ => Return.Continue
            };
        public static void PrintList(string[] list, bool vertical, OnChar manager)
        {
            int hw = Width / 2;
            for (int i = 0, j = 0; j < list.Length; i++, j++)
            {
                int x = 0;
                int y = i;
                if (!vertical)
                {
                    x = i % 2 == 0 ? 0 : hw;
                    y = i / 2;
                }
                
                if (y >= (Height - 2))
                {
                    Out.ColourNormal();
                    Stdscr.Add(Height - 1, 0, "--Press space for more, Esc to continue--");
                    while (true)
                    {
                        Return r = manager(Stdscr.GetChar());
                        if (r == Return.Return) { return; }
                        if (r == Return.Continue) { continue; }
                        break;
                    }
                    Stdscr.Clear();
                    i = -1;
                    j--;
                    continue;
                }
                
                if (list[j] == null) { i--; continue; }
                
                Write(x, y, list[j], true);
            }
            
            Out.ColourNormal();
            Stdscr.Add(Height - 1, 0, "--Press space to continue--");
            Return sp;
            do
            {
                sp = manager(Stdscr.GetChar());
            } while (sp == Return.Continue);
        }
        
        public static void Animate(Vector2I start, Vector2I end, char c)
        {
            
        }
    }
}