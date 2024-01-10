namespace RogueMod
{
    public enum StaffType
    {
        Light,
        Stricking,
        Lightning,
        Fire,
        Cold,
        Polymorph,
        MagicMissle,
        HasteMonster,
        SlowMonster,
        DrainLife,
        Nothing,
        TeleportAway,
        TeleportTo,
        Cancellation,
        MaxValue
    }
    
    public class Staff : IItem
    {
        private static readonly string[] _names = new string[(int)StaffType.MaxValue]
        {
            "light",
            "striking",
            "lightning",
            "fire",
            "cold",
            "polymorph",
            "magic missile",
            "haste monster",
            "slow monster",
            "drain life",
            "nothing",
            "teleport away",
            "teleport to",
            "cancellation"
        };
        private static readonly int[] _values = new int[(int)StaffType.MaxValue]
        {
            250, 75, 330, 330, 330, 310, 170, 5, 350, 300, 5, 340, 50, 280
        };
        
        public Staff(StaffType staff, Damage attack, int uses)
        {
            StaffType = staff;
            Attack = attack;
            Uses = uses;
        }
        
        public StaffType StaffType { get; }
        public int Uses { get; set; }

        public ItemType Type => ItemType.Staff;
        public Damage Attack { get; }

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => false;

        public int Value => _values[(int)StaffType];

        public string ToString(Rogue game, bool plural)
        {
            string p = plural ? "s" : "";
            string made = game.NameMaps.StickMaterial[(int)StaffType] ? "staff" : "wand";
            string name = game.NameMaps.Sticks[(int)StaffType];
            
            if (IsKnown(game))
            {
                return $"{made}{p} of {_names[(int)StaffType]}[{Uses} charges]({name})";
            }
            
            string playerName = game.Discoveries.StickNames[(int)StaffType];
            if (playerName != null && playerName != "")
            {
                return $"{made}{p} called {playerName}";
            }
            
            return $"{name} {made}{p}";
        }

        public bool IsKnown(Rogue game) => game.Discoveries.Sticks[(int)StaffType];
        public void MakeKnown(Rogue game)
        {
            game.Discoveries.Sticks[(int)StaffType] = true;
        }
        
        public void Effect(ICharacter character, Rogue game)
        {
            Uses--;
        }
        
        public static Staff Create(Rogue game)
        {
            StaffType type = (StaffType)Program.RNG.Next((int)StaffType.MaxValue);
            Damage d = Damage.Default;
            if (game.NameMaps.StickMaterial[(int)type])
            {
                d.Melee = "2d3";
            }
            if (type == StaffType.Stricking)
            {
                d.Melee = "1d8";
                d.HitChance = 100;
                d.Modifier = 3;
            }
            int charges = type == StaffType.Light ? 
                10 + Program.RNG.Next(10) :
                3 + Program.RNG.Next(5);
            
            return new Staff(type, d, charges);
        }
    }
}