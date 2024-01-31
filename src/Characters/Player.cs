using Zene.Structs;

namespace RogueMod
{
    public sealed class Player : IPlayer
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
            Backpack = new Backpack(Program.Properties.BackpackSize);
        }
        
        public double HP { get; set; }
        public double MaxHP { get; private set; } = Program.Properties.StartingHP;
        public int Level { get; private set; }
        public double Strength { get; set; } = Program.Properties.StartingStrength;
        public int ArmourClass
        {
            get
            {
                Armour a = (Armour)Backpack.Wearing;
                
                if (a == null) { return 1; }
                return a.ArmourClass + a.Modifier;
            }
        }
        public EntityFlags Flags { get; set; }
        public bool PickupItems => true;
        
        public int Food { get; set; } = Program.Properties.StomachSize;
        
        public Backpack Backpack { get; }

        public char Graphic => (char)Draw.Player;
        public Vector2I Position { get; set; }
        
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

        public void Action(IRogue game) { }
        public void Damage(IItem source, bool thrown)
        {
            int init = (thrown ? source.Attack.Range : source.Attack.Melee).NewRoll();
        }
    }
}