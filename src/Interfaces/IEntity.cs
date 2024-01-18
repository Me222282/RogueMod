using System;
using Zene.Structs;

namespace RogueMod
{
    public interface IEntity
    {
        public char Graphic { get; }
        public bool Character { get; }
    }
}