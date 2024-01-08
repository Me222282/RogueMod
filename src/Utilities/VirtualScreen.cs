using System;
using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public sealed class VirtualScreen
    {
        public VirtualScreen(Vector2I size)
        {
            _map = new Unit[size.Y, size.X];
            _vis = new bool[size.Y, size.X];
            Size = size;
        }
        
        public bool PrintDirect { get; set; }
        
        public Vector2I Size { get; }
        private Unit[,] _map;
        private bool[,] _vis;
        
        public bool this[int x, int y]
        {
            get => _vis[y, x];
            set
            {
                _vis[y, x] = value;
                if (!PrintDirect) { return; }
                
                Unit u = value ? _map[y, x] : new Unit(' ');
                Stdscr.Attr = u.Attribute;
                Stdscr.AddW(u.Character);
            }
        }
        
        public void Write(int x, int y, char ch)
        {
            Unit u = new Unit(ch);
            _map[y, x] = u;
            
            if (!PrintDirect || !_vis[y, x]) { return; }
            
            Stdscr.Attr = u.Attribute;
            Stdscr.AddW(y, x, u.Character);
        }
        public void Write(int x, int y, Draw ch)
        {
            Unit u = new Unit(ch);
            _map[y, x] = u;
            
            if (!PrintDirect || !_vis[y, x]) { return; }
            
            Stdscr.Attr = u.Attribute;
            Stdscr.AddW(y, x, u.Character);
        }
        public void Write(int x, int y, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Write(x + i, y, str[i]);
            }
        }
        public void Write(int x, int y, char[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Write(x + i, y, str[i]);
            }
        }
        public void Write(int x, int y, ReadOnlySpan<char> str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Write(x + i, y, str[i]);
            }
        }
        
        public void RenderBoxD(int x, int y, int w, int h)
        {
            Write(x, y, Draw.WallTL);
            RenderLineH(x + 1, y, (char)Draw.WallH, w - 2);
            Write(x + w - 1, y, (char)Draw.WallTR);
            RenderLineV(x, y + 1, (char)Draw.WallV, h - 2);
            RenderLineV(x + w - 1, y + 1, (char)Draw.WallV, h - 2);
            Write(x, y + h - 1, (char)Draw.WallBL);
            RenderLineH(x + 1, y + h - 1, (char)Draw.WallH, w - 2);
            Write(x + w - 1, y + h - 1, (char)Draw.WallBR);
        }
        public void RenderBoxD(RectangleI bounds)
            => RenderBoxD(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        public void RenderLineV(int x, int y, char c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Write(x, y + i, c);
            }
        }
        public void RenderLineH(int x, int y, char c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Write(x + i, y, c);
            }
        }
        
        public void Fill(int x, int y, int w, int h, char ch)
        {
            for (int i = 0; i < h; i++)
            {
                RenderLineH(x, y + i, ch, w);
            }
        }
        
        public char Read(int x, int y) => _map[y, x].Character;
        
        public void PrintLine(int line)
        {
            Stdscr.Move(line, 0);
            for (int i = 0; i < Size.X; i++)
            {
                Unit u = _vis[line, i] ? _map[line, i] : new Unit(' ');
                Stdscr.Attr = u.Attribute;
                Stdscr.AddW(u.Character);
            }
        }
        public void Print()
        {
            for (int y = 0; y < Size.Y; y++)
            {
                Stdscr.Move(y, 0);
                
                for (int x = 0; x < Size.X; x++)
                {
                    Unit u = _vis[y, x] ? _map[y, x] : new Unit(' ');
                    Stdscr.Attr = u.Attribute;
                    Stdscr.AddW(u.Character);
                }
            }
        }
        
        private struct Unit
        {
            public Unit(char c)
            {
                Character = c;
                Attribute = Out.GetAttribute((Draw)c);
            }
            public Unit(Draw d)
            {
                Character = (char)d;
                Attribute = Out.GetAttribute(d);
            }
            
            public char Character;
            public uint Attribute;
        }
    }
}