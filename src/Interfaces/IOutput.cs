using System;
using Zene.Structs;

namespace RogueMod
{
    public interface IOutput
    {
        public Vector2I Size { get; }
        
        public void Write(int x, int y, char c);
        public void Write(int x, int y, char c, Attribute attribute);
        public void Write(int x, int y, Draw d);
        public void Write(int x, int y, ReadOnlySpan<char> str);
        public void Write(int x, int y, ReadOnlySpan<char> str, Attribute attribute);
        
        public void Append(char c, Attribute attribute = new Attribute());
        public void Append(ReadOnlySpan<char> str, Attribute attribute = new Attribute());
        
        public void RenderLineV(int x, int y, char c, int n, Attribute attribute = new Attribute());
        public void RenderLineH(int x, int y, char c, int n, Attribute attribute = new Attribute());
        
        public char Read(int x, int y);
        public Attribute ReadAttribute(int x, int y);
        
        public void Fill(RectangleI bounds, char c);
        
        public void Clear();
        
        public int ReadKeyInput();
        public string ReadString(int n);
        public void ClearLine(int line);
        public void Pause(int ms);
    }
}