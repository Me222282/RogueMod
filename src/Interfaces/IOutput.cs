using System;
using Zene.Structs;

namespace RogueMod
{
    public interface IOutput
    {
        public Vector2I Size { get; }
        
        public void Write(int x, int y, char c);
        public void Write(int x, int y, char c, uint attribute);
        public void Write(int x, int y, Draw d);
        public void Write(int x, int y, ReadOnlySpan<char> str);
        public void Write(int x, int y, ReadOnlySpan<char> str, uint attribute);
        
        public void Append(char c);
        public void Append(ReadOnlySpan<char> str);
        
        public void RenderBoxD(int x, int y, int w, int h);
        public void RenderBoxD(RectangleI bounds);
        public void RenderLineV(int x, int y, char c, int n);
        public void RenderLineH(int x, int y, char c, int n);
        
        public char Read(int x, int y);
        
        public void Fill(RectangleI bounds, char c);
        
        public void Clear();
        
        public int ReadKeyInput();
        //public void MovePointer(int x, int y);
        //public void ClearToEnd();
    }
}