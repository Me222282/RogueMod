using System;
using Zene.Structs;

namespace RogueMod
{
    public interface IDisplayable
    {
        public char Graphic { get; }
        public char UnderChar { get; set; }
    }
}