using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public sealed class Properties
    {
        public Vector2I Size { get; } = (80, 25);
        public int CurtainTime { get; } = 1800;
        
        public int StartingHP { get; } = 12;
        public int StartingStrength { get; } = 16;
        public int BackpackSize { get; } = 23;
    }
    
    public class Controls
    {
        public const int LastMessage = Keys.F0 + 4;
        public const int ControlVis = Keys.F0 + 1;
        public const int SymbolVis = Keys.F0 + 2;
        public const int Inventory = 'i';
        public const int Drop = 'd';
        public const int Repeat = 'a';
        public const int RepeatB = Keys.F0 + 3;
    }
}