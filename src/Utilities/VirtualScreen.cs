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
            _map.Fill(new Unit(' ', false));
            _vis = new bool[size.Y - 3, size.X];
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
                
                Unit u = value ? _map[y + 1, x] : new Unit(' ', false);
                Stdscr.Attr = u.Attribute;
                Stdscr.AddW(y + 1, x, u.Character);
            }
        }
        private bool Visable(int x, int y) => y < 0 || y >= Size.Y - 3 || _vis[y, x];
        
        // Add 1 to y to leave space for Messages
        public void Write(int x, int y, char ch)
        {
            Unit u = new Unit(ch, _map[y + 1, x].Grey);
            _map[y + 1, x] = u;
            
            if (!PrintDirect || !Visable(x, y)) { return; }
            
            Stdscr.Attr = u.Attribute;
            Stdscr.AddW(y + 1, x, u.Character);
        }
        public void Write(int x, int y, Draw ch)
        {
            Unit u = new Unit(ch, _map[y + 1, x].Grey);
            _map[y + 1, x] = u;
            
            if (!PrintDirect || !Visable(x, y)) { return; }
            
            Stdscr.Attr = u.Attribute;
            Stdscr.AddW(y + 1, x, u.Character);
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
        
        public char Read(int x, int y) => _map[y + 1, x].Character;
        
        public void PrintLine(int line)
        {
            Stdscr.Move(line, 0);
            for (int i = 0; i < Size.X; i++)
            {
                Unit u = Visable(i, line - 1) ? _map[line, i] : new Unit(' ', false);
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
                    Unit u = Visable(x, y - 1) ? _map[y, x] : new Unit(' ', false);
                    Stdscr.Attr = u.Attribute;
                    Stdscr.AddW(u.Character);
                }
            }
        }
        
        public void Clear()
        {
            _map.Fill(new Unit(' ', false));
            if (PrintDirect) { Print(); }
        }
        
        public void FillCorridors(CorridorManager cm)
        {
            for (int i = 0; i < cm.Corridors.Length; i++)
            {
                Corridor c = cm.Corridors[i];
                Vector2I pos = c.Position;
                pos.Y++;
                
                for (int j = 0; j < c.Length; j++)
                {
                    _map[pos.Y, pos.X].Grey = true;
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
        
        private struct Unit
        {
            public Unit(char c, bool grey)
            {
                Character = c;
                Attribute = Out.GetAttribute((Draw)c, grey);
                Grey = grey;
            }
            public Unit(Draw d, bool grey)
            {
                Character = (char)d;
                Attribute = Out.GetAttribute(d, grey);
                Grey = grey;
            }
            
            public char Character;
            public uint Attribute;
            public bool Grey;
        }
    }
}