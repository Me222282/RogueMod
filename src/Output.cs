using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public sealed class Output : IOutput, IDisposable
    {
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
        
        public Output(Vector2I size)
        {
            Screen = Curses.InitScr();
            
            Curses.Echo = false;
            Curses.CursorVisibility = 0;
            Screen.Keypad = true;
            Screen.Blocking = true;
            
            Curses.ResizeTerm(size.Y + 1, size.X);
            Curses.FlushInput();
            Size = size;
            
            InitColours();
        }
        
        public Window Screen { get; }
        public Vector2I Size { get; }
        
        public uint Attribute
        {
            get => Screen.Attr;
            set => Screen.Attr = value;
        }
        
        public void Write(int x, int y, char c)
        {
            //Screen.Attr = Out.GetAttribute((Draw)c, false);
            Screen.Move(y, x);
            Screen.AddW(c);
        }
        public void Write(int x, int y, char c, uint attribute)
        {
            Screen.Attr = attribute;
            Screen.Move(y, x);
            Screen.AddW(c);
        }
        public void Write(int x, int y, Draw d)
        {
            //Screen.Attr = Out.GetAttribute(d, false);
            Screen.Move(y, x);
            Screen.AddW((char)d);
        }

        public void Write(int x, int y, ReadOnlySpan<char> str)
        {
            Screen.Move(y, x);
            
            for (int i = 0; i < str.Length; i++)
            {
                Screen.AddW(str[i]);
            }
        }
        public void Write(int x, int y, ReadOnlySpan<char> str, uint attribute)
        {
            Screen.Attr = attribute;
            Screen.Move(y, x);
            
            for (int i = 0; i < str.Length; i++)
            {
                Screen.AddW(str[i]);
            }
        }
        
        public void Append(char c) => Screen.AddW(c);
        public void Append(ReadOnlySpan<char> str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Screen.AddW(str[i]);
            }
        }
        
        public void RenderBoxD(int x, int y, int w, int h)
        {
            Screen.Attr = Out.GetAttribute(Colours.Brown);
            Screen.Move(y, x);
            Screen.AddW((char)Draw.WallTL);
            RenderLineH(x + 1, y, (char)Draw.WallH, w - 2);
            Screen.AddW((char)Draw.WallTR);
            RenderLineV(x, y + 1, (char)Draw.WallV, h - 2);
            RenderLineV(x + w - 1, y + 1, (char)Draw.WallV, h - 2);
            Screen.Move(y + h - 1, x);
            Screen.AddW((char)Draw.WallBL);
            RenderLineH(x + 1, y + h - 1, (char)Draw.WallH, w - 2);
            Screen.AddW((char)Draw.WallBR);
        }
        public void RenderBoxD(RectangleI bounds)
            => RenderBoxD(bounds.X, bounds.Y, bounds.Width, bounds.Height);

        public void RenderLineV(int x, int y, char c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Screen.Move(y + i, x);
                Screen.AddW(c);
            }
        }
        public void RenderLineH(int x, int y, char c, int n)
        {
            Screen.Move(y, x);
            for (int i = 0; i < n; i++)
            {
                Screen.AddW(c);
            }
        }

        public char Read(int x, int y) => Screen.ReadOutputCharW(y, x);

        public void Fill(RectangleI bounds, char c)
        {
            for (int i = 0; i < bounds.Height; i++)
            {
                RenderLineH(bounds.X, bounds.Y + i, c, bounds.Width);
            }
        }

        public void Clear() => Screen.Clear();
        
        public int ReadKeyInput() => Screen.GetChar();
        public void ClearLine(int line)
        {
            Screen.Move(line, 0);
            Screen.ClearToEol();
        }
        
        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed) { return; }
            
            Curses.EndWin();
            
            GC.SuppressFinalize(this);
        }
    }
}