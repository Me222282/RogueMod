using Zene.Structs;

namespace RogueMod
{
    public sealed class Player : ICharacter
    {
        private static readonly string[] _levelNames = new string[]
        {
            "",
            "Guild Novice",
            "Apprentice",
            "Journeyman",
            "Adventurer",
            "Fighter",
            "Warrior",
            "Rogue",
            "Champion",
            "Master Rogue",
            "Warlord",
            "Hero",
            "Guild Master",
            "Dragonlord",
            "Wizard",
            "Rogue Geek",
            "Rogue Addict",
            "Schmendrick",
            "Gunfighter",
            "Time Waster",
            "Bug Chaser"
        };
        
        public Player()
        {
            Backpack = new Backpack(23);
        }
        
        public double HP { get; set; }
        public double MaxHP { get; private set; }
        public int Level { get; private set; }
        public double Strength { get; set; } = 16;
        public int ArmourClass
        {
            get
            {
                Armour a = (Armour)Backpack.Wearing;
                
                if (a == null) { return 1; }
                return a.ArmourClass + a.Modifier;
            }
        }
        public EntityFlags Flags { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public bool PickupItems => true;

        public Backpack Backpack { get; }

        public Vector2I Position { get; set; }
        public bool Seen
        {
            get => true;
            set { }
        }
        public char Graphic => (char)Draw.Player;
        public char UnderChar { get; set; }
        
        public int Gold
        {
            get => Backpack.Gold;
            set => Backpack.Gold = value;
        }
        public string LevelName => _levelNames[Level];

        public void IncreaseLevel()
        {
            if (Level == _levelNames.Length - 1) { return; }
            
            Level++;
            MaxHP += 8;
            HP += 8;
        }

        public void Interact(Rogue game)
        {
            
        }
    }
}