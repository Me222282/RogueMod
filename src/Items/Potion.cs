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
        public Damage Attack => Damage.Default;
        
        public int Value => _values[(int)PotionType];
        
        char IEntity.Graphic => (char)Draw.Potion;
        
        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => true;

        public string ToString(Discoveries dics, bool plural)
        {
            string p = plural ? "s" : "";
            string name = dics.PotionNames[(int)PotionType];
            
            if (IsKnown(dics))
            {
                return $"potion{p} of {_names[(int)PotionType]}({name})";
            }
            
            string playerName = dics.PotionGuesses[(int)PotionType];
            if (playerName != null && playerName != "")
            {
                return $"potion{p} called {playerName}({name})";
            }
            
            return $"{name} potion";
        }
        
        public bool IsKnown(Discoveries dics) => dics.IsPotions[(int)PotionType];
        public void MakeKnown(Discoveries dics)
        {
            dics.IsPotions[(int)PotionType] = true;
        }

        public void Effect(ICharacter character, IRogue game)
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
        
        public IItem Copy() => new Potion(PotionType);
        
        public static Potion Create()
            => new Potion((PotionType)Program.RNG.Next((int)PotionType.MaxValue));
    }
}