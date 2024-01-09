using System;

namespace RogueMod
{
    public enum WeaponType
    {
        Mace,
        ShortBow,
        Arrow,
        CrossBow,
        Dart,
        LongSword,
        DoubleSword,
        Dagger,
        Spear,
        Bolt,
        MaxValue
    }
    
    public class Weapon : IItem
    {
        private static readonly string[] _names = new string[(int)WeaponType.MaxValue]
        {
            "mace",
            "short bow",
            "arrow",
            "corssbow",
            "dart",
            "long sword",
            "double bladded sword",
            "dagger",
            "spear",
            "crossbow bolt"
        };
        
        private Weapon(WeaponType weapon, Dice mDamage, Dice rDamage)
        {
            Attack = new Damage(mDamage, rDamage);
            WeaponType = weapon;
        }
        
        public WeaponType WeaponType { get; }
        public ItemType Type => ItemType.Weapon;
        public Damage Attack { get; }
        
        public int Value => 0;
        
        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Discovered { get; set; }
        public bool Stackable => WeaponType == WeaponType.Arrow || 
                                WeaponType == WeaponType.Dart ||
                                WeaponType == WeaponType.Bolt;
        
        public string ToString(Rogue game, bool plural)
        {
            string discover = "";
            
            if (Discovered)
            {
                discover = $"{Program.GetMod(Attack.HitChance)} {Program.GetMod(Attack.Modifier)} ";
            }
            
            string p = plural ? "s" : "";
            return $"{discover}{_names[(int)WeaponType]}{p}";
        }
        
        public bool IsKnown(Rogue game) => Discovered;
        public void MakeKnown(Rogue game) => Discovered = true;

        public override bool Equals(object obj)
        {
            return obj is Weapon w &&
                w.WeaponType == WeaponType &&
                w.Attack == Attack &&
                w.Cursed == Cursed;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(WeaponType, Attack, Cursed, Quantity, Discovered, Stackable);
        }
        
        public void Effect(ICharacter character, Stats tStats, Rogue game)
        {
            throw new NotImplementedException();
        }
        
        public static Weapon Create(WeaponType weapon)
        {
            return weapon switch
            {
                WeaponType.Mace => new Weapon(weapon, "2d4", "1d3"),
                WeaponType.ShortBow => new Weapon(weapon, "1d1", "1d1"),
                WeaponType.Arrow => new Weapon(weapon, "1d1", "2d3"),
                WeaponType.CrossBow => new Weapon(weapon, "1d1", "1d1"),
                WeaponType.Dart => new Weapon(weapon, "1d1", "1d3"),
                WeaponType.LongSword => new Weapon(weapon, "3d4", "1d2"),
                WeaponType.DoubleSword => new Weapon(weapon, "4d4", "1d2"),
                WeaponType.Dagger => new Weapon(weapon, "1d6", "1d4"),
                WeaponType.Spear => new Weapon(weapon, "2d3", "1d6"),
                WeaponType.Bolt => new Weapon(weapon, "1d2", "2d5"),
                _ => throw new Exception()
            };
        }
    }
}