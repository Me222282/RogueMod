using System;
using Zene.Structs;

namespace RogueMod
{
    public sealed class GameOutput : IGameOutput
    {
        public GameOutput(Vector2I size, int layers)
        {
            _screens = new Screen[layers];
            _vis = new bool[size.Y - 2, size.X];
            Size = size;
            
            for (int i = 0; i < layers; i++)
            {
                _screens[i] = new Screen(this, i);
            }
        }
        
        public IOutput DirectOut { get; set; }
        public bool PrintDirect => DirectOut is not null;
        public int Layers => _screens.Length;
        
        public Vector2I Size { get; }
        private Screen[] _screens;
        private bool[,] _vis;
        
        public IOutput this[int layer] => _screens[layer];
        
        public bool this[int x, int y]
        {
            get => _vis[y, x];
            set
            {
                _vis[y, x] = value;
                if (!PrintDirect) { return; }
                
                Unit u = value ? GetTop(x, y) : new Unit(' ', false);
                WriteDirect(x, y, u.Character, u.Attribute);
            }
        }
        private bool Visable(int x, int y) => y < 0 || y >= Size.Y - 2 || _vis[y, x];
        
        private void WriteDirect(int x, int y, char c, Attribute a)
        {
            DirectOut.Write(x, y + 1, c, a);
        }
        
        private bool TopLayer(int x, int y, int layer)
        {
            for (int i = layer + 1; i < _screens.Length; i++)
            {
                if (!_screens[i].Populated(x, y)) { continue; }
                
                return false;
            }
            
            return true;
        }
        private bool BottomLayer(int x, int y, int layer)
        {
            for (int i = layer - 1; i >= 0; i--)
            {
                if (!_screens[i].Populated(x, y)) { continue; }
                
                return false;
            }
            
            return true;
        }
        private bool PrintLayerBelow(int x, int y, int layer)
        {
            for (int i = layer - 1; i >= 0; i--)
            {
                Unit u = _screens[i].ReadUnit(x, y);
                if (u.Character == ' ') { continue; }
                
                if (DirectOut is not null)
                {
                    DirectOut.Write(x, y + 1, u.Character, u.Attribute);
                }
                
                return true;
            }
            
            return false;
        }
        private Unit GetTop(int x, int y)
        {
            for (int i = _screens.Length - 1; i >= 0; i--)
            {
                Unit c = _screens[i].ReadUnit(x, y);
                if (c.Character == ' ') { continue; }
                
                return c;
            }
            
            return new Unit(' ', false);
        }
        
        public void PrintLine(int line)
        {
            if (!PrintDirect) { return; }
            
            if (line < 0 || line > Size.Y) { return; }
            
            for (int i = 0; i < Size.X; i++)
            {
                Unit u = Visable(i, line) ? GetTop(i, line) : new Unit(' ', false);
                DirectOut.Write(i, line + 1, u.Character, u.Attribute);
            }
        }
        public void PrintAll()
        {
            if (!PrintDirect) { return; }
            
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Unit u = Visable(x, y) ? GetTop(x, y) : new Unit(' ', false);
                    DirectOut.Write(x, y + 1, u.Character, u.Attribute);
                }
            }
        }
        
        public void ClearAll()
        {
            for (int i = 0; i < _screens.Length; i++)
            {
                _screens[i].ClearW();
            }
            
            _vis.Fill(false);
            
            if (PrintDirect) { DirectOut.Fill(new RectangleI((0, 1), Size), ' '); }
        }
        
        public void FillCorridorMap(ICorridorManager cm, int layer)
        {
            Screen o = _screens[layer];
            
            foreach (Corridor c in cm)
            {
                Vector2I pos = c.Position;
                
                for (int j = 0; j < c.Length; j++)
                {
                    o.Grey(pos.X, pos.Y);
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
                Attribute = RogueMod.Attribute.GetAttribute((Draw)c, grey);
                Grey = grey;
            }
            public Unit(Draw d, bool grey)
            {
                Character = (char)d;
                Attribute = RogueMod.Attribute.GetAttribute(d, grey);
                Grey = grey;
            }
            
            public char Character;
            public Attribute Attribute;
            public bool Grey;
        }
        
        private class Screen : IOutput
        {
            public Screen(GameOutput source, int layer)
            {
                Size = source.Size;
                _source = source;
                _layer = layer;
                _map = new Unit[Size.Y, Size.X];
                _map.Fill(new Unit(' ', false));
            }
            
            private GameOutput _source;
            
            public Vector2I Size { get; }
            
            private int _layer;
            private Unit[,] _map;
            
            public bool Populated(int x, int y) => _map[y, x].Character != ' ';
            public void Grey(int x, int y) => _map[y, x].Grey = true;
            
            // Add one to leave gap for messages
            public void Write(int x, int y, char ch)
                => Write(x, y, (Draw)ch);
            public void Write(int x, int y, Draw ch)
            {
                Unit u = new Unit(ch, _map[y, x].Grey);
                _map[y, x] = u;
                
                if (!_source.PrintDirect || !_source._vis[y, x] || !_source.TopLayer(x, y, _layer) ||
                    ((char)ch == ' ' && _source.PrintLayerBelow(x, y, _layer))) { return; }
                
                _source.WriteDirect(x, y, u.Character, u.Attribute);
            }
            public void Write(int x, int y, char ch, Attribute attribute)
            {
                Unit u = new Unit(ch, _map[y, x].Grey);
                if (u.Attribute == Attribute.Normal)
                {
                    u.Attribute = attribute;
                }
                _map[y, x] = u;
                
                if (!_source.PrintDirect || !_source._vis[y, x] || !_source.TopLayer(x, y, _layer) ||
                    ((char)ch == ' ' && _source.PrintLayerBelow(x, y, _layer))) { return; }
                
                _source.WriteDirect(x, y, u.Character, u.Attribute);
            }
            public void Write(int x, int y, ReadOnlySpan<char> str)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    Write(x + i, y, str[i]);
                }
            }
            public void Write(int x, int y, ReadOnlySpan<char> str, Attribute attribute)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    Write(x + i, y, str[i], attribute);
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
            public void RenderLineV(int x, int y, char c, int n, Attribute attribute = new Attribute())
            {
                for (int i = 0; i < n; i++)
                {
                    Write(x, y + i, c, attribute);
                }
            }
            public void RenderLineH(int x, int y, char c, int n, Attribute attribute = new Attribute())
            {
                for (int i = 0; i < n; i++)
                {
                    Write(x + i, y, c, attribute);
                }
            }
            
            public char Read(int x, int y) => _map[y, x].Character;
            public Attribute ReadAttribute(int x, int y) => _map[y, x].Attribute;
            public Unit ReadUnit(int x, int y) => _map[y, x];
            //public Attribute GetAttribute(int x, int y) => _map[y, x].Attribute;
            
            public void Fill(RectangleI bounds, char ch)
            {
                for (int i = 0; i < bounds.Height; i++)
                {
                    RenderLineH(bounds.X, bounds.Y + i, ch, bounds.Width);
                }
            }
            
            public void Clear()
            {
                ClearW();
                
                if (!_source.PrintDirect) { return; }
                
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int x = 0; x < Size.X; x++)
                    {
                        if (!_source.TopLayer(x, y, _layer)) { continue; }
                        _source.WriteDirect(x, y, ' ', Attribute.Normal);
                    }
                }
            }
            public void ClearW()
            {
                _map.Fill(new Unit(' ', false));
            }
            
            public int ReadKeyInput() => _source.DirectOut?.ReadKeyInput() ?? -1;
            public string ReadString(int n) => _source.DirectOut?.ReadString(n);
            public void Pause(int ms) => _source.DirectOut?.Pause(ms);
            
            public void Append(char c, Attribute attribute) => throw new NotSupportedException();
            public void Append(ReadOnlySpan<char> str, Attribute attribute) => throw new NotSupportedException();
            public void ClearLine(int line) => RenderLineH(0, line, ' ', Size.X);
        }
    }
}