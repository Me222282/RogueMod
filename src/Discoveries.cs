namespace RogueMod
{
    public class Discoveries
    {
        public bool[] Rings { get; } = new bool[(int)RingType.MaxValue];
        public string[] RingNames { get; } = new string[(int)RingType.MaxValue];
        
        public bool[] Scrolls { get; } = new bool[(int)ScrollType.MaxValue];
        public string[] ScrollNames { get; } = new string[(int)ScrollType.MaxValue];
        
        public bool[] Potions { get; } = new bool[(int)PotionType.MaxValue];
        public string[] PotionNames { get; } = new string[(int)PotionType.MaxValue];
        
        public bool[] Sticks { get; } = new bool[(int)StaffType.MaxValue];
        public string[] StickNames { get; } = new string[(int)StaffType.MaxValue];
    }
}