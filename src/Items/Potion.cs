using System;

namespace RogueMod
{
    public enum PotionType
    {
        Confusion,
        Paralysis,
        Posion,
        GainStrength,
        SeeInvisable,
        Healing,
        MonsterDetection,
        MagicDetection,
        RaiseLevel,
        ExtraHealing,
        HasteSelf,
        RestorStrength,
        Blindness,
        ThirstQuenching,
        MaxValue
    }
    
    public class Potion : IItem
    {
        private static readonly string[] _names = new string[(int)PotionType.MaxValue]
        {
            "confusion",
            "paralysis",
            "poison",
            "gain strength",
            "see invisible",
            "healing",
            "monster detection",
            "magic detection",
            "raise level",
            "extra healing",
            "haste self",
            "restore strength",
            "blindness",
            "thirst quenching"
        };
        private static readonly int[] _values = new int[(int)PotionType.MaxValue]
        {
            5, 5, 5, 150, 100, 130, 130, 105, 250, 200, 190, 130, 5, 5
        };
        
        public Potion(PotionType potion)
        {
            PotionType = potion;
        }
        
        public PotionType PotionType { get; }
        public ItemType Type => ItemType.Potion;
        public Damage Attack => throw new NotImplementedException();
        
        public int Value => _values[(int)PotionType];
        
        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => true;

        public string ToString(Rogue game, bool plural)
        {
            string p = plural ? "s" : "";
            string name = game.NameMaps.Potions[(int)PotionType];
            
            if (IsKnown(game))
            {
                return $"potion{p} of {_names[(int)PotionType]}({name})";
            }
            
            string playerName = game.Discoveries.PotionNames[(int)PotionType];
            if (playerName != null && playerName != "")
            {
                return $"potion{p} called {playerName}({name})";
            }
            
            return $"{name} potion";
        }
        
        public bool IsKnown(Rogue game) => game.Discoveries.Potions[(int)PotionType];
        public void MakeKnown(Rogue game)
        {
            game.Discoveries.Potions[(int)PotionType] = true;
        }

        public void Effect(ICharacter character, Stats tStats, Rogue game)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return obj is Potion p &&
                p.PotionType == PotionType;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(PotionType, Value, Cursed, Quantity);
        }
    
        public static Potion Create()
            => new Potion((PotionType)Program.RNG.Next((int)PotionType.MaxValue));
    }
}