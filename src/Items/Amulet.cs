using System;

namespace RogueMod
{
    public class Amulet : IItem
    {
        public ItemType Type => ItemType.Amulet;
        public Damage Attack => Damage.Zero;

        public bool Cursed { get; set; }
        public int Quantity { get; set; } = 1;
        public bool Stackable => false;

        public int Value => 1000;
        
        public string ToString(Rogue game, bool plural) => "The Amulet of Yendor";
        
        public bool IsKnown(Rogue game) => true;
        public void MakeKnown(Rogue game) { }
        
        public override bool Equals(object obj) => obj is Amulet a;
        public override int GetHashCode() => HashCode.Combine(Type);
        
        public void Effect(ICharacter character, Stats tStats, Rogue game)
        {
            throw new System.NotImplementedException();
        }
    }
}