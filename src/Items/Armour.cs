using System;

namespace RogueMod
{
    public enum ArmourType
    {
        Leather,
        StuddedLeather,
        Ring,
        Scale,
        Chain,
        Banded,
        Splint,
        Plate,
        MaxValue
    }
    
    public class Armour : IItem
    {
        private Armour(string name, int armourClass)
        {
            ArmourClass = armourClass;
            Name = name;
        }
        
        public string Name { get; }
        public int ArmourClass { get; }
        public int Modifier { get; set; }

        public ItemType Type => ItemType.Armour;
        public Damage Attack { get; } = new Damage();

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Discovered { get; set; }
        public bool Stackable => false;

        public int Value => 30 * (ArmourClass + Modifier);
        
        char IEntity.Graphic => (char)Draw.Armour;
        
        public string ToString(IRogue game, bool plural)
        {
            string m = "", discover = "";
            
            if (Discovered)
            {
                m = $"{Program.GetMod(Modifier)} ";
                discover = $" [armour class {ArmourClass + Modifier}]";
            }
            
            string arm = "mail";
            if (Name == "leather" || Name == "studded leather")
            {
                arm = "armour";
            }
            
            return $"{m}{Name} {arm}{discover}";
        }
        
        public bool IsKnown(IRogue game) => Discovered;
        public void MakeKnown(IRogue game) => Discovered = true;

        public void Effect(ICharacter character, IRogue game) { }
        
        public IItem Copy() => new Armour(Name, ArmourClass) { Modifier = Modifier };
        
        public static Armour Create(ArmourType armour)
        {
            return armour switch
            {
                ArmourType.Leather => new Armour("leather", 3),
                ArmourType.StuddedLeather => new Armour("studded leather", 4),
                ArmourType.Ring => new Armour("ring", 4),
                ArmourType.Scale => new Armour("scale", 5),
                ArmourType.Chain => new Armour("chain", 6),
                ArmourType.Banded => new Armour("banded", 7),
                ArmourType.Splint => new Armour("splint", 7),
                ArmourType.Plate => new Armour("plate", 8),
                _ => throw new Exception()
            };
        }
        public static Armour Create()
            => Create((ArmourType)Program.RNG.Next((int)ArmourType.MaxValue));
    }
}