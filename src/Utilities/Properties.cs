using CursesSharp;
using Zene.Structs;

namespace RogueMod
{
    public sealed class Properties
    {
        public Vector2I Size { get; } = (80, 25);
        public int CurtainTime { get; } = 1500;
        public int ThrowTime { get; } = 55;
        
        public int StartingHP { get; } = 12;
        public int StartingStrength { get; } = 16;
        public int BackpackSize { get; } = 23;
        public int StomachSize { get; } = 100;
        public string Fruit { get; } = "slime mold";
        
        public int MaxRooms { get; } = 9;
        public int MinRooms { get; } = 5;
        public int DoorLockedProb { get; } = 10;
        public int DoorHiddenProb { get; } = 7;
    }
    
    public class Controls
    {
        public const int LastMessage = Keys.F0 + 4;
        public const int LastMessageB = 'R' & 31;
        public const int ControlVis = Keys.F0 + 1;
        public const int SymbolVis = Keys.F0 + 2;
        public const int Inventory = 'i';
        public const int Drop = 'd';
        public const int Repeat = 'a';
        public const int Wear = 'W';
        public const int TakeOff = 'T';
        public const int PutOnRing = 'P';
        public const int RemoveRing = 'R';
        public const int Wield = 'w';
        public const int Throw = 't';
        public const int RepeatB = Keys.F0 + 3;
    }
}