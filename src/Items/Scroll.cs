using System;

namespace RogueMod
{
    public enum ScrollType
    {
        MonsterConfusion,
        MagicMapping,
        HoldMonster,
        Sleep,
        EnchantArmour,
        Idnentify,
        ScareMonster,
        FoodDetection,
        Teleportation,
        EnchantWeapon,
        CreateMonster,
        RemoveCurse,
        Aggrovate,
        Blank,
        Vorpalise,
        MaxValue
    }
    
    public class Scroll : IItem
    {
        private static readonly string[] _names = new string[(int)ScrollType.MaxValue]
        {
            "monster confusion",
            "magic mapping",
            "hold monster",
            "sleep",
            "enchant armour",
            "identify",
            "scare monster",
            "food detection",
            "teleportation",
            "enchant weapon",
            "create monster",
            "remove curse",
            "aggravate monsters",
            "blank paper",
            "vorpalise weapon"
        };
        private static readonly int[] _values = new int[(int)ScrollType.MaxValue]
        {
            140, 150, 180, 5, 160, 100, 200, 50, 165, 150, 75, 105, 20, 5, 300
        };
        
        public Scroll(ScrollType scroll)
        {
            ScrollType = scroll;
        }
        
        public ScrollType ScrollType { get; }
        public ItemType Type => ItemType.Scroll;
        public Damage Attack => Damage.Zero;

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => true;

        public int Value => _values[(int)ScrollType];
        
        char IEntity.Graphic => (char)Draw.Scroll;
        
        public string ToString(Discoveries dics, bool plural)
        {
            string p = plural ? "s" : "";
            string name = dics.ScrollNames[(int)ScrollType];
            
            if (IsKnown(dics))
            {
                return $"scroll{p} of {_names[(int)ScrollType]}({name})";
            }
            
            string playerName = dics.StickGuesses[(int)ScrollType];
            if (playerName != null && playerName != "")
            {
                return $"scroll{p} called {playerName}";
            }
            
            return $"scroll{p} titled '{name}'";
        }
        
        public bool IsKnown(Discoveries dics) => dics.IsScrolls[(int)ScrollType];
        public void MakeKnown(Discoveries dics)
        {
            dics.IsScrolls[(int)ScrollType] = true;
        }
        
        public void Effect(ICharacter character, IRogue game)
        {
            throw new NotImplementedException();
        }
        
        public override bool Equals(object obj)
        {
            return obj is Scroll s &&
                s.ScrollType == ScrollType &&
                s.Cursed == Cursed;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(ScrollType, Value, Cursed, Quantity);
        }
        
        public IItem Copy() => new Scroll(ScrollType);
        
        public static Scroll Create()
            => new Scroll((ScrollType)Program.RNG.Next((int)ScrollType.MaxValue));
    }
}