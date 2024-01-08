using System;

namespace RogueMod
{
    public struct Damage
    {
        public Damage(Dice melee, Dice range, int mod = 0, int hit = 0)
        {
            Melee = melee;
            Range = range;
            Modifier = mod;
            HitChance = hit;
        }
        
        public Dice Melee { get; set; }
        public Dice Range { get; set; }
        
        public int Modifier { get; set; }
        public int HitChance { get; set; }
        
        public static Damage Default { get; } = new Damage("1d1", "1d1", 0, 0);
        public static Damage Zero { get; } = new Damage("0d0", "0d0", 0, 0);
        
        public static bool operator ==(Damage l, Damage r) => l.Equals(r);
        public static bool operator !=(Damage l, Damage r) => !l.Equals(r);

        public override bool Equals(object obj)
        {
            return obj is Damage d &&
                d.Melee == Melee &&
                d.Range == Range &&
                d.Modifier == Modifier &&
                d.HitChance == HitChance;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Melee, Range, Modifier, HitChance);
        }
    }
}