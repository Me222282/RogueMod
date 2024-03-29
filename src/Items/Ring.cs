using System;

namespace RogueMod
{
    public enum RingType
    {
        None,
        AddDamage,
        AddStrength,
        SustainStrength,
        Search,
        SeeInvisable,
        Aggrovate,
        AddHitChance,
        Stealth,
        Healing,
        SlowDigest,
        Teleport,
        SustainArmour,
        Protect,
        MaxValue
    }
    
    public class Ring : IItem
    {
        private static readonly string[] _names = new string[(int)RingType.MaxValue]
        {
            "adornment",
            "increase damage",
            "add strength",
            "sustain strength",
            "searching",
            "see invisible",
            "aggravate monster",
            "dexterity",
            "stealth",
            "regeneration",
            "slow digestion",
            "teleportation",
            "maintain armour",
            "protection"
        };
        private static readonly int[] _values = new int[(int)RingType.MaxValue]
        {
            10, 400, 400, 280, 420, 310, 10, 440, 470, 460, 240, 30, 380, 400
        };
        
        public Ring(RingType r)
        {
            RingType = r;
        }
        
        public RingType RingType { get; }
        public double Mod { get; set; }
        
        public ItemType Type => ItemType.Ring;
        public Damage Attack => Damage.Default;
        
        public int Value => _values[(int)RingType];
        
        char IEntity.Graphic => (char)Draw.Ring;
        
        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => false;

        public string ToString(Discoveries dics, bool plural)
        {
            string p = plural ? "s" : "";
            string name = dics.RingNames[(int)RingType];
            
            if (IsKnown(dics))
            {
                string b = RingBonus(RingType) ? $" {Program.GetMod((int)Mod)}" : "";
                return $"ring{p} of {_names[(int)RingType]}{b}({name})";
            }
            
            string playerName = dics.RingGuesses[(int)RingType];
            if (playerName != null && playerName != "")
            {
                return $"ring{p} called {playerName}";
            }
            
            return $"{name} ring{p}";
        }
        private static bool RingBonus(RingType ring)
        {
            return ring == RingType.Protect ||
                ring == RingType.AddStrength ||
                ring == RingType.AddDamage ||
                ring == RingType.AddHitChance;
        }
        
        public bool IsKnown(Discoveries dics) => dics.IsRings[(int)RingType];
        public void MakeKnown(Discoveries dics)
        {
            dics.IsRings[(int)RingType] = true;
        }
        
        public IItem Copy() => new Ring(RingType);
        
        public void Effect(ICharacter character, IRogue game)
        {
            throw new NotImplementedException();
        }
        
        public static Ring Create()
        {
            Ring r = new Ring((RingType)Program.RNG.Next((int)RingType.MaxValue));
            
            if (!RingBonus(r.RingType)) { return r; }
            
            r.Mod = "1d4".Roll(true);
            if (r.Mod < 0)
            {
                r.Cursed = true;
            }
            return r;
        }
    }
}